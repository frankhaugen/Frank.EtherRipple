namespace Frank.ServiceBusExplorer.Cli.Gui.ActionItems;

public class ActionItem
{
    public string Name { get; init; }
    public Action Action { get; init; }
    
    public ActionItem(string name, Action action)
    {
        Name = name;
        Action = action;
    }
    
    public override string ToString() => Name;
}