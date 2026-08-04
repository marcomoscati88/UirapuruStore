using Microsoft.EntityFrameworkCore;
using Uirapuru.Catalog.Api.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(
	$"appsettings.{builder.Environment.EnvironmentName}.Catalog.json",
	optional: true,
	reloadOnChange: true);

// Add services to the container.

builder.Services.AddDbContext<CatalogDbContext>(options =>
	options.UseNpgsql(
		builder.Configuration.GetConnectionString("CatalogDatabase"),
		npgsqlOptions =>
		{
			npgsqlOptions.MigrationsHistoryTable(
				"DbMigrationsHistory",
				"catalog");
		}));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();

	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint(
			"/openapi/v1.json",
			"Uirapuru Catalog API v1");
	});
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
