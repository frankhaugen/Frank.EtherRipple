using System.Windows.Controls;

using Frank.ServiceBusExplorer.Gui.UserControlFactories;
using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControls.ServiceBusControls;

public class ServiceBusTreeViewItem : TreeViewItem
{
    private readonly IServiceBusRepository _serviceBusRepository;
    private readonly ServiceBusEntity _serviceBus;
    private readonly IServiceBusTopicTreeViewItemFactory _serviceBusTopicTreeViewItemFactory;

    private readonly List<TopicEntity> _entities = new();

    public ServiceBusTreeViewItem(IServiceBusRepository serviceBusRepository, ServiceBusEntity serviceBus, IServiceBusTopicTreeViewItemFactory serviceBusTopicTreeViewItemFactory)
    {
        _serviceBusRepository = serviceBusRepository;
        _serviceBus = serviceBus;
        _serviceBusTopicTreeViewItemFactory = serviceBusTopicTreeViewItemFactory;
        _entities = _serviceBusRepository.GetTopicsAsync(_serviceBus.Name, CancellationToken.None).GetAwaiter().GetResult().ToList();

        Header = _serviceBus.Name;

        foreach (var entity in _entities)
        {
            var serviceBusTreeViewItem = _serviceBusTopicTreeViewItemFactory.Create(_serviceBus, entity);
            Items.Add(serviceBusTreeViewItem);
        }
    }
}