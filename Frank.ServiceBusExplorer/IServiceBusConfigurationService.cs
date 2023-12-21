namespace Frank.ServiceBusExplorer;

public interface IServiceBusConfigurationService
{
    IEnumerable<ServiceBusConfigurationItem> GetServiceBusConfigurationItems();
    ServiceBusConfigurationItem? GetServiceBusConfigurationItem(string name);
}