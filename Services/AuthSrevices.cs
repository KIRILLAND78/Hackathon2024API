using Hackathon2024API.DTO.User;
using Hackathon2024API.Interfaces.Services;
using Hackathon2024API.Models;
using Hackathon2024API.Result;
using Microsoft.AspNetCore.Identity;

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
				Mail = dto.Email,
				UserName = dto.Login
			};

			var existingUser = await _userManager.FindByEmailAsync(identityUser.Mail);
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
	}
}
