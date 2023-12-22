using System.Text.Json;

using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer;

public class ServiceBusConfiguration : IServiceBusConfiguration
{
    private readonly IEnumerable<ServiceBusConfigurationItem> _serviceBusConfigurationItems;

    public ServiceBusConfiguration(FileSystemInfo configurationFile)
    {
        if (!configurationFile.Exists)
            throw new FileNotFoundException("The specified configuration file does not exist.", configurationFile.FullName);
        var fileContents = File.ReadAllText(configurationFile.FullName);
        var serviceBusConfigurationItems = JsonSerializer.Deserialize<IEnumerable<ServiceBusConfigurationItem>>(fileContents);
        _serviceBusConfigurationItems = serviceBusConfigurationItems ?? throw new Exception("The specified configuration file does not contain any service bus configuration items.");
    }

    public IEnumerable<ServiceBusConfigurationItem> GetServiceBusConfigurationItems() => _serviceBusConfigurationItems;
}