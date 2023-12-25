using Frank.ServiceBusExplorer.Gui.UserControls;

namespace Frank.ServiceBusExplorer.Gui.UserControlFactories;

internal class ServiceBusTreeViewFactory(IServiceBusRepository serviceBusRepository, IServiceBusTreeViewItemFactory serviceBusTreeViewItemFactory) : IServiceBusTreeViewFactory
{
    public ServiceBusTreeView Create() => new(serviceBusRepository, serviceBusTreeViewItemFactory);
}