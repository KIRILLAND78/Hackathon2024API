namespace Hackathon2024API.Services.Interfaces;

public interface IBaseRepository <TEntity>
{
    IQueryable<TEntity> GetAll();

    Task<TEntity> CreateAsync(TEntity entity);

    Task<TEntity> UpdateAsync(TEntity entity);

    Task<TEntity> RemoveAsync(TEntity entity);
}