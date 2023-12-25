using System.Windows.Controls;

using Azure.Messaging.ServiceBus;

using Frank.ServiceBusExplorer.Gui.DialogFactories;
using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControls;

public class ListViewModel : ListView
{
    public ListViewModel(IEnumerable<ServiceBusReceivedMessage> messages, IMessageDetailsWindowFactory messageDetailsWindowFactory, ServiceBusEntity serviceBusEntity, TopicEntity topicEntity, SubscriptionEntity subscriptionEntity)
    {
        DataContext = this;
        ItemsSource = messages;
        
        this.MouseDoubleClick += (sender, args) =>
        {
            if (SelectedItem is not ServiceBusReceivedMessage message) return;
            var messageDetailsWindow = messageDetailsWindowFactory.Create(message, serviceBusEntity, topicEntity, subscriptionEntity);
            messageDetailsWindow.Show();
        };
    }
}