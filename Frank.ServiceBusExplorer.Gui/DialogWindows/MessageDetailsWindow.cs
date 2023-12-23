using System.Windows.Controls;

using Frank.ServiceBusExplorer.Gui.UserControls;
using Frank.ServiceBusExplorer.Models;

using ICSharpCode.AvalonEdit.Highlighting;

namespace Frank.ServiceBusExplorer.Gui.DialogWindows;

using System.Windows;
using Azure.Messaging.ServiceBus;

public class MessageDetailsWindow : Window
{
    private readonly ServiceBusReceivedMessage _message;
    private readonly IServiceBusRepository _serviceBusRepository;
    private readonly ServiceBusMessageDetailsGrid _serviceBusMessageDetailsGrid = new();

    public MessageDetailsWindow(IServiceBusRepository serviceBusRepository, ServiceBusReceivedMessage message, ServiceBusEntity serviceBusEntity, TopicEntity topicEntity, SubscriptionEntity subscriptionEntity)
    {
        _message = message;
        _serviceBusRepository = serviceBusRepository;
        
        Title = $"Message Details - {_message.MessageId}";
        Width = 400;
        Height = 300;
        
        // Header Row, first column
        var topLeftPanel = new StackPanel();
        topLeftPanel.Children.Add(new TextArea("Message Id", _message.MessageId));
        topLeftPanel.Children.Add(new TextArea("Correlation Id", _message.CorrelationId));
        topLeftPanel.Children.Add(new TextArea("Content Type", _message.ContentType));
        topLeftPanel.Children.Add(new TextArea("Session Id", _message.SessionId));
        topLeftPanel.Children.Add(new TextArea("Partition Key", _message.PartitionKey));
        
        // Header Row, second column (Times)
        var topMiddlePanel = new StackPanel();
        topMiddlePanel.Children.Add(new TextArea("Enqueued Time", _message.EnqueuedTime.ToString()));
        topMiddlePanel.Children.Add(new TextArea("Scheduled Enqueue Time", _message.ScheduledEnqueueTime.ToString()));
        topMiddlePanel.Children.Add(new TextArea("Locked Until", _message.LockedUntil.ToString()));
        topMiddlePanel.Children.Add(new TextArea("Time To Live", _message.TimeToLive.ToString()));
        
        
        // Body
        _serviceBusMessageDetailsGrid.SetCellContent(1, 0, new CodeArea(IdentifyHighlightStyle(_message.ContentType)) { Text = _message.Body.ToString() }, "Body");
        
        
        // Footer Row, first column
        
        var bottomLeftPanel = new StackPanel();
        
        var peekLockButton = new Button { Content = "Peek Lock" };
        peekLockButton.Click += async (sender, args) =>
        {
            await _serviceBusRepository.CompleteMessageAsync(_message, serviceBusEntity, topicEntity, subscriptionEntity);
            Close();
        };
        
        Content = _serviceBusMessageDetailsGrid;
    }

    private static IHighlightingDefinition IdentifyHighlightStyle(string contentType = "plain/text") =>
        contentType switch
        {
            "application/json" => HighlightingManager.Instance.GetDefinition("JSON"),
            "application/xml" => HighlightingManager.Instance.GetDefinition("XML"),
            "plain/text" => HighlightingManager.Instance.GetDefinition("MarkDownWithFontSize"),
            _ => HighlightingManager.Instance.GetDefinition("MarkDownWithFontSize")
        };
}