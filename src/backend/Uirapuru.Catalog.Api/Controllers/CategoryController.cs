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

	[HttpGet("list")]
	public async Task<IActionResult> List(
	[FromBody] CreateCategoryCommand.Input command, CancellationToken cancellationToken)
	{
		CreateCategoryCommand.Result result = await _sender.Send(command, cancellationToken);

		return Ok(result);
	}

	[HttpPost("create")]
	public async Task<IActionResult> Create(
		[FromBody] CreateCategoryCommand.Input command, CancellationToken cancellationToken)
	{
		CreateCategoryCommand.Result result = await _sender.Send(command, cancellationToken);

		return Ok(result);
	}
}
