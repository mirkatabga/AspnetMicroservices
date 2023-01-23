using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Common;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContext : DbContext
    {
        private const string ORDERING_MICROSERVICE = "ordering microservice";

        public OrderContext(DbContextOptions options) 
            : base(options)
        {           
        }

        public DbSet<Order>? Orders { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<EntityBase>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = ORDERING_MICROSERVICE;
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                }
                else if(entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModifiedBy = ORDERING_MICROSERVICE;
                    entry.Entity.LastModifiedDate = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}