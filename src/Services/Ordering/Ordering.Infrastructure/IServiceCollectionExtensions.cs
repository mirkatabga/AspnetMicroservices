using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Infrastructure.Mail;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, InfrastructureConfig configuration)
        {
            services.AddDbContext<OrderContext>(options =>
                options.UseSqlServer(configuration.PersistenceConfig?.ConnectionString));

            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.Configure<EmailConfig>(c => c = configuration.EmailConfig!);
            services.AddTransient<IEmailService, EmailService>();

            return services;
        }
    }
}
