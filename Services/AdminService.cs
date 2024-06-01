using Hackathon2024API.DTO.User;
using Hackathon2024API.Data.Models;
using Hackathon2024API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hackathon2024API.Services;

public class AdminService : IAdminService
{
    private readonly IBaseRepository<User> _baseRepository;

    public AdminService(IBaseRepository<User> baseRepository)
    {
        _baseRepository = baseRepository;
    }

    public async Task CreateChanges(LimitSettings limitSettings)
    {
        try
        {
            var user = await _baseRepository.GetAll().FirstOrDefaultAsync(x => x.Id == limitSettings.UserId);

            if (user == null)
            {
                return;
            }
            
            user.MaxFilesCount = limitSettings.MaxFilesCount;
            //user.CanChange = limitSettings.CanChange;
            user.CanUpload = limitSettings.CanUpload;
            user.CanRead = limitSettings.CanRead;
            user.ImageQuality = limitSettings.ImageQuality;
            user.CanUpload = limitSettings.CanUpload;
            user.CanDelete = limitSettings.CanDelete;
            user.MaxFileSizeMb = limitSettings.MaxFileSizeMb;

            await _baseRepository.UpdateAsync(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}