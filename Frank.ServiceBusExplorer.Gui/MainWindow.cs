using System.ComponentModel;
using System.Windows;

using Frank.ServiceBusExplorer.Gui.Pages;

namespace Frank.ServiceBusExplorer.Gui;

internal class MainWindow : Window
{
    private readonly ILogger<MainWindow> _logger;

    public MainWindow(ILogger<MainWindow> logger, ServiceBusTreeViewPage serviceBusTreeViewPage)
    {
        _logger = logger;
        ConfigureWindow();
        Content = serviceBusTreeViewPage;
    }

    private void ConfigureWindow()
    {
        MinWidth = 512;
        MinHeight = 256;
        
        Width = 1024;
        Height = 768;

        SizeToContent = SizeToContent.WidthAndHeight;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        _logger.LogInformation("Closing");
        base.OnClosing(e);
    }
}