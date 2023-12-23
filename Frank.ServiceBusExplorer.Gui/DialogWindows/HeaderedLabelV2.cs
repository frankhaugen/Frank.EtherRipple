using System.Windows;
using System.Windows.Controls;

namespace Frank.ServiceBusExplorer.Gui.DialogWindows;

public class HeaderedLabelV2 : StackPanel
{
    public HeaderedLabelV2(string header, string text) : this()
    {
        Header = header;
        Text = text;
    }
    
    public HeaderedLabelV2()
    {
        Orientation = Orientation.Horizontal;
        Children.Add(new Label { HorizontalContentAlignment = HorizontalAlignment.Left });
        Children.Add(new Label { HorizontalContentAlignment = HorizontalAlignment.Right });
    }

    public string Header
    {
        get => (string)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register("Header", typeof(string), typeof(HeaderedLabel), new PropertyMetadata(string.Empty, OnHeaderChanged));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(HeaderedLabel), new PropertyMetadata(string.Empty, OnTextChanged));

    private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is HeaderedLabelV2 control && control.Children[0] is Label headerLabel)
        {
            headerLabel.Content = e.NewValue;
        }
    }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is HeaderedLabelV2 control && control.Children[1] is Label textLabel)
        {
            textLabel.Content = e.NewValue;
        }
    }
}