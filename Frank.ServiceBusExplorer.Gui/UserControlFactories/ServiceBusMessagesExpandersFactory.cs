using Frank.ServiceBusExplorer.Gui.UserControls;
using Frank.ServiceBusExplorer.Gui.UserControls.ServiceBusControls;
using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControlFactories;

public class ServiceBusMessagesExpandersFactory : IServiceBusMessagesExpandersFactory
{
    private readonly IServiceBusRepository _serviceBusRepository;

    public ServiceBusMessagesExpandersFactory(IServiceBusRepository serviceBusRepository)
    {
        _serviceBusRepository = serviceBusRepository;
    }

    public ServiceBusMessagesExpanders Create(ServiceBusEntity serviceBus, TopicEntity topic, SubscriptionEntity subscription)
    {
        return new ServiceBusMessagesExpanders(_serviceBusRepository, serviceBus, topic, subscription);
    }
}