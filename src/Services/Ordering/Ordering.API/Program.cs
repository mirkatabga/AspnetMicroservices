using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Ordering.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddEventBus(GetConfig<EventBusConfig>());
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(typeof(Program));
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(GetConfig<InfrastructureConfig>());
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

T GetConfig<T>() where T : class
{
    return builder!.Configuration
            .GetSection(typeof(T).Name)
            .Get<T>();
}
