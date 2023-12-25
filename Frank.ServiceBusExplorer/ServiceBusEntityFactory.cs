using Azure;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer;

public class ServiceBusEntityFactory
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusAdministrationClient _administrationClient;
    
    internal ServiceBusEntityFactory(string name, ServiceBusClient client, ServiceBusAdministrationClient administrationClient)
    {
        Name = name;
        _client = client;
        _administrationClient = administrationClient;

        var queueCount = _administrationClient.GetQueuesRuntimePropertiesAsync(CancellationToken.None).ToBlockingEnumerable().Count();
        var topicCount = _administrationClient.GetTopicsRuntimePropertiesAsync(CancellationToken.None).ToBlockingEnumerable().Count();
        Entity = new ServiceBusEntity { Name = name, TopicCount = topicCount, QueueCount = queueCount };
    }

    public string Name { get; }
    
    public ServiceBusEntity Entity { get; }
    
    public AsyncPageable<TopicRuntimeProperties> GetTopicsAsync(CancellationToken cancellationToken) 
        => _administrationClient.GetTopicsRuntimePropertiesAsync(cancellationToken);

    public AsyncPageable<SubscriptionRuntimeProperties> GetSubscriptionsAsync(string topicName, CancellationToken cancellationToken) 
        => _administrationClient.GetSubscriptionsRuntimePropertiesAsync(topicName, cancellationToken);

    public ServiceBusReceiver GetSubscriptionReceiver(string topicName, string subscriptionName, SubQueue subQueue) 
        => _client.CreateReceiver(topicName, subscriptionName, new ServiceBusReceiverOptions() { SubQueue = subQueue, ReceiveMode = ServiceBusReceiveMode.PeekLock});
}