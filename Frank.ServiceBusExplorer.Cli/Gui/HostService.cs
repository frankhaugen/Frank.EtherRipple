using System.Text;

using Azure.Messaging.ServiceBus;

using Frank.ServiceBusExplorer.Cli.Gui;
using Frank.ServiceBusExplorer.Cli.Gui.ActionItems;
using Frank.ServiceBusExplorer.Models;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Spectre.Console;

namespace Frank.ServiceBusExplorer.Cli;

public class HostService(IHostApplicationLifetime hostApplicationLifetime, IUIFactory uiFactory, IServiceBusRepository serviceBusRepository, IConsoleNavigationService navigationService)
{
    public async Task StartAsync()
    {
        SetupLayout();
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
    
    public void SetupLayout()
    {
        var grid = new Grid()
            .AddColumn(new GridColumn().NoWrap().PadRight(1))
            .AddColumn(new GridColumn().PadLeft(1))
            .AddRow("[b]Breadcrumbs:[/] " + navigationService.GetBreadcrumbs())
            .AddEmptyRow();

        var panel = new Panel(grid)
            .Expand()
            .BorderStyle(new Style(foreground: Color.Grey))
            .Header("[b]Service Bus Explorer[/]");

        AnsiConsole.Write(panel);
    }

    
    private async Task DisplayRootMenuAsync()
    {
        var figlet = new FigletText("Frank's Service Bus Explorer")
            .Centered()
            .Color(Color.Green);
        AnsiConsole.Write(figlet);

        var actions = new[]
        {
            new AsyncActionItem("Display Service Bus Configuration", DisplayAsync),
            new AsyncActionItem("Display Service Bus Tree", DisplayServiceBusTreeAsync),
            new AsyncActionItem("Exit", async () => hostApplicationLifetime.StopApplication())
        };

        var menu = uiFactory.CreateAsyncMenu("Select an action", actions, item => item.Name, selectedItem => selectedItem.Action());
        await menu.DisplayAsync();
    }

    private async Task DisplayServiceBusTreeAsync()
    {
        var tree = new Tree("Service Buses")
            .Style(Style.Parse("green"))
            .Guide(TreeGuide.Line);

        var serviceBuses = serviceBusRepository.GetServiceBuses();
        foreach (var serviceBus in serviceBuses)
        {
            var serviceBusNode = tree.AddNode(serviceBus.Name);
            var topics = await serviceBusRepository.GetTopicsAsync(serviceBus.Name, hostApplicationLifetime.ApplicationStopping);

            foreach (var topic in topics)
            {
                var topicNode = serviceBusNode.AddNode(topic.Name);
                var subscriptions = await serviceBusRepository.GetSubscriptionsAsync(serviceBus.Name, topic.Name, hostApplicationLifetime.ApplicationStopping);

                foreach (var subscription in subscriptions)
                {
                    topicNode.AddNode($"{subscription.Name} (Messages: {subscription.ActiveMessageCount}, Dead Letters: {subscription.DeadLetterMessageCount})");
                }
            }
        }

        AnsiConsole.Write(tree);
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
            new AsyncActionItem($"Show Messages {subscription.ActiveMessageCount}", () => ShowMessagesMenuAsync(serviceBusName, topicName, subscription.Name)),
            new AsyncActionItem($"Show Dead Letter Messages {subscription.DeadLetterMessageCount}", () => ShowDeadLetterMessagesMenuAsync(serviceBusName, topicName, subscription.Name)),
            new AsyncActionItem("Back", () => Task.CompletedTask)
        };
        var messageMenu = uiFactory.CreateAsyncMenu("Select an option", actions, action => action.Name, message => message.Action());
        return messageMenu.DisplayAsync();
    }
    
    private void DisplayShutDownHaltingMessage()
    {
        AnsiConsole.MarkupLine("Press any key to exit...");
        Console.ReadKey();
        hostApplicationLifetime.StopApplication();
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