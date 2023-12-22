namespace Frank.ServiceBusExplorer.Infrastructure;

public class SubscriptionEntity : ServiceBusEntity
{
    public long TotalMessageCount { get; set; }
    public long ActiveMessageCount { get; set; }
    public long DeadLetterMessageCount { get; set; }
}