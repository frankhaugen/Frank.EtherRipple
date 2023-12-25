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
    private readonly ServiceBusEntity _serviceBusEntity;
    private readonly TopicEntity _topicEntity;
    private readonly SubscriptionEntity _subscriptionEntity;
    private readonly ServiceBusMessageDetailsGrid _serviceBusMessageDetailsGrid = new();

    public MessageDetailsWindow(IServiceBusRepository serviceBusRepository, ServiceBusReceivedMessage message, ServiceBusEntity serviceBusEntity, TopicEntity topicEntity, SubscriptionEntity subscriptionEntity)
    {
        _message = message;
        _serviceBusRepository = serviceBusRepository;
        _serviceBusEntity = serviceBusEntity;
        _topicEntity = topicEntity;
        _subscriptionEntity = subscriptionEntity;

        InitializeWindow();
        BuildLayout();
        Content = _serviceBusMessageDetailsGrid;
    }

    private void InitializeWindow()
    {
        Title = $"Message Details - {_message.MessageId} - {_message.State}";
        MinWidth = 400;
        MinHeight = 300;
        SizeToContent = SizeToContent.WidthAndHeight;
    }

    private void BuildLayout()
    {
        BuildHeaderRow();
        BuildBody();
        BuildFooterRow();
    }

    private void BuildHeaderRow()
    {
        _serviceBusMessageDetailsGrid.SetCellContent(0, 0, CreatePropertiesPanel(), "Properties");
        _serviceBusMessageDetailsGrid.SetCellContent(0, 1, CreateTimesPanel(), "Times");
        _serviceBusMessageDetailsGrid.SetCellContent(0, 2, CreateSystemPropertiesPanel(), "System Properties");
    }

    private StackPanel CreateSystemPropertiesPanel()
    {
        var panel = new StackPanel();
        panel.Children.Add(new HeaderedLabel("Delivery Count", _message.DeliveryCount.ToString()));
        panel.Children.Add(new HeaderedLabel("Lock Token", _message.LockToken.ToString()));
        panel.Children.Add(new HeaderedLabel("Sequence Number", _message.SequenceNumber.ToString()));
        panel.Children.Add(new HeaderedLabel("Dead Letter Source", _message.DeadLetterSource));
        return panel;
    }

    private StackPanel CreateTimesPanel()
    {
        var panel = new StackPanel();
        panel.Children.Add(new HeaderedLabel("Enqueued Time", _message.EnqueuedTime.ToString()));
        panel.Children.Add(new HeaderedLabel("Scheduled Enqueue Time", _message.ScheduledEnqueueTime.ToString()));
        panel.Children.Add(new HeaderedLabel("Locked Until", _message.LockedUntil.ToString()));
        panel.Children.Add(new HeaderedLabel("Time To Live", _message.TimeToLive.ToString()));
        return panel;
    }

    private StackPanel CreatePropertiesPanel()
    {
        var panel = new StackPanel();
        panel.Children.Add(new HeaderedLabel("Message Id", _message.MessageId));
        panel.Children.Add(new HeaderedLabel("Session Id", _message.SessionId));
        panel.Children.Add(new HeaderedLabel("Correlation Id", _message.CorrelationId));
        panel.Children.Add(new HeaderedLabel("To", _message.To));
        panel.Children.Add(new HeaderedLabel("Reply To", _message.ReplyTo));
        panel.Children.Add(new HeaderedLabel("Reply To Session Id", _message.ReplyToSessionId));
        panel.Children.Add(new HeaderedLabel("Label", _message.Subject));
        return panel;
    }
    
    private void BuildBody()
    {
        var codeEditor = new CodeEditor(IdentifyHighlightStyle(_message.ContentType)) { Text = _message.Body.ToString(), IsReadOnly = true };
        
        _serviceBusMessageDetailsGrid.SetCellContent(1, 0, codeEditor, "Body");
    }

    private void BuildFooterRow()
    {
        _serviceBusMessageDetailsGrid.SetCellContent(2, 0, CreateActionsPanel(), "Actions");
        _serviceBusMessageDetailsGrid.SetCellContent(2, 2, CreateApplicationPropertiesPanel(), "Application Properties");
    }

    private StackPanel CreateActionsPanel()
    {
        var panel = new StackPanel();
        var deadLetterButton = CreateButton("Dead Letter", DeadLetterMessage);
        panel.Children.Add(deadLetterButton);

        var completeButton = CreateButton("Complete", CompleteMessage);
        panel.Children.Add(completeButton);

        return panel;
    }

    private Button CreateButton(string content, RoutedEventHandler handler)
    {
        var button = new Button { Content = content };
        button.Click += handler;
        button.Margin = new Thickness(11, 11, 11, 0);
        return button;
    }
    
private void DeadLetterMessage(object sender, RoutedEventArgs e)
    {
        _serviceBusRepository.DeadLetterMessageAsync(_message, _serviceBusEntity, _topicEntity, _subscriptionEntity);
        Close();
    }

    private void CompleteMessage(object sender, RoutedEventArgs e)
    {
        _serviceBusRepository.CompleteMessageAsync(_message, _serviceBusEntity, _topicEntity, _subscriptionEntity);
        Close();
    }

    private StackPanel CreateApplicationPropertiesPanel()
    {
        var panel = new StackPanel();
        foreach (var (key, value) in _message.ApplicationProperties)
        {
            panel.Children.Add(new HeaderedLabel(key, value.ToString()));
        }

        return panel;
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