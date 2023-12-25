using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer;

public class ServiceBusRepository : IServiceBusRepository
{
    private readonly Dictionary<string, ServiceBusEntityFactory> _serviceBusEntityFactories = new();

    public ServiceBusRepository(IServiceBusConfiguration serviceBusConfigurationService)
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

    public IEnumerable<ServiceBusEntity> GetServiceBuses() => _serviceBusEntityFactories.Select(x => x.Value.Entity);

    public async Task<IEnumerable<TopicEntity>> GetTopicsAsync(string serviceBusName, CancellationToken cancellationToken)
    {
        var topics = new List<TopicEntity>();
        var client = _serviceBusEntityFactories[serviceBusName];
        await foreach (var topic in client.GetTopicsAsync(cancellationToken))
        {
            topics.Add(new TopicEntity { Name = topic.Name, SubscriptionCount = topic.SubscriptionCount, SizeInBytes = topic.SizeInBytes});
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

    public async Task<IEnumerable<ServiceBusReceivedMessage>> GetMessagesAsync(string serviceBusName, string topicName, string subscriptionName, SubQueue subQueue, CancellationToken cancellationToken)
    {
        var client = _serviceBusEntityFactories[serviceBusName];
        var receiver = client.GetSubscriptionReceiver(topicName, subscriptionName, subQueue);
        var messages = await receiver.ReceiveMessagesAsync(10, TimeSpan.FromSeconds(3), cancellationToken);
        return messages;
    }

    public async Task CompleteMessageAsync(ServiceBusReceivedMessage message, ServiceBusEntity serviceBusEntity, TopicEntity topicEntity, SubscriptionEntity subscriptionEntity)
    {
        var client = _serviceBusEntityFactories[serviceBusEntity.Name];
        var receiver = client.GetSubscriptionReceiver(topicEntity.Name, subscriptionEntity.Name, SubQueue.None);
        await receiver.CompleteMessageAsync(message);
    }
    
    public async Task DeadLetterMessageAsync(ServiceBusReceivedMessage message, ServiceBusEntity serviceBusEntity, TopicEntity topicEntity, SubscriptionEntity subscriptionEntity)
    {
        var client = _serviceBusEntityFactories[serviceBusEntity.Name];
        var receiver = client.GetSubscriptionReceiver(topicEntity.Name, subscriptionEntity.Name, SubQueue.None);
        await receiver.DeadLetterMessageAsync(message);
    }
}