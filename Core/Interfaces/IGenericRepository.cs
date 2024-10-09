using Core.Entities;

namespace Core.Interfaces;

public interface IGenericRepository<TEntity>
where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(int id);
    Task<IReadOnlyList<TEntity>> GetAllAsync();

    Task<TResult?> GetBySpec<TResult>(ISpecification<TEntity, TResult> specification);
    Task<IReadOnlyList<TResult>> GetAllBySpecAsync<TResult>(ISpecification<TEntity, TResult> specification);

    Task<int> Count<TResult>(ISpecification<TEntity, TResult> specification);

    void Create(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    bool DoesExist(int id);
    Task<bool> SaveChangesAsync();
}
