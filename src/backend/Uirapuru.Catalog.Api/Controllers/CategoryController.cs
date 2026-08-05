using MediatR;
using Microsoft.AspNetCore.Mvc;
using Uirapuru.Catalog.Api.Application.Categories;

namespace Uirapuru.Catalog.Api.Controllers;

[ApiController]
[Route("api/category")]
public sealed class CategoryController : ControllerBase
{
	private readonly ISender _sender;

	public CategoryController(ISender sender)
	{
		_sender = sender;
	}

	[HttpPost("create")]
	public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command, CancellationToken cancellationToken)
	{
		await _sender.Send(command, cancellationToken);

		return Ok();
	}
}
