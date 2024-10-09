using Api.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")] // Will become "api/products".
public class BaseApiController : ControllerBase
{
    protected async Task<ActionResult<Pagination<TResult>>> CreatePagedResultAsync<TEntity, TResult>(
        IGenericRepository<TEntity> repository,
        ISpecification<TEntity, TResult> spec,
        int pageIndex,
        int pageSize)
    where TEntity : BaseEntity
    {
        var count = await repository.Count(spec);
        var products = await repository.GetAllBySpecAsync(spec);
        var pagination = new Pagination<TResult>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            Count = count,
            Data = products
        };
        return new ActionResult<Pagination<TResult>>(pagination);
    }
}
