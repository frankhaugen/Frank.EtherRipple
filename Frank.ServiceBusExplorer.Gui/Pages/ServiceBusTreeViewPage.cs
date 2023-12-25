using System.Windows.Controls;

using Frank.ServiceBusExplorer.Gui.UserControlFactories;

namespace Frank.ServiceBusExplorer.Gui.Pages;

public class ServiceBusTreeViewPage : Page
{
    public ServiceBusTreeViewPage(IServiceBusTreeViewFactory serviceBusTreeViewFactory)
    {
        Content = serviceBusTreeViewFactory.Create();
    }
}