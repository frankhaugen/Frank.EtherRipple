using System.Text;

using Azure.Messaging.ServiceBus;

using Frank.ServiceBusExplorer.Cli.Gui;
using Frank.ServiceBusExplorer.Cli.Gui.ActionItems;
using Frank.ServiceBusExplorer.Models;

using Microsoft.Extensions.Hosting;

using Spectre.Console;

namespace Frank.ServiceBusExplorer.Cli;

public class HostService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IUIFactory _uiFactory;
    private readonly IServiceBusRepository _serviceBusRepository;

    public HostService(IHostApplicationLifetime hostApplicationLifetime, IUIFactory uiFactory, IServiceBusRepository serviceBusRepository)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _uiFactory = uiFactory;
        _serviceBusRepository = serviceBusRepository;
    }

    public async Task StartAsync()
    {
        try
        {
            await DisplayRootMenuAsync();
        }
        catch (Exception e)
        {
            var alert = _uiFactory.CreateAlert();
            alert.ShowException(e);
        }
        DisplayShutDownHaltingMessage();
    }

    private async Task DisplayRootMenuAsync()
    {
        var figlet = new FigletText("Frank's Service Bus Explorer")
            .Centered()
            .Color(Color.Green);
        AnsiConsole.Write(figlet);
        var actions = new[]
        {
            new AsyncActionItem() { Name = "Display Service Bus Configuration", Action = DisplayAsync },
            new AsyncActionItem { Name = "Exit", Action = async () => _hostApplicationLifetime.StopApplication() }
        };
        var menu = _uiFactory.CreateAsyncMenu("Select an action", actions, item => item.Name, selectedItem => selectedItem.Action());
        await menu.DisplayAsync();
        AnsiConsole.MarkupLine("[green]Goodbye[/]");
    }

    private void DisplayShutDownHaltingMessage()
    {
        AnsiConsole.MarkupLine("Press any key to exit...");
        Console.ReadKey();
        _hostApplicationLifetime.StopApplication();
    }

    private async Task DisplayAsync()
    {
        var serviceBuses = _serviceBusRepository.GetServiceBuses();
        var serviceBusMenu = _uiFactory.CreateAsyncMenu("Select a Service Bus", serviceBuses, bus => bus.Name, selectedBus => ShowTopicsMenuAsync(selectedBus));
        await serviceBusMenu.DisplayAsync();
    }

    private async Task ShowTopicsMenuAsync(ServiceBusEntity serviceBus)
    {
        var topics = await _serviceBusRepository.GetTopicsAsync(serviceBus.Name, _hostApplicationLifetime.ApplicationStopping);
        var topicMenu = _uiFactory.CreateAsyncMenu("Select a Topic", topics, topic => topic.Name, topic => ShowSubscriptionsMenuAsync(serviceBus.Name, topic));
        await topicMenu.DisplayAsync();
    }

    private async Task ShowSubscriptionsMenuAsync(string serviceBusName, TopicEntity topic)
    {
        var subscriptions = await _serviceBusRepository.GetSubscriptionsAsync(serviceBusName, topic.Name, _hostApplicationLifetime.ApplicationStopping);
        var subscriptionMenu = _uiFactory.CreateAsyncMenu("Select a Subscription", subscriptions, subscription => $"{subscription.Name} ({subscription.ActiveMessageCount})", entity => ShowSubscriptionMenuAsync(entity, serviceBusName, topic.Name));
        await subscriptionMenu.DisplayAsync();
    }

    private async Task ShowSubscriptionMenuAsync(SubscriptionEntity subscription, string serviceBusName, string topicName)
    {
        var actions = new[]
        {
            new AsyncActionItem() { Name = $"Show Messages {subscription.ActiveMessageCount}", Action = () => ShowMessagesMenuAsync(serviceBusName, topicName, subscription.Name) },
            new AsyncActionItem { Name = $"Show Dead Letter Messages {subscription.DeadLetterMessageCount}", Action = () => ShowDeadLetterMessagesMenuAsync(serviceBusName, topicName, subscription.Name) },
            new AsyncActionItem { Name = "Back", Action = () => Task.CompletedTask }
        };
        var messageMenu = _uiFactory.CreateAsyncMenu("Select an option", actions, action => action.Name, message => message.Action());
        await messageMenu.DisplayAsync();
    }

    private async Task ShowDeadLetterMessagesMenuAsync(string serviceBusName, string topicName, string subscriptionName)
    {
        var messages = await _serviceBusRepository.GetDeadLetterMessagesAsync(serviceBusName, topicName, subscriptionName, _hostApplicationLifetime.ApplicationStopping);
        
        await DisplayMessagesAsync(messages);
    }

    private async Task DisplayMessagesAsync(IEnumerable<ServiceBusReceivedMessage> messages)
    {
        var menu = _uiFactory.CreateAsyncMenu("Select a message", messages, ConvertToMessageEssentials, ShowMessageAsync);
        await menu.DisplayAsync();
        await Task.CompletedTask;
    }

    private Task ShowMessageAsync(ServiceBusReceivedMessage message)
    {
        var jsonPage = _uiFactory.CreateTextPage("Message body", message.Body.ToString());
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
        stringBuilder.Append($"CorrelationId: {arg.CorrelationId}");
        stringBuilder.Append(" | ");
        stringBuilder.Append($"Subject: {arg.Subject}");
        
        return stringBuilder.ToString();
    }

    private async Task ShowMessagesMenuAsync(string serviceBusName, string topicName, string subscriptionName)
    {
        var messages = await _serviceBusRepository.GetMessagesAsync(serviceBusName, topicName, subscriptionName, _hostApplicationLifetime.ApplicationStopping);
        
        await DisplayMessagesAsync(messages);
    }
}