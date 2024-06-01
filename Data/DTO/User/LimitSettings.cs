namespace Hackathon2024API.DTO.User;

public record LimitSettings(
    long UserId,
    
    short MaxFilesCount,
    bool CanUpload,
    bool CanRead,
    byte ImageQuality,
    bool CanChange,
    bool CanDelete,
    int MaxFileSizeMb
    );