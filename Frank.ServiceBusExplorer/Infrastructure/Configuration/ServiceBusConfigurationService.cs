using System.Text.Json;

namespace Frank.ServiceBusExplorer;

public class ServiceBusConfigurationService : IServiceBusConfigurationService
{
    private readonly IEnumerable<ServiceBusConfigurationItem> _serviceBusConfigurationItems;

    public ServiceBusConfigurationService(FileSystemInfo configurationFile)
    {
        if (!configurationFile.Exists)
            throw new FileNotFoundException("The specified configuration file does not exist.", configurationFile.FullName);
        var fileContents = File.ReadAllText(configurationFile.FullName);
        var serviceBusConfigurationItems = JsonSerializer.Deserialize<IEnumerable<ServiceBusConfigurationItem>>(fileContents);
        _serviceBusConfigurationItems = serviceBusConfigurationItems ?? throw new Exception("The specified configuration file does not contain any service bus configuration items.");
    }

    public ServiceBusConfigurationService(IEnumerable<ServiceBusConfigurationItem> serviceBusConfigurationItems) => _serviceBusConfigurationItems = serviceBusConfigurationItems;
    
    public IEnumerable<ServiceBusConfigurationItem> GetServiceBusConfigurationItems() => _serviceBusConfigurationItems;
    public ServiceBusConfigurationItem? GetServiceBusConfigurationItem(string name) => _serviceBusConfigurationItems.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
}