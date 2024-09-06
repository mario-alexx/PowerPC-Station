using System;
using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Base API controller that provides shared functionality for handling paginated results.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{ 

  /// <summary>
  /// Creates a paginated result based on the specified repository, specification, page index, and page size.
  /// </summary>
  /// <typeparam name="T">The type of the entity, which must inherit from <see cref="BaseEntity"/>.</typeparam>
  /// <param name="repo">The generic repository used to fetch the data.</param>
  /// <param name="spec">The specification that defines the filtering, sorting, or other criteria for the query.</param>
  /// <param name="pageIndex">The index of the page to retrieve.</param>
  /// <param name="pageSize">The number of items to include in each page.</param>
  /// <returns>A paginated result containing the items for the requested page, along with pagination metadata.</returns>
  protected async Task<ActionResult> CreatePagedResult<T>(IGenericRepository<T> repo, ISpecification<T> spec,
    int pageIndex, int pageSize) where T : BaseEntity 
  {
    var items = await repo.ListAsync(spec);
    var count = await repo.CountAsync(spec);

    var pagination = new Pagination<T>(pageIndex, pageSize, count,items);

    return Ok(pagination);
  }
}
