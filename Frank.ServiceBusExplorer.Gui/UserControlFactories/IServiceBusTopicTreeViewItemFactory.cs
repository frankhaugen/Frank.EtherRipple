using Frank.ServiceBusExplorer.Gui.UserControls;
using Frank.ServiceBusExplorer.Gui.UserControls.ServiceBusControls;
using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControlFactories;

public interface IServiceBusTopicTreeViewItemFactory
{
    ServiceBusTopicTreeViewItem Create(ServiceBusEntity serviceBus, TopicEntity topic);
}