using System.Windows.Controls;

using Frank.ServiceBusExplorer.Gui.UserControlFactories;

namespace Frank.ServiceBusExplorer.Gui.UserControls;

public class ServiceBusTreeView : TreeView
{
    public ServiceBusTreeView(IServiceBusRepository serviceBusRepository, IServiceBusTreeViewItemFactory serviceBusTreeViewItemFactory)
    {
        foreach (var entity in serviceBusRepository.GetServiceBuses())
        {
            var serviceBusTreeViewItem = serviceBusTreeViewItemFactory.Create(entity);
            Items.Add(serviceBusTreeViewItem);
        }
    }
}