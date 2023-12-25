using Azure.Messaging.ServiceBus;

using Frank.ServiceBusExplorer.Gui.DialogFactories;
using Frank.ServiceBusExplorer.Gui.UserControls;
using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControlFactories;

public class ListViewModelFactory : IListViewModelFactory
{
    private readonly IMessageDetailsWindowFactory _messageDetailsWindowFactory;

    public ListViewModelFactory(IMessageDetailsWindowFactory messageDetailsWindowFactory)
    {
        _messageDetailsWindowFactory = messageDetailsWindowFactory;
    }

    public ListViewModel Create(IEnumerable<ServiceBusReceivedMessage> items, ServiceBusEntity serviceBusEntity, TopicEntity topicEntity, SubscriptionEntity subscriptionEntity) 
        => new(items, _messageDetailsWindowFactory, serviceBusEntity, topicEntity, subscriptionEntity);
}