using Frank.ServiceBusExplorer.Gui.UserControls;

namespace Frank.ServiceBusExplorer.Gui.UserControlFactories;

internal interface IServiceBusTreeViewFactory
{
    ServiceBusTreeView Create();
}