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

	[HttpGet("get/{idCategory}")]
	public async Task<IActionResult> Get(byte idCategory, CancellationToken cancellationToken)
	{
		GetCategoryQuery.Input input = new GetCategoryQuery.Input
		{
			IdCategory = idCategory
		};

		GetCategoryQuery.Result result = await _sender.Send(input, cancellationToken);

		return Ok(result);
	}

	[HttpGet("list")]
	public async Task<IActionResult> List(CancellationToken cancellationToken)
	{
		GetCategoryListQuery.Result result = await _sender.Send(new GetCategoryListQuery.Input(), cancellationToken);

		return Ok(result);
	}

	[HttpPost("create")]
	public async Task<IActionResult> Create([FromBody] CreateCategoryCommand.Input command, CancellationToken cancellationToken)
	{
		CreateCategoryCommand.Result result = await _sender.Send(command, cancellationToken);

		return Ok(result);
	}

	[HttpPost("update")]
	public async Task<IActionResult> Update([FromBody] UpdateCategoryCommand.Input command, CancellationToken cancellationToken)
	{
		UpdateCategoryCommand.Result result = await _sender.Send(command, cancellationToken);

		return Ok(result);
	}

	[HttpPost("disable")]
	public async Task<IActionResult> Disable([FromBody] DisableCategoryCommand.Input command, CancellationToken cancellationToken)
	{
		DisableCategoryCommand.Result result = await _sender.Send(command, cancellationToken);

		return Ok(result);
	}

	[HttpPost("enable")]
	public async Task<IActionResult> Enable([FromBody] EnableCategoryCommand.Input command, CancellationToken cancellationToken)
	{
		EnableCategoryCommand.Result result = await _sender.Send(command, cancellationToken);

		return Ok(result);
	}
}
