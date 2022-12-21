using MongoDB.Driver;
using Services.Catalog.Catalog.API.Entities;

namespace Services.Catalog.Catalog.API.Data
{
    public interface ICatalogContext
    {
        public IMongoCollection<Product> Products { get; }
    }
}