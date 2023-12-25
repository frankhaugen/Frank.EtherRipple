using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.UserControls.ServiceBusControls;

public class ServiceBusDropdown : SimpleDropdown<ServiceBusEntity>
{
    public ServiceBusDropdown(IServiceBusRepository serviceBusRepository) : base(msg => $"{msg.Name} (Topics: {msg.TopicCount}, Queues: {msg.QueueCount})")
    {
        Items = serviceBusRepository.GetServiceBuses();
    }
}