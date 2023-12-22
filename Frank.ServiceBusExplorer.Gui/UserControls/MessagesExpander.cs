using System.Windows.Controls;

using Azure.Messaging.ServiceBus;

using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControls;

public class MessagesExpander : Expander
{
    private readonly IServiceBusRepository _serviceBusRepository;
    private readonly ServiceBusEntity _serviceBus;
    private readonly TopicEntity _topic;
    private readonly SubscriptionEntity _subscription;
        
    public MessagesExpander(IServiceBusRepository serviceBusRepository, ServiceBusEntity serviceBus, TopicEntity topic, SubscriptionEntity subscription, string header)
    {
        _serviceBusRepository = serviceBusRepository;
        _serviceBus = serviceBus;
        _topic = topic;
        _subscription = subscription;

        Header = header;
        
        var messages = _serviceBusRepository.GetMessagesAsync(_serviceBus.Name, _topic.Name, _subscription.Name, CancellationToken.None).Result.ToList();
        
        Content = new ListViewModel<ServiceBusReceivedMessage>(messages);
    }
}