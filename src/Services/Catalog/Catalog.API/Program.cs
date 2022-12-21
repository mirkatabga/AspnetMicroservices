using MongoDB.Driver;
using Services.Catalog.Catalog.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<MongoConfig>(builder.Configuration);
builder.Services.AddSingleton<IMongoClient>(sp => 
{
    var config = sp.GetRequiredService<MongoConfig>();
    return new MongoClient(config.ConnectionString);
});

builder.Services.AddScoped<ICatalogContext, MongoCatalogContext>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
