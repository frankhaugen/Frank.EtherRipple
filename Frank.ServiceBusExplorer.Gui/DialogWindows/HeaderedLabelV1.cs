using System.Windows;
using System.Windows.Controls;

namespace Frank.ServiceBusExplorer.Gui.DialogWindows;

public class HeaderedLabelV1 : StackPanel
{
    public HeaderedLabelV1(string header, string text)
    {
        Orientation = Orientation.Horizontal;
        Children.Add(new Label(){Content = header, HorizontalContentAlignment = HorizontalAlignment.Left});
        Children.Add(new Label(){Content = text, HorizontalContentAlignment = HorizontalAlignment.Right});
    }
}