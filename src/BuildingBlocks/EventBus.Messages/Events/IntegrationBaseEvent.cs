namespace EventBus.Messages.Events
{
    public abstract class IntegrationBaseEvent
    {
        protected IntegrationBaseEvent()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.UtcNow;
        }

        protected IntegrationBaseEvent(Guid id, DateTime createdDate)
        {
            Id = id;
            CreatedDate = createdDate;
        }

        public Guid Id { get; }

        public DateTime CreatedDate { get; }
    }
}