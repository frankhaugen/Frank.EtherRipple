using System.Windows.Controls;

using Frank.ServiceBusExplorer.Gui.UserControlFactories;
using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControls;

public class ServiceBusMessagesExpanders : StackPanel
{
    public ServiceBusMessagesExpanders(IServiceBusRepository serviceBusRepository, ServiceBusEntity serviceBus, TopicEntity topic, SubscriptionEntity subscription, IListViewModelFactory listViewModelFactory)
    {

        var deadLetterMessagesExpander = new Expander()
        {
            Header = $"Dead Letter Messages ({subscription.DeadLetterMessageCount})"
        };
        var activeMessagesExpander = new Expander()
        {
            Header = $"Active Messages ({subscription.ActiveMessageCount})"
        };
        
        Children.Add(deadLetterMessagesExpander);
        Children.Add(activeMessagesExpander);
        
        deadLetterMessagesExpander.Expanded += async (sender, args) =>
        {
            var messages = await serviceBusRepository.GetDeadLetterMessagesAsync(serviceBus.Name, topic.Name, subscription.Name, CancellationToken.None);
            
            Dispatcher.Invoke(() => 
            {
                deadLetterMessagesExpander.Content = listViewModelFactory.Create(messages, serviceBus, topic, subscription);
            });
        };
        
        activeMessagesExpander.Expanded += async (sender, args) =>
        {
            var messages = await serviceBusRepository.GetMessagesAsync(serviceBus.Name, topic.Name, subscription.Name, CancellationToken.None);
            
            Dispatcher.Invoke(() => 
            {
                activeMessagesExpander.Content = listViewModelFactory.Create(messages, serviceBus, topic, subscription);
            });
        };
    }
}