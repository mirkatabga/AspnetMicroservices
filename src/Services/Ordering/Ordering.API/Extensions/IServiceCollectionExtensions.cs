using EventBus.Messages.Common;
using MassTransit;
using Ordering.API.EventBusConsumer;
using Ordering.Ordering.API.EventBusConsumer;

namespace Ordering.Ordering.API.Extensions
{
    internal static class IServiceCollectionExtensions
    {
        public static void AddEventBus(this IServiceCollection services, EventBusConfig eventBusConfig)
        {
            services.AddMassTransit(config =>
            {
                config.AddConsumer<BasketCheckoutConsumer>();

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(eventBusConfig.HostAddress);

                    cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue,
                        c => c.ConfigureConsumer<BasketCheckoutConsumer>(ctx));
                });
            });
        }
    }
}