using System.Windows.Controls;

using Frank.ServiceBusExplorer.Gui.UserControlFactories;
using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControls;

public class ServiceBusTopicTreeViewItem : TreeViewItem
{
    private readonly IServiceBusSubscriptionTreeViewItemFactory _serviceBusSubscriptionTreeViewItemFactory;
    private readonly IServiceBusRepository _serviceBusRepository;
    private readonly ServiceBusEntity _serviceBus;
    private readonly TopicEntity _topic;

    private readonly List<SubscriptionEntity> _entities = new();
    
    public ServiceBusTopicTreeViewItem(IServiceBusRepository serviceBusRepository, ServiceBusEntity serviceBus, TopicEntity topic, IServiceBusSubscriptionTreeViewItemFactory serviceBusSubscriptionTreeViewItemFactory)
    {
        _serviceBusRepository = serviceBusRepository;
        _serviceBus = serviceBus;
        _topic = topic;
        _serviceBusSubscriptionTreeViewItemFactory = serviceBusSubscriptionTreeViewItemFactory;
        _entities = _serviceBusRepository.GetSubscriptionsAsync(_serviceBus.Name, _topic.Name, CancellationToken.None).Result.ToList();
        
        Header = _topic.Name;
        
        foreach (var entity in _entities)
        {
            var serviceBusTreeViewItem = _serviceBusSubscriptionTreeViewItemFactory.Create(_serviceBus, _topic, entity);
            Items.Add(serviceBusTreeViewItem);
        }
    }
}