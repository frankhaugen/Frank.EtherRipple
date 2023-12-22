namespace Frank.ServiceBusExplorer.Cli.GuiFrameworkWip.ActionItems;

public class AsyncActionItem
{
    public string Name { get; init; }
    public Func<Task> Action { get; init; }
    
    public AsyncActionItem(string name, Func<Task> action)
    {
        Name = name;
        Action = action;
    }
    
    public override string ToString() => Name;
}