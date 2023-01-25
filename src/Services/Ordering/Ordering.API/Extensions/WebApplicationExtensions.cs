using Microsoft.EntityFrameworkCore;
using Ordering.Infrastructure.Persistence;

namespace Ordering.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void MigrateDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OrderContext>();

            context.Database.Migrate();
        }

        public static void SeedDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OrderContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<OrderContext>>();

            OrderContextSeed.SeedAsync(context, logger).Wait();
        }
    }
}