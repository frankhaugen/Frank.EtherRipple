using Frank.ServiceBusExplorer.Gui.UserControls;
using Frank.ServiceBusExplorer.Gui.UserControls.ServiceBusControls;
using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControlFactories;

public interface IServiceBusSubscriptionTreeViewItemFactory
{
    ServiceBusSubscriptionTreeViewItem Create(ServiceBusEntity serviceBus, TopicEntity topic, SubscriptionEntity entity);
}