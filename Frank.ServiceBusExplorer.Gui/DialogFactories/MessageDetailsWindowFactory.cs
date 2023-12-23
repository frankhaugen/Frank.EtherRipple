using Azure.Messaging.ServiceBus;

using Frank.ServiceBusExplorer.Gui.DialogWindows;
using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.DialogFactories;

public class MessageDetailsWindowFactory(IServiceBusRepository serviceBusRepository) : IMessageDetailsWindowFactory
{
    public MessageDetailsWindow Create(ServiceBusReceivedMessage message, ServiceBusEntity serviceBusEntity, TopicEntity topicEntity, SubscriptionEntity subscriptionEntity)
    {
        return new(serviceBusRepository, message, serviceBusEntity, topicEntity, subscriptionEntity);
    }
}