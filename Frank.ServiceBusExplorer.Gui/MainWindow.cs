using System.ComponentModel;
using System.Windows;

using Frank.ServiceBusExplorer.Gui.Commands;
using Frank.ServiceBusExplorer.Gui.UserControlFactories;
using Frank.ServiceBusExplorer.Gui.UserControls;

using Material.Icons;
using Material.Icons.WPF;

namespace Frank.ServiceBusExplorer.Gui;

internal class MainWindow : Window
{
    private readonly ILogger<MainWindow> _logger;

    public MainWindow(ILogger<MainWindow> logger, IServiceBusTreeViewFactory serviceBusTreeViewFactory)
    {
        _logger = logger;

        ConfigureWindow();
        
        var listExperiment = new ActionableList();
        
        listExperiment.Items.Add(new ActionableListItem()
        {
            Text = "Hello",
            Actions = new List<IconButton>()
            {
                IconButton.Create(MaterialIconKind.Details, () => MessageBox.Show("Accelerometer"), "Open Accelerometer"),
            },
        });
        
        listExperiment.Items.Add(new ActionableListItem()
        {
            Text = "World",
            Actions = new List<IconButton>()
            {
                IconButton.Create(MaterialIconKind.AccessPoint, () => MessageBox.Show("AccessPoint"), "Show AccessPoint"),
                new IconButton(new MaterialIcon() {Kind = MaterialIconKind.Edit}, new RelayCommand(o => { MessageBox.Show("Edit"); })),
            },
        });
        
        Content = listExperiment;
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