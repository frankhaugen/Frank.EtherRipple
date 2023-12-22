using Azure;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace Frank.ServiceBusExplorer.Infrastructure;

public class ServiceBusEntityFactory
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusAdministrationClient _administrationClient;
    
    internal ServiceBusEntityFactory(string name, ServiceBusClient client, ServiceBusAdministrationClient administrationClient)
    {
        Name = name;
        _client = client;
        _administrationClient = administrationClient;
    }

    public string Name { get; }
    
    public AsyncPageable<TopicRuntimeProperties> GetTopicsAsync(CancellationToken cancellationToken) 
        => _administrationClient.GetTopicsRuntimePropertiesAsync(cancellationToken);

    public AsyncPageable<SubscriptionRuntimeProperties> GetSubscriptionsAsync(string topicName, CancellationToken cancellationToken) 
        => _administrationClient.GetSubscriptionsRuntimePropertiesAsync(topicName, cancellationToken);

    public ServiceBusReceiver GetSubscriptionReceiver(string topicName, string subscriptionName) 
        => _client.CreateReceiver(topicName, subscriptionName, new ServiceBusReceiverOptions() { SubQueue = SubQueue.None, ReceiveMode = ServiceBusReceiveMode.PeekLock});

    public ServiceBusReceiver GetDeadLetterReceiver(string topicName, string subscriptionName) 
        => _client.CreateReceiver(topicName, subscriptionName, new ServiceBusReceiverOptions() { SubQueue = SubQueue.DeadLetter, ReceiveMode = ServiceBusReceiveMode.PeekLock});
}