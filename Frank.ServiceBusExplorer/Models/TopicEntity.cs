namespace Frank.ServiceBusExplorer.Models;

public class TopicEntity : ServiceBusEntity
{
    public int SubscriptionCount { get; set; }
    public long SizeInBytes { get; set; }
    
}