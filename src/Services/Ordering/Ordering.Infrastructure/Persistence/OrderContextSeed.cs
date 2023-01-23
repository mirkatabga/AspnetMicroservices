using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public static class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext context, ILogger<OrderContext> logger)
        {
            if (!context.Orders!.Any())
            {
                context.Orders!.Add(new Order
                {
                    UserName = "ppeshov", 
                    FirstName = "Pesho",
                    LastName = "Peshov", 
                    EmailAddress = "some_pesho_peshov@gmail.com", 
                    AddressLine = "Sofia, bul. Bulgaria 1", 
                    Country = "Bulgaria", 
                    TotalPrice = 350
                });

                await context.SaveChangesAsync();

                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
            }
        }
    }    
}