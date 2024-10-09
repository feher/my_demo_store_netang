using System.Linq.Expressions;
using Core.Interfaces;

namespace Core.Specifications;

public class BaseSpecification<TEntity, TResult>(Expression<Func<TEntity, bool>>? criteria) : ISpecification<TEntity, TResult>
{
    protected BaseSpecification() : this(null)
    {
        // Empty.
    }

    public Expression<Func<TEntity, bool>>? Criteria => criteria;
    public Expression<Func<TEntity, object>>? OrderBy { get; protected set; }
    public Expression<Func<TEntity, object>>? OrderByDescending { get; protected set; }

    public bool IsPagingEnabled { get; protected set; } = false;
    public int Skip { get; protected set; }
    public int Take { get; protected set; }

    public Expression<Func<TEntity, TResult>>? Select { get; protected set; }
    public bool Distinct { get; protected set; } = false;
}
