using Frank.ServiceBusExplorer.Gui.UserControls;
using Frank.ServiceBusExplorer.Gui.UserControls.ServiceBusControls;

namespace Frank.ServiceBusExplorer.Gui.UserControlFactories;

public interface IServiceBusTreeViewFactory
{
    ServiceBusTreeView Create();
}