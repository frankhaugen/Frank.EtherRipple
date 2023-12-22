using System.Text;
using System.Text.Json;

using Azure.Messaging.ServiceBus;

using Frank.ServiceBusExplorer.Gui;
using Frank.ServiceBusExplorer.Infrastructure;
using Frank.ServiceBusExplorer.Infrastructure.Entities;

namespace Frank.ServiceBusExplorer;

public class ServiceBusMenuService : IServiceBusMenuService
{
    private readonly IUIFactory _uiFactory;
    private readonly IServiceBusRepository _serviceBusRepository;

    public ServiceBusMenuService(IUIFactory uiFactory, IServiceBusRepository serviceBusRepository)
    {
        _uiFactory = uiFactory;
        _serviceBusRepository = serviceBusRepository;
    }
    
    public async Task DisplayAsync(CancellationToken cancellationToken)
    {
        var serviceBuses = _serviceBusRepository.GetServiceBuses();
        var serviceBusMenu = _uiFactory.CreateAsyncMenu("Select a Service Bus", serviceBuses, bus => bus.Name, selectedBus => ShowTopicsMenuAsync(selectedBus, cancellationToken));
        await serviceBusMenu.DisplayAsync();
    }

    private async Task ShowTopicsMenuAsync(ServiceBusEntity serviceBus, CancellationToken cancellationToken)
    {
        var topics = await _serviceBusRepository.GetTopicsAsync(serviceBus.Name, cancellationToken);
        var topicMenu = _uiFactory.CreateAsyncMenu("Select a Topic", topics, topic => topic.Name, topic => ShowSubscriptionsMenuAsync(serviceBus.Name, topic, cancellationToken));
        await topicMenu.DisplayAsync();
    }

    private async Task ShowSubscriptionsMenuAsync(string serviceBusName, TopicEntity topic, CancellationToken cancellationToken)
    {
        var subscriptions = await _serviceBusRepository.GetSubscriptionsAsync(serviceBusName, topic.Name, cancellationToken);
        var subscriptionMenu = _uiFactory.CreateAsyncMenu("Select a Subscription", subscriptions, subscription => $"{subscription.Name} ({subscription.ActiveMessageCount})", entity => ShowSubscriptionMenuAsync(entity, serviceBusName, topic.Name, cancellationToken));
        await subscriptionMenu.DisplayAsync();
    }

    private async Task ShowSubscriptionMenuAsync(SubscriptionEntity subscription, string serviceBusName, string topicName, CancellationToken cancellationToken)
    {
        var actions = new[]
        {
            new AsyncActionItem() { Name = $"Show Messages {subscription.ActiveMessageCount}", Action = () => ShowMessagesMenuAsync(serviceBusName, topicName, subscription.Name, cancellationToken) },
            new AsyncActionItem { Name = $"Show Dead Letter Messages {subscription.DeadLetterMessageCount}", Action = () => ShowDeadLetterMessagesMenuAsync(serviceBusName, topicName, subscription.Name, cancellationToken) },
            new AsyncActionItem { Name = "Back", Action = () => Task.CompletedTask }
        };
        var messageMenu = _uiFactory.CreateAsyncMenu("Select an option", actions, action => action.Name, message => message.Action());
        await messageMenu.DisplayAsync();
    }

    private async Task ShowDeadLetterMessagesMenuAsync(string serviceBusName, string topicName, string subscriptionName, CancellationToken cancellationToken)
    {
        var messages = await _serviceBusRepository.GetDeadLetterMessagesAsync(serviceBusName, topicName, subscriptionName, cancellationToken);
        
        await DisplayMessagesAsync(messages, cancellationToken);
    }

    private async Task DisplayMessagesAsync(IEnumerable<ServiceBusReceivedMessage> messages, CancellationToken cancellationToken)
    {
        var menu = _uiFactory.CreateAsyncMenu("Select a message", messages, ConvertToMessageEssentials, ShowMessageAsync);
        await menu.DisplayAsync();
        await Task.CompletedTask;
    }

    private Task ShowMessageAsync(ServiceBusReceivedMessage message)
    {
        var jsonPage = _uiFactory.CreateJsonPage(message.Body.ToString());
        jsonPage.Display();
        
        return Task.CompletedTask;
    }

    private string ConvertToMessageEssentials(ServiceBusReceivedMessage arg)
    {
        var stringBuilder = new StringBuilder();
        
        stringBuilder.Append($"MessageId: {arg.MessageId}");
        stringBuilder.Append(" | ");
        stringBuilder.Append($"SequenceNumber: {arg.SequenceNumber}");
        stringBuilder.Append(" | ");
        stringBuilder.Append($"EnqueuedTime: {arg.EnqueuedTime}");
        stringBuilder.Append(" | ");
        stringBuilder.Append($"ExpiresAt: {arg.ExpiresAt}");
        stringBuilder.Append(" | ");
        stringBuilder.Append($"ContentType: {arg.ContentType}");
        stringBuilder.Append(" | ");
        stringBuilder.Append($"CorrelationId: {arg.CorrelationId}");
        stringBuilder.Append(" | ");
        stringBuilder.Append($"Subject: {arg.Subject}");
        
        return stringBuilder.ToString();
    }

    private async Task ShowMessagesMenuAsync(string serviceBusName, string topicName, string subscriptionName, CancellationToken cancellationToken)
    {
        var messages = await _serviceBusRepository.GetMessagesAsync(serviceBusName, topicName, subscriptionName, cancellationToken);
        
        await DisplayMessagesAsync(messages, cancellationToken);
    }
}

public interface IServiceBusMenuService
{
    Task DisplayAsync(CancellationToken cancellationToken);
}