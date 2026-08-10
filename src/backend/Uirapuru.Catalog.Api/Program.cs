using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Uirapuru.Catalog.Api.Application.Categories.Repositories;
using Uirapuru.Catalog.Api.Application.Common.Behaviors;
using Uirapuru.Catalog.Api.Application.Common.Persistence;
using Uirapuru.Catalog.Api.Application.Products.Repositories;
using Uirapuru.Catalog.Api.Application.Products.Storage;
using Uirapuru.Catalog.Api.Infrastructure.Persistence;
using Uirapuru.Catalog.Api.Infrastructure.Persistence.Categories.Repositories;
using Uirapuru.Catalog.Api.Infrastructure.Persistence.Products.Repositories;
using Uirapuru.Catalog.Api.Infrastructure.Storage.Products;

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

builder.Services.AddScoped<IUnitOfWork>(serviceProvider =>
	serviceProvider.GetRequiredService<CatalogDbContext>());
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductImageStorage, ProductImageStorage>();

builder.Services.AddMediatR(configuration =>
{
	configuration.RegisterServicesFromAssembly(
		typeof(Program).Assembly);
	configuration.AddOpenBehavior(
		typeof(TransactionBehavior<,>));
});

builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

string configuredProductImageStoragePath = builder.Configuration["ProductImages:StoragePath"];
string productImageStoragePath = Path.IsPathRooted(configuredProductImageStoragePath) ? configuredProductImageStoragePath : Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, configuredProductImageStoragePath));
Directory.CreateDirectory(productImageStoragePath);

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
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(productImageStoragePath),
	RequestPath = builder.Configuration["ProductImages:RequestPath"]
});
app.UseAuthorization();
app.MapControllers();

app.Run();
