using Azure.Messaging.ServiceBus;

using Frank.ServiceBusExplorer.Gui.DialogWindows;
using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.DialogFactories;

public interface IMessageDetailsWindowFactory
{
    MessageDetailsWindow Create(ServiceBusReceivedMessage message, ServiceBusEntity serviceBusEntity, TopicEntity topicEntity, SubscriptionEntity subscriptionEntity);
}