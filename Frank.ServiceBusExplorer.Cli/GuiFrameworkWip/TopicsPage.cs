using Frank.ServiceBusExplorer.Cli.GuiFrameworkWip.ActionItems;
using Frank.ServiceBusExplorer.Cli.GuiFrameworkWip.Menues;
using Frank.ServiceBusExplorer.Models;

using Microsoft.Extensions.Hosting;

using Spectre.Console;
using Spectre.Console.Rendering;

namespace Frank.ServiceBusExplorer.Cli.GuiFrameworkWip;

public class TopicsPage(IServiceBusRepository serviceBusRepository, IHostApplicationLifetime hostApplicationLifetime, INavigator navigator) : IPage
{
    public Guid Id { get; } = PageIds.TopicsPageId;
    public string Title => PageNames.TopicsPageName;
    
    private ServiceBusEntity? _data;
    
    private IAsyncMenu<AsyncActionItem> _menu = null;
    
    public async Task<IRenderable> GetViewAsync()
    {
        var topics = serviceBusRepository.GetTopicsAsync(_data!.Name, hostApplicationLifetime.ApplicationStopping).GetAwaiter().GetResult();
        IEnumerable<TopicEntity> topicEntities = topics.ToList();
        
        var actions = new List<AsyncActionItem>()
        {
            new("Refresh", async () => await navigator.NavigateToAsync(PageIds.TopicsPageId, _data)),
            new("Back", async () => await navigator.GoBackAsync())
        };
        actions.AddRange(topicEntities.Select(topicEntity => new AsyncActionItem($"{topicEntity.Name} (Topics: {topicEntity.TopicCount}, Queues: {topicEntity.QueueCount}, Size: {topicEntity.SizeInBytes:N}B)", () => navigator.NavigateToAsync(PageIds.SubscriptionsPageId, topicEntity.Name))));

        _menu = MenuFactory.CreateAsyncMenu("Select a Topic", actions, async topic => await navigator.NavigateToAsync(PageIds.SubscriptionsPageId, topic.Name));

        var prompt = await _menu.GetPromptAsync();
        await navigator.UpdateMenuOptionsAsync(prompt);
        
        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .ShowHeaders()
            .Title("Topics")
            .AddColumn("Name")
            .AddColumn("Subscriptions")
            .AddColumn("Size");

        foreach (var topic in topicEntities)
        {
            table.AddRow(topic.Name, topic.SubscriptionCount.ToString(), topic.SizeInBytes.ToString());
        }
        
        return table;
    }

    public SelectionPrompt<ActionItem> GetOptions()
    {
        return new SelectionPrompt<ActionItem>();
    }
    
    public void SetData(object data) => _data = data as ServiceBusEntity;
}

