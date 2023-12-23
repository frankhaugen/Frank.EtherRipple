using System.Windows.Controls;

using Frank.ServiceBusExplorer.Gui.UserControlFactories;
using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControls;

public class ServiceBusMessagesExpanders : StackPanel
{
    private readonly IServiceBusRepository _serviceBusRepository;
    private readonly IListViewModelFactory _listViewModelFactory;
    private readonly ServiceBusEntity _serviceBus;
    private readonly TopicEntity _topic;
    private readonly SubscriptionEntity _subscription;
        
    public ServiceBusMessagesExpanders(IServiceBusRepository serviceBusRepository, ServiceBusEntity serviceBus, TopicEntity topic, SubscriptionEntity subscription, IListViewModelFactory listViewModelFactory)
    {
        _serviceBusRepository = serviceBusRepository;
        _serviceBus = serviceBus;
        _topic = topic;
        _subscription = subscription;
        _listViewModelFactory = listViewModelFactory;

        var deadLetterMessagesExpander = new MessagesExpander(_serviceBusRepository, _serviceBus, _topic, _subscription, "Dead Letter Messages");
        var activeMessagesExpander = new MessagesExpander(_serviceBusRepository, _serviceBus, _topic, _subscription, "Active Messages");
        
        Children.Add(deadLetterMessagesExpander);
        Children.Add(activeMessagesExpander);
        
        deadLetterMessagesExpander.Expanded += async (sender, args) =>
        {
            var messages = await _serviceBusRepository.GetDeadLetterMessagesAsync(_serviceBus.Name, _topic.Name, _subscription.Name, CancellationToken.None);
            
            Dispatcher.Invoke(() => 
            {
                deadLetterMessagesExpander.Content = _listViewModelFactory.Create(messages, _serviceBus, _topic, _subscription);
            });
        };
        
        activeMessagesExpander.Expanded += async (sender, args) =>
        {
            var messages = await _serviceBusRepository.GetMessagesAsync(_serviceBus.Name, _topic.Name, _subscription.Name, CancellationToken.None);
            
            Dispatcher.Invoke(() => 
            {
                activeMessagesExpander.Content = _listViewModelFactory.Create(messages, _serviceBus, _topic, _subscription);
            });
        };
    }
}