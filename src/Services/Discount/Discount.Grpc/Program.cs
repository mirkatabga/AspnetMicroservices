using System.Data;
using Microsoft.Extensions.Options;
using Npgsql;
using Discount.Grpc.Data;
using Discount.Grpc.Repositories;
using Discount.Grpc.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.Configure<PostgresConfig>(builder.Configuration.GetSection(nameof(PostgresConfig)));

// Add services to the container.
builder.Services.AddTransient<IDbConnection>(sp => 
{
    string? connectionString = sp.GetRequiredService<IOptions<PostgresConfig>>().Value.ConnectionString;
    return new NpgsqlConnection(connectionString);
});

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
