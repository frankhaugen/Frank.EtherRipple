using System.Windows.Controls;

using Frank.ServiceBusExplorer.Gui.UserControlFactories;
using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControls;

public class ServiceBusSubscriptionTreeViewItem : TreeViewItem
{
    public ServiceBusSubscriptionTreeViewItem(ServiceBusEntity serviceBus, TopicEntity topic, SubscriptionEntity entity, IServiceBusMessagesExpandersFactory serviceBusMessagesExpandersFactory)
    {
        Header = $"{entity.Name} (Total Messages: {entity.TotalMessageCount}, Active Messages: {entity.ActiveMessageCount}, Dead Letter Messages: {entity.DeadLetterMessageCount})";
        var messagesExpander = serviceBusMessagesExpandersFactory.Create(serviceBus, topic, entity);
        Items.Add(messagesExpander);
    }
}