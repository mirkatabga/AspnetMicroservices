using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(GetInfrastructureConfig());
}

var app = builder.Build();
{
    app.MigrateDatabase();
    app.SeedDatabase();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}

InfrastructureConfig GetInfrastructureConfig()
{
    return builder.Configuration
            .GetSection(nameof(InfrastructureConfig))
            .Get<InfrastructureConfig>();
}
