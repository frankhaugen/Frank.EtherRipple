using System.Windows;
using System.Windows.Controls;

namespace Frank.ServiceBusExplorer.Gui.DialogWindows;

public class HeaderedLabel : UserControl
{
    private Label _headerLabel;
    private Label _textLabel;
    
    public HeaderedLabel(string header, string text) : this()
    {
        Header = header;
        Text = text;
    }

    public HeaderedLabel()
    {
        // Create and configure the grid layout
        var grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        // Create header label
        _headerLabel = new Label { HorizontalAlignment = HorizontalAlignment.Left };
        Grid.SetColumn(_headerLabel, 0);
        grid.Children.Add(_headerLabel);

        // Create text label
        _textLabel = new Label { HorizontalAlignment = HorizontalAlignment.Right };
        Grid.SetColumn(_textLabel, 1);
        grid.Children.Add(_textLabel);

        // Set the grid as the content of the control
        this.Content = grid;
    }

    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header), typeof(string), typeof(HeaderedLabel), new PropertyMetadata(string.Empty, OnHeaderChanged));

    public string Header
    {
        get => (string)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is HeaderedLabel control)
        {
            control._headerLabel.Content = e.NewValue;
        }
    }

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text), typeof(string), typeof(HeaderedLabel), new PropertyMetadata(string.Empty, OnTextChanged));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is HeaderedLabel control)
        {
            control._textLabel.Content = e.NewValue;
        }
    }
}