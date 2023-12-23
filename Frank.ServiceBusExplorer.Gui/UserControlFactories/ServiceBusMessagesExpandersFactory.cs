using Frank.ServiceBusExplorer.Gui.UserControls;
using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControlFactories;

public class ServiceBusMessagesExpandersFactory : IServiceBusMessagesExpandersFactory
{
    private readonly IServiceBusRepository _serviceBusRepository;
    private readonly IListViewModelFactory _listViewModelFactory;

    public ServiceBusMessagesExpandersFactory(IServiceBusRepository serviceBusRepository, IListViewModelFactory listViewModelFactory)
    {
        _serviceBusRepository = serviceBusRepository;
        _listViewModelFactory = listViewModelFactory;
    }

    public ServiceBusMessagesExpanders Create(ServiceBusEntity serviceBus, TopicEntity topic, SubscriptionEntity subscription)
    {
        return new ServiceBusMessagesExpanders(_serviceBusRepository, serviceBus, topic, subscription, _listViewModelFactory);
    }
}