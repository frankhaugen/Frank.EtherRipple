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
        topLeftPanel.Children.Add(new HeaderedLabel("Message Id", _message.MessageId));
        topLeftPanel.Children.Add(new HeaderedLabel("Correlation Id", _message.CorrelationId));
        topLeftPanel.Children.Add(new HeaderedLabel("Content Type", _message.ContentType));
        topLeftPanel.Children.Add(new HeaderedLabel("Session Id", _message.SessionId));
        topLeftPanel.Children.Add(new HeaderedLabel("Partition Key", _message.PartitionKey));
        topLeftPanel.Children.Add(new HeaderedLabel("Reply To", _message.ReplyTo));
        _serviceBusMessageDetailsGrid.SetCellContent(0, 0, topLeftPanel, "Properties");
        
        // Header Row, second column (Times)
        var topMiddlePanel = new StackPanel();
        topMiddlePanel.Children.Add(new HeaderedLabel("Enqueued Time", _message.EnqueuedTime.ToString()));
        topMiddlePanel.Children.Add(new HeaderedLabel("Scheduled Enqueue Time", _message.ScheduledEnqueueTime.ToString()));
        topMiddlePanel.Children.Add(new HeaderedLabel("Locked Until", _message.LockedUntil.ToString()));
        topMiddlePanel.Children.Add(new HeaderedLabel("Time To Live", _message.TimeToLive.ToString()));
        _serviceBusMessageDetailsGrid.SetCellContent(0, 1, topMiddlePanel, "Times");
        
        // Header Row, third column (System Properties)
        var topRightPanel = new StackPanel();
        topRightPanel.Children.Add(new HeaderedLabel("Delivery Count", _message.DeliveryCount.ToString()));
        topRightPanel.Children.Add(new HeaderedLabel("Lock Token", _message.LockToken.ToString()));
        topRightPanel.Children.Add(new HeaderedLabel("Sequence Number", _message.SequenceNumber.ToString()));
        topRightPanel.Children.Add(new HeaderedLabel("Dead Letter Source", _message.DeadLetterSource));
        _serviceBusMessageDetailsGrid.SetCellContent(0, 2, topRightPanel, "System Properties");
        
        // Body
        _serviceBusMessageDetailsGrid.SetCellContent(1, 0, new CodeArea(IdentifyHighlightStyle(_message.ContentType)) { Text = _message.Body.ToString() }, "Body");
        
        // Footer Row, first column
        var bottomLeftPanel = new StackPanel();
        var completeButton = new Button { Content = "Complete" };
        completeButton.Click += async (sender, args) =>
        {
            await _serviceBusRepository.CompleteMessageAsync(_message, serviceBusEntity, topicEntity, subscriptionEntity);
            Close();
        };
        bottomLeftPanel.Children.Add(completeButton);
        _serviceBusMessageDetailsGrid.SetCellContent(2, 0, bottomLeftPanel, "Actions");
        
        // Footer Row, second column
        var bottomMiddlePanel = new StackPanel();
        var deadLetterButton = new Button { Content = "Dead Letter" };
        deadLetterButton.Click += async (sender, args) =>
        {
            await _serviceBusRepository.DeadLetterMessageAsync(_message, serviceBusEntity, topicEntity, subscriptionEntity);
            Close();
        };
        bottomMiddlePanel.Children.Add(deadLetterButton);
        _serviceBusMessageDetailsGrid.SetCellContent(2, 1, bottomMiddlePanel, "Actions");
        
        // Footer Row, third column (Application Properties)
        var bottomRightPanel = new StackPanel();
        foreach (var (key, value) in _message.ApplicationProperties)
        {
            bottomRightPanel.Children.Add(new HeaderedLabel(key, value.ToString()));
        }
        _serviceBusMessageDetailsGrid.SetCellContent(2, 2, bottomRightPanel, "Application Properties");
        
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