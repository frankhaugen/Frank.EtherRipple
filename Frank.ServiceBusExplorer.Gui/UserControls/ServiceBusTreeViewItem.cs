using System.Windows.Controls;

using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControls;

public class ServiceBusTreeViewItem : TreeViewItem
{
    private readonly IServiceBusRepository _serviceBusRepository;
    private readonly ServiceBusEntity _serviceBus;

    private readonly List<TopicEntity> _entities = new();
    
    public ServiceBusTreeViewItem(IServiceBusRepository serviceBusRepository, ServiceBusEntity serviceBus)
    {
        _serviceBusRepository = serviceBusRepository;
        _serviceBus = serviceBus;
        _entities = _serviceBusRepository.GetTopicsAsync(_serviceBus.Name, CancellationToken.None).GetAwaiter().GetResult().ToList();
        
        Header = _serviceBus.Name;
        
        foreach (var entity in _entities)
        {
            var serviceBusTreeViewItem = new ServiceBusTopicTreeViewItem(_serviceBusRepository, _serviceBus, entity);
            Items.Add(serviceBusTreeViewItem);
        }
    }
}