using Frank.ServiceBusExplorer.Cli.GuiFrameworkWip.ActionItems;
using Frank.ServiceBusExplorer.Cli.GuiFrameworkWip.Menues;

namespace Frank.ServiceBusExplorer.Cli.GuiFrameworkWip;

public static class MenuFactory
{
    public static IAsyncMenu<AsyncActionItem> CreateAsyncMenu(string? prompt, IEnumerable<AsyncActionItem> items, Func<AsyncActionItem, Task> onSelect) => new AsyncMenu<AsyncActionItem>(prompt, items, item => item.Name, onSelect);
    
    public static IMenu<ActionItem> CreateMenu(string? prompt, IEnumerable<ActionItem> items, Action<ActionItem> onSelect) => new Menu<ActionItem>(prompt, items, item => item.Name, onSelect);
}