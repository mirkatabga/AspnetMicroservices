using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Services.Catalog.Catalog.API.Data;
using Services.Catalog.Catalog.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<MongoConfig>(builder.Configuration.GetSection("MongoConfig"));
builder.Services.AddSingleton<IMongoClient>(sp => 
{
    var config = sp.GetRequiredService<IOptions<MongoConfig>>();
    return new MongoClient(config.Value.ConnectionString);
});

builder.Services.AddScoped<ICatalogContext, MongoCatalogContext>();
builder.Services.AddScoped<IDataSeeder, MongoDataSeeder>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    
    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    seeder.Seed();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
