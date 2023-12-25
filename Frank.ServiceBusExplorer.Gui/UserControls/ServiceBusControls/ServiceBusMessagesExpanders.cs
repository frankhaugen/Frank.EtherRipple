using System.Windows.Controls;

using Azure.Messaging.ServiceBus;

using Frank.ServiceBusExplorer.Gui.DialogWindows;
using Frank.ServiceBusExplorer.Models;

using Material.Icons;

namespace Frank.ServiceBusExplorer.Gui.UserControls.ServiceBusControls;

public class ServiceBusMessagesExpanders : StackPanel
{
    public ServiceBusMessagesExpanders(IServiceBusRepository serviceBusRepository, ServiceBusEntity serviceBus, TopicEntity topic, SubscriptionEntity subscription)
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
            var messages = await serviceBusRepository.GetMessagesAsync(serviceBus.Name, topic.Name, subscription.Name, SubQueue.DeadLetter, CancellationToken.None);
            
            var listViewModelItems = messages.Select(message => ActionableListItem.Create(
                $"Message Id: {message.MessageId}", 
                IconButton.Create(MaterialIconKind.Details, () =>
                {
                    MessageDetailsWindow.Create(serviceBusRepository, message, serviceBus, topic, subscription).Show();
                    return Task.CompletedTask;
                }, "Details")
                ));
            
            Dispatcher.Invoke(() => 
            {
                deadLetterMessagesExpander.Content = ActionableList.Create(listViewModelItems.ToArray());
            });
        };
        
        activeMessagesExpander.Expanded += async (sender, args) =>
        {
            var messages = await serviceBusRepository.GetMessagesAsync(serviceBus.Name, topic.Name, subscription.Name, SubQueue.None, CancellationToken.None);
            
            var listViewModelItems = messages.Select(message => ActionableListItem.Create(
                $"Message Id: {message.MessageId}", 
                IconButton.Create(MaterialIconKind.Details, () =>
                {
                    MessageDetailsWindow.Create(serviceBusRepository, message, serviceBus, topic, subscription).Show();
                    return Task.CompletedTask;
                }, "Details")
            ));

            Dispatcher.Invoke(() => 
            {
                activeMessagesExpander.Content = ActionableList.Create(listViewModelItems.ToArray());
            });
        };
    }
}