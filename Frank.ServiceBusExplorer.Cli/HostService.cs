using System.Text;

using Azure.Messaging.ServiceBus;

using Frank.ServiceBusExplorer.Cli.Gui;
using Frank.ServiceBusExplorer.Cli.Gui.ActionItems;
using Frank.ServiceBusExplorer.Models;

using Microsoft.Extensions.Hosting;

using Spectre.Console;

namespace Frank.ServiceBusExplorer.Cli;

public class HostService(IHostApplicationLifetime hostApplicationLifetime, IUIFactory uiFactory, IServiceBusRepository serviceBusRepository)
{
    public async Task StartAsync()
    {
        try
        {
            await DisplayRootMenuAsync();
        }
        catch (Exception e)
        {
            var alert = uiFactory.CreateAlert();
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
            new AsyncActionItem { Name = "Exit", Action = async () => hostApplicationLifetime.StopApplication() }
        };
        var menu = uiFactory.CreateAsyncMenu("Select an action", actions, item => item.Name, selectedItem => selectedItem.Action());
        await menu.DisplayAsync();
        AnsiConsole.MarkupLine("[green]Goodbye[/]");
    }

    private void DisplayShutDownHaltingMessage()
    {
        AnsiConsole.MarkupLine("Press any key to exit...");
        Console.ReadKey();
        hostApplicationLifetime.StopApplication();
    }

    private Task DisplayAsync()
    {
        var serviceBuses = serviceBusRepository.GetServiceBuses();
        var serviceBusMenu = uiFactory.CreateAsyncMenu("Select a Service Bus", serviceBuses, bus => bus.Name, ShowTopicsMenuAsync);
        return serviceBusMenu.DisplayAsync();
    }

    private async Task ShowTopicsMenuAsync(ServiceBusEntity serviceBus)
    {
        var topics = await serviceBusRepository.GetTopicsAsync(serviceBus.Name, hostApplicationLifetime.ApplicationStopping);
        var topicMenu = uiFactory.CreateAsyncMenu("Select a Topic", topics, topic => topic.Name, topic => ShowSubscriptionsMenuAsync(serviceBus.Name, topic));
        await topicMenu.DisplayAsync();
    }

    private async Task ShowSubscriptionsMenuAsync(string serviceBusName, TopicEntity topic)
    {
        var subscriptions = await serviceBusRepository.GetSubscriptionsAsync(serviceBusName, topic.Name, hostApplicationLifetime.ApplicationStopping);
        var subscriptionMenu = uiFactory.CreateAsyncMenu("Select a Subscription", subscriptions, subscription => $"{subscription.Name} ({subscription.ActiveMessageCount})", entity => ShowSubscriptionMenuAsync(entity, serviceBusName, topic.Name));
        await subscriptionMenu.DisplayAsync();
    }

    private Task ShowSubscriptionMenuAsync(SubscriptionEntity subscription, string serviceBusName, string topicName)
    {
        var actions = new[]
        {
            new AsyncActionItem() { Name = $"Show Messages {subscription.ActiveMessageCount}", Action = () => ShowMessagesMenuAsync(serviceBusName, topicName, subscription.Name) },
            new AsyncActionItem { Name = $"Show Dead Letter Messages {subscription.DeadLetterMessageCount}", Action = () => ShowDeadLetterMessagesMenuAsync(serviceBusName, topicName, subscription.Name) },
            new AsyncActionItem { Name = "Back", Action = () => Task.CompletedTask }
        };
        var messageMenu = uiFactory.CreateAsyncMenu("Select an option", actions, action => action.Name, message => message.Action());
        return messageMenu.DisplayAsync();
    }
    
    private async Task ShowMessagesMenuAsync(string serviceBusName, string topicName, string subscriptionName)
    {
        var messages = await serviceBusRepository.GetMessagesAsync(serviceBusName, topicName, subscriptionName, hostApplicationLifetime.ApplicationStopping);
        DisplayMessagesAsync(messages);
    }
    
    private async Task ShowDeadLetterMessagesMenuAsync(string serviceBusName, string topicName, string subscriptionName)
    {
        var messages = await serviceBusRepository.GetDeadLetterMessagesAsync(serviceBusName, topicName, subscriptionName, hostApplicationLifetime.ApplicationStopping);
        DisplayMessagesAsync(messages);
    }
    
    private void DisplayMessagesAsync(IEnumerable<ServiceBusReceivedMessage> messages)
    {
        var menu = uiFactory.CreateMenu("Select a message", messages, ConvertToMessageEssentials, ShowMessage);
        menu.Display();
    }
    
    private void ShowMessage(ServiceBusReceivedMessage message)
    {
        var jsonPage = uiFactory.CreateTextPage("Message body", message.Body.ToString());
        jsonPage.Display();
    }

    private static string ConvertToMessageEssentials(ServiceBusReceivedMessage arg)
    {
        var stringBuilder = new StringBuilder();
        
        stringBuilder.Append($"MessageId: {arg.MessageId}");
        stringBuilder.Append(" | ");
        stringBuilder.Append($"SequenceNumber: {arg.SequenceNumber}");
        stringBuilder.Append(" | ");
        stringBuilder.Append($"EnqueuedTime: {arg.EnqueuedTime.ToString("s")}");
        stringBuilder.Append(" | ");
        stringBuilder.Append($"ExpiresAt: {arg.ExpiresAt.ToString("s")}");
        stringBuilder.Append(" | ");
        stringBuilder.Append($"CorrelationId: {arg.CorrelationId}");
        stringBuilder.Append(" | ");
        stringBuilder.Append($"Subject: {arg.Subject}");
        
        return stringBuilder.ToString();
    }
}