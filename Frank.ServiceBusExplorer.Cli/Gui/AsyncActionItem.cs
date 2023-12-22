namespace Frank.ServiceBusExplorer.Cli.Gui;

public class AsyncActionItem
{
    public required string Name { get; init; }
    public required Func<Task> Action { get; init; }
}