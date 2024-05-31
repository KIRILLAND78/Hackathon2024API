using Hackathon2024API.DTO.User;
using Hackathon2024API.Result;

namespace Hackathon2024API.Services.Interfaces;

public interface IAdminService
{
    Task CreateChanges(LimitSettings limitSettings);

}