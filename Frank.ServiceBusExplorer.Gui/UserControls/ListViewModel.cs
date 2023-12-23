using System.Windows.Controls;

using Azure.Messaging.ServiceBus;

using Frank.ServiceBusExplorer.Gui.DialogFactories;
using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControls;

public class ListViewModel<T> : ListView
{
    public ListViewModel(IEnumerable<T> items, IMessageDetailsWindowFactory messageDetailsWindowFactory, ServiceBusEntity serviceBusEntity, TopicEntity topicEntity, SubscriptionEntity subscriptionEntity)
    {
        DataContext = this;
        ItemsSource = items;
        
        this.MouseDoubleClick += (sender, args) =>
        {
            if (SelectedItem is not ServiceBusReceivedMessage message) return;
            var messageDetailsWindow = messageDetailsWindowFactory.Create(message, serviceBusEntity, topicEntity, subscriptionEntity);
            messageDetailsWindow.Show();
        };
    }
}