using Basket.API.GrpcServices;
using Basket.API.Repositories;
using MassTransit;
using Discount.Grpc.Protos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.Configuration = builder.Configuration.GetValue<string>(
       "DistributedCacheConfig:ConnectionString");
});

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opt =>
    opt.Address = new Uri(builder.Configuration.GetValue<string>("DiscountGrpcConfig:Url")));

builder.Services.AddMassTransit(config =>
    config.UsingRabbitMq((_, cfg) => cfg.Host(builder.Configuration["EventBusSettings:HostAddress"])));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<DiscountGrpcService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket API v1"));
}

app.UseAuthorization();

app.MapControllers();

app.Run();
