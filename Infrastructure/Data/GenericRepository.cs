using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class GenericRepository<T>(StoreContext context) : IGenericRepository<T>
where T : BaseEntity
{
    public void Create(T entity)
    {
        context.Set<T>().Add(entity);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await context.Set<T>().FindAsync(id);
    }

    public async Task<TResult?> GetBySpec<TResult>(ISpecification<T, TResult> specification)
    {
        var query = ApplySpecification(specification);
        return await query.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<TResult>> GetAllBySpecAsync<TResult>(ISpecification<T, TResult> specification)
    {
        var query = ApplySpecification(specification);
        return await query.ToListAsync();
    }

    public async Task<int> Count<TResult>(ISpecification<T, TResult> specification)
    {
        // For item count, we need to apply only the category (i.e. the filtering).
        var query = ApplyCategorySpecification(specification);
        return await query.CountAsync();
    }

    public void Update(T entity)
    {
        context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        context.Set<T>().Remove(entity);
    }

    public bool DoesExist(int id)
    {
        return context.Set<T>().Any(entity => entity.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await context.SaveChangesAsync()) > 0;
    }

    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
    {
        return SpecificationEvaluator<T, TResult>.Evaluate(context.Set<T>().AsQueryable(), specification);
    }

    private IQueryable<TResult> ApplyCategorySpecification<TResult>(ISpecification<T, TResult> specification)
    {
        return SpecificationEvaluator<T, TResult>.Evaluate(
            context.Set<T>().AsQueryable(),
            specification,
            [ SpecificationPart.Criteria ]);
    }
}
