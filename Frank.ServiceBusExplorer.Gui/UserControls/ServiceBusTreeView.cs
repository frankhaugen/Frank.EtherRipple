using System.Windows.Controls;

using Frank.ServiceBusExplorer;
using Frank.ServiceBusExplorer.Models;

namespace PureWpfApp.UserControls;

public class ServiceBusTreeView : TreeView
{
    private readonly IServiceBusRepository _serviceBusRepository;

    private readonly List<ServiceBusEntity> _entities = new();
    
    public ServiceBusTreeView(IServiceBusRepository serviceBusRepository)
    {
        _serviceBusRepository = serviceBusRepository;
        _entities = _serviceBusRepository.GetServiceBuses().ToList();
        
        foreach (var entity in _entities)
        {
            var serviceBusTreeViewItem = new ServiceBusTreeViewItem(_serviceBusRepository, entity.Name);
            Items.Add(serviceBusTreeViewItem);
        }
    }
}

public class ServiceBusTreeViewItem : TreeViewItem
{
    private readonly IServiceBusRepository _serviceBusRepository;
    private readonly string _serviceBusName;

    private readonly List<TopicEntity> _entities = new();
    
    public ServiceBusTreeViewItem(IServiceBusRepository serviceBusRepository, string serviceBusName)
    {
        _serviceBusRepository = serviceBusRepository;
        _serviceBusName = serviceBusName;
        _entities = _serviceBusRepository.GetTopicsAsync(serviceBusName, CancellationToken.None).GetAwaiter().GetResult().ToList();
        
        foreach (var entity in _entities)
        {
            var serviceBusTreeViewItem = new ServiceBusTopicTreeViewItem(_serviceBusRepository, _serviceBusName, entity.Name);
            Items.Add(serviceBusTreeViewItem);
        }
    }
}
public class ServiceBusTopicTreeViewItem : TreeViewItem
{
    private readonly IServiceBusRepository _serviceBusRepository;
    private readonly string _serviceBusName;
    private readonly string _topicName;

    private readonly List<SubscriptionEntity> _entities = new();
    
    public ServiceBusTopicTreeViewItem(IServiceBusRepository serviceBusRepository, string serviceBusName, string topicName)
    {
        _serviceBusRepository = serviceBusRepository;
        _serviceBusName = serviceBusName;
        _topicName = topicName;
        _entities = _serviceBusRepository.GetSubscriptionsAsync(serviceBusName, topicName, CancellationToken.None).Result.ToList();
        
        foreach (var entity in _entities)
        {
            var serviceBusTreeViewItem = new ServiceBusSubscriptionTreeViewItem(_serviceBusRepository, _serviceBusName, _topicName, entity.Name);
            Items.Add(serviceBusTreeViewItem);
        }
    }
}

public class ServiceBusSubscriptionTreeViewItem : TreeViewItem
{
    public ServiceBusSubscriptionTreeViewItem(IServiceBusRepository serviceBusRepository, string serviceBusName, string topicName, string entityName)
    {
        
    }
}