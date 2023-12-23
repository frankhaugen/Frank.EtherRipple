using System.ComponentModel;
using System.Windows;

using Frank.ServiceBusExplorer.Gui.UserControlFactories;

namespace Frank.ServiceBusExplorer.Gui;

internal class MainWindow : Window
{
    private readonly ILogger<MainWindow> _logger;

    public MainWindow(ILogger<MainWindow> logger, IServiceBusTreeViewFactory serviceBusTreeViewFactory)
    {
        _logger = logger;

        ConfigureWindow();

        Content = serviceBusTreeViewFactory.Create();
    }

    private void ConfigureWindow()
    {
        MinWidth = 512;
        MinHeight = 256;

        SizeToContent = SizeToContent.WidthAndHeight;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        _logger.LogInformation("Closing");
        base.OnClosing(e);
    }
}