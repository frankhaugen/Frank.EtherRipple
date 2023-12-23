using Frank.ServiceBusExplorer.Gui.UserControls;
using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControlFactories;

internal class ServiceBusTreeViewItemFactory : IServiceBusTreeViewItemFactory
{
    private readonly IServiceBusRepository _serviceBusRepository;
    private readonly IServiceBusTopicTreeViewItemFactory _serviceBusTopicTreeViewItemFactory;

    public ServiceBusTreeViewItemFactory(IServiceBusRepository serviceBusRepository, IServiceBusTopicTreeViewItemFactory serviceBusTopicTreeViewItemFactory)
    {
        _serviceBusRepository = serviceBusRepository;
        _serviceBusTopicTreeViewItemFactory = serviceBusTopicTreeViewItemFactory;
    }

    public ServiceBusTreeViewItem Create(ServiceBusEntity serviceBus) => new(_serviceBusRepository, serviceBus, _serviceBusTopicTreeViewItemFactory);
}