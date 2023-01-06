using System.Data;
using Npgsql;
using Microsoft.Extensions.Options;
using Discount.API.Data;
using Discount.API.Repositories;
using Discount.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<PostgresConfig>(builder.Configuration.GetSection(nameof(PostgresConfig)));

// Add services to the container.
builder.Services.AddTransient<IDbConnection>(sp => 
{
    string? connectionString = sp.GetRequiredService<IOptions<PostgresConfig>>().Value.ConnectionString;
    return new NpgsqlConnection(connectionString);
});

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MigrateDatabase<Program>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
