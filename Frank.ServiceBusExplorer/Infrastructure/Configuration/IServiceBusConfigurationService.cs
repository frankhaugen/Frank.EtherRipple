namespace Frank.ServiceBusExplorer.Infrastructure.Configuration;

public interface IServiceBusConfigurationService
{
    IEnumerable<ServiceBusConfigurationItem> GetServiceBusConfigurationItems();
    ServiceBusConfigurationItem? GetServiceBusConfigurationItem(string name);
}