using System.Windows.Controls;

using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControls;

public class ServiceBusTreeView : TreeView
{
    private readonly IServiceBusRepository _serviceBusRepository;

    private readonly List<ServiceBusEntity> _entities = new();
    
    public ServiceBusTreeView(IServiceBusRepository serviceBusRepository)
    {
        _serviceBusRepository = serviceBusRepository;
        _entities = _serviceBusRepository.GetServiceBuses().ToList();
        
        foreach (var entity in _entities)
        {
            var serviceBusTreeViewItem = new ServiceBusTreeViewItem(_serviceBusRepository, entity);
            Items.Add(serviceBusTreeViewItem);
        }
    }
}