using System.Linq.Expressions;

namespace Core.Interfaces;

public interface ISpecification<TEntity, TResult>
{
    Expression<Func<TEntity, bool>>? Criteria { get; }
    Expression<Func<TEntity, object>>? OrderBy { get; }
    Expression<Func<TEntity, object>>? OrderByDescending { get; }

    bool IsPagingEnabled { get; }
    int Skip { get; }
    int Take { get; }

    Expression<Func<TEntity, TResult>>? Select { get; }
    bool Distinct { get; }
}
