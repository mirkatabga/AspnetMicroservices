namespace Services.Catalog.Catalog.API.Data
{
    public class MongoConfig
    {
        public string? ConnectionString { get; set; }

        public string? DatabaseName { get; set; }

        public string? ProductsCollectionName { get; set; }
    }
}