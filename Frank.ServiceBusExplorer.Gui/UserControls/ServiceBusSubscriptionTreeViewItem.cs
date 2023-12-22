using System.Windows.Controls;

using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControls;

public class ServiceBusSubscriptionTreeViewItem : TreeViewItem
{
    private readonly IServiceBusRepository _serviceBusRepository;
    private readonly ServiceBusEntity _serviceBus;
    private readonly TopicEntity _topic;
    private readonly SubscriptionEntity _entity;
        
    public ServiceBusSubscriptionTreeViewItem(IServiceBusRepository serviceBusRepository, ServiceBusEntity serviceBus, TopicEntity topic, SubscriptionEntity entity)
    {
        _serviceBusRepository = serviceBusRepository;
        _serviceBus = serviceBus;
        _topic = topic;
        _entity = entity;

        Header = $"{_entity.Name} (Total Messages: {_entity.TotalMessageCount}, Active Messages: {_entity.ActiveMessageCount}, Dead Letter Messages: {_entity.DeadLetterMessageCount})";
    }
}