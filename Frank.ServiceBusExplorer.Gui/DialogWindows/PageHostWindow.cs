using System.Windows;
using System.Windows.Controls;

using Frank.ServiceBusExplorer.Gui.UserControls;

namespace Frank.ServiceBusExplorer.Gui.DialogWindows;

public class PageHostWindow<T> : Window where T : Page
{
    public PageHostWindow(T child)
    {
        Content = child;
        InitializeComponent();
    }
    
    private void InitializeComponent()
    {
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        WindowStyle = WindowStyle.ToolWindow;
        SizeToContent = SizeToContent.WidthAndHeight;
        ShowInTaskbar = false;
        ShowActivated = true;
    }
    
    public new T Content
    {
        get => ((PageView<T>)base.Content).Content;
        init => base.Content = new PageView<T>(value);
    }
}