//
    // public async Task StartAsync()
    // {
    //     SetupLayout();
    //     try
    //     {
    //         await DisplayRootMenuAsync();
    //     }
    //     catch (Exception e)
    //     {
    //         var alert = uiFactory.CreateAlert();
    //         alert.ShowException(e);
    //     }
    //     DisplayShutDownHaltingMessage();
    // }
    //
    // public void SetupLayout()
    // {
    //     var grid = new Grid()
    //         .AddColumn(new GridColumn().NoWrap().PadRight(1))
    //         .AddColumn(new GridColumn().PadLeft(1))
    //         .AddRow("[b]Breadcrumbs:[/] " + navigationService.GetBreadcrumbs())
    //         .AddEmptyRow();
    //
    //     var panel = new Panel(grid)
    //         .Expand()
    //         .BorderStyle(new Style(foreground: Color.Grey))
    //         .Header("[b]Service Bus Explorer[/]");
    //
    //     AnsiConsole.Write(panel);
    // }
    //
    //
    // private async Task DisplayRootMenuAsync()
    // {
    //     var figlet = new FigletText("Frank's Service Bus Explorer")
    //         .Centered()
    //         .Color(Color.Green);
    //     AnsiConsole.Write(figlet);
    //
    //     var actions = new[]
    //     {
    //         new AsyncActionItem("Display Service Bus Configuration", DisplayAsync),
    //         new AsyncActionItem("Display Service Bus Tree", DisplayServiceBusTreeAsync),
    //         new AsyncActionItem("Exit", async () => hostApplicationLifetime.StopApplication())
    //     };
    //
    //     var menu = uiFactory.CreateAsyncMenu("Select an action", actions, item => item.Name, selectedItem => selectedItem.Action());
    //     await menu.DisplayAsync();
    // }
    //
    // private async Task ShowTopicsMenuAsync(ServiceBusEntity serviceBus)
    // {
    //     var topics = await serviceBusRepository.GetTopicsAsync(serviceBus.Name, hostApplicationLifetime.ApplicationStopping);
    //     var topicMenu = uiFactory.CreateAsyncMenu("Select a Topic", topics, topic => topic.Name, topic => ShowSubscriptionsMenuAsync(serviceBus.Name, topic));
    //     await topicMenu.DisplayAsync();
    // }
    //
    // private async Task ShowSubscriptionsMenuAsync(string serviceBusName, TopicEntity topic)
    // {
    //     var subscriptions = await serviceBusRepository.GetSubscriptionsAsync(serviceBusName, topic.Name, hostApplicationLifetime.ApplicationStopping);
    //     var subscriptionMenu = uiFactory.CreateAsyncMenu("Select a Subscription", subscriptions, subscription => $"{subscription.Name} ({subscription.ActiveMessageCount})", entity => ShowSubscriptionMenuAsync(entity, serviceBusName, topic.Name));
    //     await subscriptionMenu.DisplayAsync();
    // }
    //
    // private Task ShowSubscriptionMenuAsync(SubscriptionEntity subscription, string serviceBusName, string topicName)
    // {
    //     var actions = new[]
    //     {
    //         new AsyncActionItem($"Show Messages {subscription.ActiveMessageCount}", () => ShowMessagesMenuAsync(serviceBusName, topicName, subscription.Name)),
    //         new AsyncActionItem($"Show Dead Letter Messages {subscription.DeadLetterMessageCount}", () => ShowDeadLetterMessagesMenuAsync(serviceBusName, topicName, subscription.Name)),
    //         new AsyncActionItem("Back", () => Task.CompletedTask)
    //     };
    //     var messageMenu = uiFactory.CreateAsyncMenu("Select an option", actions, action => action.Name, message => message.Action());
    //     return messageMenu.DisplayAsync();
    // }
    //
    // private void DisplayShutDownHaltingMessage()
    // {
    //     AnsiConsole.MarkupLine("Press any key to exit...");
    //     Console.ReadKey();
    //     hostApplicationLifetime.StopApplication();
    // }
    //
    // private async Task ShowMessagesMenuAsync(string serviceBusName, string topicName, string subscriptionName)
    // {
    //     var messages = await serviceBusRepository.GetMessagesAsync(serviceBusName, topicName, subscriptionName, hostApplicationLifetime.ApplicationStopping);
    //     DisplayMessagesAsync(messages);
    // }
    //
    // private async Task ShowDeadLetterMessagesMenuAsync(string serviceBusName, string topicName, string subscriptionName)
    // {
    //     var messages = await serviceBusRepository.GetDeadLetterMessagesAsync(serviceBusName, topicName, subscriptionName, hostApplicationLifetime.ApplicationStopping);
    //     DisplayMessagesAsync(messages);
    // }
    //
    // private void DisplayMessagesAsync(IEnumerable<ServiceBusReceivedMessage> messages)
    // {
    //     var menu = uiFactory.CreateMenu("Select a message", messages, ConvertToMessageEssentials, ShowMessage);
    //     menu.Display();
    // }
    //
    // private void ShowMessage(ServiceBusReceivedMessage message)
    // {
    //     var jsonPage = uiFactory.CreateTextPage("Message body", message.Body.ToString());
    //     jsonPage.Display();
    // }
    //
    // private static string ConvertToMessageEssentials(ServiceBusReceivedMessage arg)
    // {
    //     var stringBuilder = new StringBuilder();
    //     
    //     stringBuilder.Append($"MessageId: {arg.MessageId}");
    //     stringBuilder.Append(" | ");
    //     stringBuilder.Append($"SequenceNumber: {arg.SequenceNumber}");
    //     stringBuilder.Append(" | ");
    //     stringBuilder.Append($"EnqueuedTime: {arg.EnqueuedTime.ToString("s")}");
    //     stringBuilder.Append(" | ");
    //     stringBuilder.Append($"ExpiresAt: {arg.ExpiresAt.ToString("s")}");
    //     stringBuilder.Append(" | ");
    //     stringBuilder.Append($"CorrelationId: {arg.CorrelationId}");
    //     stringBuilder.Append(" | ");
    //     stringBuilder.Append($"Subject: {arg.Subject}");
    //     
    //     return stringBuilder.ToString();
    // }