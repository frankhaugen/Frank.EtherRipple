namespace Frank.ServiceBusExplorer.Cli;

public interface IConsolePage
{
    Guid Id { get; init; }
    string DisplayName { get; init; }
    Guid? ParentId { get; set; }
    
    Task DisplayAsync();
}