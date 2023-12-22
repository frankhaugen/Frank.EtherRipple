namespace Frank.ServiceBusExplorer.Cli;

public class PageNames : Dictionary<Guid, string>
{
    public PageNames()
    {
        Add(PageIds.ServiceBusPageId, "Service Bus");
        Add(PageIds.TopicsPageId, "Topics");
        Add(PageIds.SubscriptionsPageId, "Subscriptions");
        Add(PageIds.MessagesPageId, "Messages");
        Add(PageIds.MessageBodyPageId, "Message Body");
    }
};