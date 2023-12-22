namespace Frank.ServiceBusExplorer.Infrastructure.Entities;

public class TopicEntity : ServiceBusEntity
{
    public int SubscriptionCount { get; set; }
}