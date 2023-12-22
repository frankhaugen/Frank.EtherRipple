using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace Frank.ServiceBusExplorer.Infrastructure;

public class ServiceBusRepository : IServiceBusRepository
{
    private readonly Dictionary<string, ServiceBusEntityFactory> _serviceBusEntityFactories = new();

    public ServiceBusRepository(IServiceBusConfigurationService serviceBusConfigurationService)
    {
        var serviceBusConfigurationItems = serviceBusConfigurationService.GetServiceBusConfigurationItems();
        
        foreach (var serviceBusConfigurationItem in serviceBusConfigurationItems)
        {
            var client = new ServiceBusClient(serviceBusConfigurationItem.ConnectionString);
            var administrationClient = new ServiceBusAdministrationClient(serviceBusConfigurationItem.ConnectionString);
            var serviceBusEntityFactory = new ServiceBusEntityFactory(serviceBusConfigurationItem.Name, client, administrationClient);
            _serviceBusEntityFactories.Add(serviceBusConfigurationItem.Name, serviceBusEntityFactory);
        }
    }

    public IEnumerable<ServiceBusEntity> GetServiceBuses() => _serviceBusEntityFactories.Keys.Select(x => new ServiceBusEntity { Name = x });

    public async Task<IEnumerable<TopicEntity>> GetTopicsAsync(string serviceBusName, CancellationToken cancellationToken)
    {
        var topics = new List<TopicEntity>();
        var client = _serviceBusEntityFactories[serviceBusName];
        await foreach (var topic in client.GetTopicsAsync(cancellationToken))
        {
            topics.Add(new TopicEntity { Name = topic.Name, SubscriptionCount = topic.SubscriptionCount});
        }
        return topics;
    }

    public async Task<IEnumerable<SubscriptionEntity>> GetSubscriptionsAsync(string serviceBusName, string topicName, CancellationToken cancellationToken)
    {
        var client = _serviceBusEntityFactories[serviceBusName];
        var subscriptions = new List<SubscriptionEntity>();

        await foreach (var subscription in client.GetSubscriptionsAsync(topicName, cancellationToken))
        {
            subscriptions.Add(new SubscriptionEntity { Name = subscription.SubscriptionName, ActiveMessageCount = subscription.ActiveMessageCount, DeadLetterMessageCount = subscription.DeadLetterMessageCount, TotalMessageCount = subscription.TotalMessageCount });
        }

        return subscriptions;
    }

    public async Task<IEnumerable<ServiceBusReceivedMessage>> GetMessagesAsync(string serviceBusName, string topicName, string subscriptionName, CancellationToken cancellationToken)
    {
        var client = _serviceBusEntityFactories[serviceBusName];
        var receiver = client.GetSubscriptionReceiver(topicName, subscriptionName);
        var messages = new List<ServiceBusReceivedMessage>();
        await foreach (var message in receiver.ReceiveMessagesAsync(cancellationToken))
        {
            messages.Add(message);
        }
        return messages;
    }

    public async Task<IEnumerable<ServiceBusReceivedMessage>> GetDeadLetterMessagesAsync(string serviceBusName, string topicName, string subscriptionName, CancellationToken cancellationToken)
    {
        var client = _serviceBusEntityFactories[serviceBusName];
        var receiver = client.GetDeadLetterReceiver(topicName, subscriptionName);
        var messages = new List<ServiceBusReceivedMessage>();
        await foreach (var message in receiver.ReceiveMessagesAsync(cancellationToken))
        {
            messages.Add(message);
        }
        return messages;
    }
}