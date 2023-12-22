using Azure.Messaging.ServiceBus;

using Frank.ServiceBusExplorer.Infrastructure.Entities;

namespace Frank.ServiceBusExplorer.Infrastructure;

public interface IServiceBusRepository
{
    IEnumerable<ServiceBusEntity> GetServiceBuses();
    Task<IEnumerable<TopicEntity>> GetTopicsAsync(string serviceBusName, CancellationToken cancellationToken);
    Task<IEnumerable<SubscriptionEntity>> GetSubscriptionsAsync(string serviceBusName, string topicName, CancellationToken cancellationToken);
    Task<IEnumerable<ServiceBusReceivedMessage>> GetMessagesAsync(string serviceBusName, string topicName, string subscriptionName, CancellationToken cancellationToken);
    Task<IEnumerable<ServiceBusReceivedMessage>> GetDeadLetterMessagesAsync(string serviceBusName, string topicName, string subscriptionName, CancellationToken cancellationToken);
}