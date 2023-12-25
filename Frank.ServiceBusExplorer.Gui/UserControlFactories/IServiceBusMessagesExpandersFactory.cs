using Frank.ServiceBusExplorer.Gui.UserControls;
using Frank.ServiceBusExplorer.Gui.UserControls.ServiceBusControls;
using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControlFactories;

public interface IServiceBusMessagesExpandersFactory
{
    ServiceBusMessagesExpanders Create(ServiceBusEntity serviceBus, TopicEntity topic, SubscriptionEntity subscription);
}