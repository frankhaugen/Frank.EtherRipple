using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Frank.ServiceBusExplorer.Gui.UserControls;

public class ActionableListItem : UserControl
{
    private readonly StackPanel _stackPanel;

    public ActionableListItem()
    {
        _stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
        InitializeComponents();
        DataContext = this; // Set the DataContext for binding to work
    }
    
    public static ActionableListItem Create(string text, params IconButton[] actions)
    {
        var item = new ActionableListItem 
        { 
            Text = text,
            Actions = actions
        };
        return item;
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(ActionableListItem));

    public IEnumerable<IconButton> Actions
    {
        get => (IEnumerable<IconButton>)GetValue(ActionCommandsProperty);
        init => SetValue(ActionCommandsProperty, value);
    }

    public static readonly DependencyProperty ActionCommandsProperty =
        DependencyProperty.Register(nameof(Actions), typeof(IEnumerable<IconButton>), typeof(ActionableListItem), new PropertyMetadata(null, OnActionCommandsChanged));

    private void InitializeComponents()
    {
        var textBlock = new Label();
        textBlock.SetBinding(Label.ContentProperty, new Binding(nameof(Text)));

        _stackPanel.Children.Add(textBlock);

        Content = _stackPanel;
        
        // Event handlers for mouse enter and leave
        MouseEnter += (s, e) => Background = Brushes.LightGray; // Change color on hover
        MouseLeave += (s, e) => Background = Brushes.Transparent; // Revert on mouse leave
    }

    private static void OnActionCommandsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ActionableListItem item) item.UpdateActionButtons();
    }
    
    private void UpdateActionButtons()
    {
        // Remove all buttons (if any), keep the first child (Label)
        while (_stackPanel.Children.Count > 1) 
            _stackPanel.Children.RemoveAt(_stackPanel.Children.Count - 1);

        if (!Actions.Any())
            return;

        foreach (var action in Actions) 
            _stackPanel.Children.Add(action);
    }
}
