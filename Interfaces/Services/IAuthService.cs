using Hackathon2024API.DTO.User;
using Hackathon2024API.Result;
namespace Hackathon2024API.Interfaces.Services
{
	public interface IAuthService
	{
		Task<BaseResult<UserDto>> Register(RegisterUserDto dto);

		//Task<BaseResult<TokenDto>> Login(LoginUserDto dto);

		/// <summary>
		/// Метод, предназначенный для проверки наличия пользователя в системе
		/// </summary>
		/// <param name="email">Почта, по которой проверяем, существует ли у нас такой пользователь</param>
		/// <returns>Возвращает userId, если таков существует, в противном случае null</returns>
		//Task<BaseResult<long>> GetCurrentUser(string email);
	}
}
