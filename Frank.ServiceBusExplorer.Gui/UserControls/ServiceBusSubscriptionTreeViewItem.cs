using System.Windows.Controls;

using Frank.ServiceBusExplorer.Gui.UserControlFactories;
using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControls;

public class ServiceBusSubscriptionTreeViewItem : TreeViewItem
{
    private readonly IServiceBusMessagesExpandersFactory _serviceBusMessagesExpandersFactory;
    private readonly ServiceBusEntity _serviceBus;
    private readonly TopicEntity _topic;
    private readonly SubscriptionEntity _entity;
        
    public ServiceBusSubscriptionTreeViewItem(ServiceBusEntity serviceBus, TopicEntity topic, SubscriptionEntity entity, IServiceBusMessagesExpandersFactory serviceBusMessagesExpandersFactory)
    {
        _serviceBus = serviceBus;
        _topic = topic;
        _entity = entity;
        _serviceBusMessagesExpandersFactory = serviceBusMessagesExpandersFactory;

        Header = $"{_entity.Name} (Total Messages: {_entity.TotalMessageCount}, Active Messages: {_entity.ActiveMessageCount}, Dead Letter Messages: {_entity.DeadLetterMessageCount})";
        
        var messagesExpander = _serviceBusMessagesExpandersFactory.Create(_serviceBus, _topic, _entity);
        
        Items.Add(messagesExpander);
    }
}