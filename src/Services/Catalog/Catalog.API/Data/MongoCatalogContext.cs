using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Services.Catalog.Catalog.API.Entities;

namespace Services.Catalog.Catalog.API.Data
{
    public class MongoCatalogContext : ICatalogContext
    {
        public MongoCatalogContext(
            IOptions<MongoConfig> config,
            IMongoClient client)
        {
            var db = client.GetDatabase(config.Value.DatabaseName);
            Products = db.GetCollection<Product>(config.Value.ProductsCollectionName);
        }

        public IMongoCollection<Product> Products { get; private set; }
    }
}