namespace Ordering.Infrastructure
{
    public class InfrastructureConfig
    {
        public PersistenceConfig? PersistenceConfig { get; set; }

        public EmailConfig? EmailConfig { get; set; }
    }

    public class PersistenceConfig
    {
        public string? ConnectionString { get; set; }
    }

    public class EmailConfig
    {
        public string? FromAddress { get; set; }

        public string? ApiKey { get; set; }

        public string? FromName { get; set; }
    }
}