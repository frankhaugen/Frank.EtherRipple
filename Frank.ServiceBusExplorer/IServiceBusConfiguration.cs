using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer;

public interface IServiceBusConfiguration
{
    IEnumerable<ServiceBusConfigurationItem> GetServiceBusConfigurationItems();
}