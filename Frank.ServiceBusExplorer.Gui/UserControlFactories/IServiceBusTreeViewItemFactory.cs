using Frank.ServiceBusExplorer.Gui.UserControls;
using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControlFactories;

public interface IServiceBusTreeViewItemFactory
{
    ServiceBusTreeViewItem Create(ServiceBusEntity serviceBus);
}