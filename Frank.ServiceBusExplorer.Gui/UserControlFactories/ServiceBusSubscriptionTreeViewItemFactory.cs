using Frank.ServiceBusExplorer.Gui.UserControls;
using Frank.ServiceBusExplorer.Gui.UserControls.ServiceBusControls;
using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControlFactories;

public class ServiceBusSubscriptionTreeViewItemFactory : IServiceBusSubscriptionTreeViewItemFactory
{
    private readonly IServiceBusMessagesExpandersFactory _serviceBusMessagesExpandersFactory;

    public ServiceBusSubscriptionTreeViewItemFactory(IServiceBusMessagesExpandersFactory serviceBusMessagesExpandersFactory)
    {
        _serviceBusMessagesExpandersFactory = serviceBusMessagesExpandersFactory;
    }

    public ServiceBusSubscriptionTreeViewItem Create(ServiceBusEntity serviceBus, TopicEntity topic, SubscriptionEntity entity)
    {
        return new ServiceBusSubscriptionTreeViewItem(serviceBus, topic, entity, _serviceBusMessagesExpandersFactory);
    }
}