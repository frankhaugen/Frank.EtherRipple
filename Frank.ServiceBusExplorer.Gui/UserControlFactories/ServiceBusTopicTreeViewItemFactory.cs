using Frank.ServiceBusExplorer.Gui.UserControls;
using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControlFactories;

public class ServiceBusTopicTreeViewItemFactory : IServiceBusTopicTreeViewItemFactory
{
    private readonly IServiceBusSubscriptionTreeViewItemFactory _serviceBusSubscriptionTreeViewItemFactory;
    private readonly IServiceBusRepository _serviceBusRepository;

    public ServiceBusTopicTreeViewItemFactory(IServiceBusSubscriptionTreeViewItemFactory serviceBusSubscriptionTreeViewItemFactory, IServiceBusRepository serviceBusRepository)
    {
        _serviceBusSubscriptionTreeViewItemFactory = serviceBusSubscriptionTreeViewItemFactory;
        _serviceBusRepository = serviceBusRepository;
    }

    public ServiceBusTopicTreeViewItem Create(ServiceBusEntity serviceBus, TopicEntity topic) => new(_serviceBusRepository, serviceBus, topic, _serviceBusSubscriptionTreeViewItemFactory);
}