using System.Windows.Controls;

using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControls;

public class ServiceBusTopicTreeViewItem : TreeViewItem
{
    private readonly IServiceBusRepository _serviceBusRepository;
    private readonly ServiceBusEntity _serviceBus;
    private readonly TopicEntity _topic;

    private readonly List<SubscriptionEntity> _entities = new();
    
    public ServiceBusTopicTreeViewItem(IServiceBusRepository serviceBusRepository, ServiceBusEntity serviceBus, TopicEntity topic)
    {
        _serviceBusRepository = serviceBusRepository;
        _serviceBus = serviceBus;
        _topic = topic;
        _entities = _serviceBusRepository.GetSubscriptionsAsync(_serviceBus.Name, _topic.Name, CancellationToken.None).Result.ToList();
        
        Header = _topic.Name;
        
        foreach (var entity in _entities)
        {
            var serviceBusTreeViewItem = new ServiceBusSubscriptionTreeViewItem(_serviceBusRepository, _serviceBus, _topic, entity);
            Items.Add(serviceBusTreeViewItem);
        }
    }
}