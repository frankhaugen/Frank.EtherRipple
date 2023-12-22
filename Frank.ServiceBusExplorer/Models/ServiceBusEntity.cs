namespace Frank.ServiceBusExplorer.Models;

public class ServiceBusEntity
{
    public string Name { get; set; }
    
    public int TopicCount { get; set; }
    
    public int QueueCount { get; set; }
}