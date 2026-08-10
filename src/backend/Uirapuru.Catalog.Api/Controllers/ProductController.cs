using MediatR;
using Microsoft.AspNetCore.Mvc;
using Uirapuru.Catalog.Api.Application.Products;

namespace Uirapuru.Catalog.Api.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductController : ControllerBase
{
	private readonly ISender _sender;

	public ProductController(ISender sender)
	{
		_sender = sender;
	}

	[HttpGet("get/{idProduct}")]
	public async Task<IActionResult> Get(int idProduct, CancellationToken cancellationToken)
	{
		GetProductQuery.Input input = new GetProductQuery.Input
		{
			IdProduct = idProduct
		};

		GetProductQuery.Result result = await _sender.Send(input, cancellationToken);

		return Ok(result);
	}

	[HttpGet("list")]
	public async Task<IActionResult> List(CancellationToken cancellationToken)
	{
		GetProductListQuery.Result result = await _sender.Send(new GetProductListQuery.Input(), cancellationToken);

		return Ok(result);
	}

	[HttpPost("create")]
	public async Task<IActionResult> Create([FromBody] CreateProductCommand.Input command, CancellationToken cancellationToken)
	{
		CreateProductCommand.Result result = await _sender.Send(command, cancellationToken);

		return Ok(result);
	}

	[HttpPost("update")]
	public async Task<IActionResult> Update([FromBody] UpdateProductCommand.Input command, CancellationToken cancellationToken)
	{
		UpdateProductCommand.Result result = await _sender.Send(command, cancellationToken);

		return Ok(result);
	}

	[HttpPost("disable")]
	public async Task<IActionResult> Disable([FromBody] DisableProductCommand.Input command, CancellationToken cancellationToken)
	{
		DisableProductCommand.Result result = await _sender.Send(command, cancellationToken);

		return Ok(result);
	}

	[HttpPost("enable")]
	public async Task<IActionResult> Enable([FromBody] EnableProductCommand.Input command, CancellationToken cancellationToken)
	{
		EnableProductCommand.Result result = await _sender.Send(command, cancellationToken);

		return Ok(result);
	}

	[HttpPost("update-image")]
	public async Task<IActionResult> UpdateImage([FromForm] UpdateProductImageCommand.Input command, CancellationToken cancellationToken)
	{
		UpdateProductImageCommand.Result result = await _sender.Send(command, cancellationToken);

		return Ok(result);
	}
}
