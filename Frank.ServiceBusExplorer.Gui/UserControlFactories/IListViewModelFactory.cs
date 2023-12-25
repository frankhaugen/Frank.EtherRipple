using Azure.Messaging.ServiceBus;

using Frank.ServiceBusExplorer.Gui.UserControls;
using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControlFactories;

public interface IListViewModelFactory
{
    ListViewModel Create(IEnumerable<ServiceBusReceivedMessage> items, ServiceBusEntity serviceBusEntity, TopicEntity topicEntity, SubscriptionEntity subscriptionEntity);
}