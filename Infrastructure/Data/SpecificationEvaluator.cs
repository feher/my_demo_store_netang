using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

public class SpecificationEvaluator<TEntity, TResult>
where TEntity : BaseEntity
{
    public static IQueryable<TResult> GetQuery(IQueryable<TEntity> query, ISpecification<TEntity, TResult> specification)
    {
        if (specification.Criteria is not null)
        {
            query = query.Where(specification.Criteria);
        }
        if (specification.OrderBy is not null)
        {
            query = query.OrderBy(specification.OrderBy);
        }
        if (specification.OrderByDescending is not null)
        {
            query = query.OrderByDescending(specification.OrderByDescending);
        }
        if (specification.Select is null)
        {
            return query.Cast<TResult>();
        }

        var selectQuery = query.Select(specification.Select);
        if (specification.Distinct)
        {
            selectQuery = selectQuery.Distinct();
        }
        return selectQuery;
    }
}
