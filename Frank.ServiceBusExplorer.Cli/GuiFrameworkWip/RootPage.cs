using Frank.ServiceBusExplorer.Cli.Gui.ActionItems;

using Microsoft.Extensions.Hosting;

using Spectre.Console;
using Spectre.Console.Rendering;

namespace Frank.ServiceBusExplorer.Cli.GuiFrameworkWip;

public class RootPage(IServiceBusRepository serviceBusRepository, IHostApplicationLifetime hostApplicationLifetime) : IPage
{
    public Guid Id { get; } = PageIds.RootPageId;
    public string Title => PageNames.RootPageName;
    
    private object? _data;

    public IRenderable GetView()
    {
        var tree = new Tree("Service Buses Overview")
            .Style(Style.Parse("green"))
            .Guide(TreeGuide.Line);

        var serviceBuses = serviceBusRepository.GetServiceBuses();
        foreach (var serviceBus in serviceBuses)
        {
            var serviceBusNode = tree.AddNode($"{serviceBus.Name} (Topics: {serviceBus.TopicCount}, Queues: {serviceBus.QueueCount})");
            var topics = serviceBusRepository.GetTopicsAsync(serviceBus.Name, hostApplicationLifetime.ApplicationStopping).GetAwaiter().GetResult();

            foreach (var topic in topics)
            {
                var topicNode = serviceBusNode.AddNode($"{topic.Name} (Subscriptions: {topic.SubscriptionCount}, Size: {topic.SizeInBytes})");
                var subscriptions = serviceBusRepository.GetSubscriptionsAsync(serviceBus.Name, topic.Name, hostApplicationLifetime.ApplicationStopping).GetAwaiter().GetResult();

                foreach (var subscription in subscriptions)
                {
                    topicNode.AddNode($"{subscription.Name} (Messages: {subscription.ActiveMessageCount}, Dead Letters: {subscription.DeadLetterMessageCount})");
                }
            }
        }
        
        return tree;
    }

    public SelectionPrompt<ActionItem> GetOptions()
    {
        return new SelectionPrompt<ActionItem>();
    }

    public void SetData(object data)
    {
        _data = data;
    }
}