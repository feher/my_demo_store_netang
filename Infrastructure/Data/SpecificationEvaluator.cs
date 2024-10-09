using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

public enum SpecificationPart
{
    Criteria,
    Ordering,
    Paging
}

public class SpecificationEvaluator<TEntity, TResult>
where TEntity : BaseEntity
{
    public static IQueryable<TResult> Evaluate(
        IQueryable<TEntity> query,
        ISpecification<TEntity, TResult> specification,
        IReadOnlyList<SpecificationPart>? partsToEvaluate = null)
    {
        if (specification.Criteria is not null && (partsToEvaluate is null || partsToEvaluate.Contains(SpecificationPart.Criteria)))
        {
            query = query.Where(specification.Criteria);
        }
        if (specification.OrderBy is not null && (partsToEvaluate is null || partsToEvaluate.Contains(SpecificationPart.Ordering)))
        {
            query = query.OrderBy(specification.OrderBy);
        }
        if (specification.OrderByDescending is not null && (partsToEvaluate is null || partsToEvaluate.Contains(SpecificationPart.Ordering)))
        {
            query = query.OrderByDescending(specification.OrderByDescending);
        }

        // The selection is always applied because we need a query that produces TResult.
        var resultQuery = ApplySelection(query, specification);

        if (specification.IsPagingEnabled && (partsToEvaluate is null || partsToEvaluate.Contains(SpecificationPart.Paging)))
        {
            resultQuery = resultQuery.Skip(specification.Skip).Take(specification.Take);
        }

        return resultQuery;
    }

    private static IQueryable<TResult> ApplySelection(IQueryable<TEntity> query, ISpecification<TEntity, TResult> specification)
    {
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
