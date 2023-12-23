using Frank.ServiceBusExplorer.Gui.UserControls;

namespace Frank.ServiceBusExplorer.Gui.UserControlFactories;

internal class ServiceBusTreeViewFactory : IServiceBusTreeViewFactory
{
    private readonly IServiceBusRepository _serviceBusRepository;
    private readonly IServiceBusTopicTreeViewItemFactory _serviceBusTopicTreeViewItemFactory;

    public ServiceBusTreeViewFactory(IServiceBusRepository serviceBusRepository, IServiceBusTopicTreeViewItemFactory serviceBusTopicTreeViewItemFactory)
    {
        _serviceBusRepository = serviceBusRepository;
        _serviceBusTopicTreeViewItemFactory = serviceBusTopicTreeViewItemFactory;
    }

    public ServiceBusTreeView Create() => new(_serviceBusRepository, _serviceBusTopicTreeViewItemFactory);
}