using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Hackathon2024API.Data.Settings;
using Hackathon2024API.DTO.User;
using Hackathon2024API.Interfaces.Services;
using Hackathon2024API.Models;
using Hackathon2024API.Result;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Hackathon2024API.Services
{

	public class AuthSrevice : IAuthService
	{
		private readonly UserManager<User> _userManager;
		private readonly IConfiguration _configuration;

		public AuthSrevice(UserManager<User> userManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_configuration = configuration;
		}
		public async Task<BaseResult<UserDto>> Register(RegisterUserDto dto)
		{
			//TODO валидация пароля. (Pass and PassConfirm)
			var identityUser = new User()
			{
				Email = dto.Email,
				UserName = dto.Login
			};

			var existingUser = await _userManager.FindByEmailAsync(identityUser.Email);
			
			if (existingUser != null)
			{
				return new BaseResult<UserDto>
				{
					ErrorMessage = $"Пользователь с такой почтой уже существует"
				};
			}

			//через встренный identity репозиторий заносим в бд юзера
			var result = await _userManager.CreateAsync(identityUser, dto.Password);

			if (result.Succeeded)
			{
				var userDto = new UserDto
				{
					Response = "Пользователь с почтой " + dto.Email + " успешно создан"
				};

				return new BaseResult<UserDto>
				{
					Data = userDto
				};
			}

			return new BaseResult<UserDto>
			{
				ErrorMessage = result.Errors.ToString()
			};
		}

		public async Task<BaseResult<Token>> Login(LoginUserDto dto)
		{
			var user = await _userManager.FindByEmailAsync(dto.Email);

			if (user == null)
			{
				return new BaseResult<Token>()
				{
					ErrorMessage = "Польз не сущ-ет"
				};
			}

			var result = await _userManager.CheckPasswordAsync(user, dto.Password);

			if (!result)
			{
				return new BaseResult<Token>()
				{
					ErrorMessage = "Проверка на пароль не пройдена"
				};
			}

			var token = new Token
			{
				AccessToken = GenerateAccessToken(dto.Email),
				RefreshToken = ""
			};
				
			
			
			
			return new BaseResult<Token>()
			{
				Data = token
			};
		}
		
		private string GenerateAccessToken(string email)
		{
			IEnumerable<Claim> claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Email, email),
				new Claim(ClaimTypes.Role, "Admin"),
			};
        
			var jwtKey = _configuration.GetSection("Jwt:JwtKey").Value;

			if (jwtKey is null)
			{
				return "";
			}
        
			SecurityKey securityKey =
				new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        
			SigningCredentials _ = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        
			SecurityToken securityToken = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddMinutes(2),
				issuer: _configuration.GetSection("Jwt:Issuer").Value,
				audience:_configuration.GetSection("Jwt:Audience").Value,
            
				signingCredentials:_
			);
        
			string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
			return tokenString;
		}
	}
}
