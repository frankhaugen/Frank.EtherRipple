namespace Frank.ServiceBusExplorer.Gui;

public interface IUIFactory
{
    IAlert CreateAlert();
    
    IUserInput<string> CreateStringInput(string prompt);
    
    IMenu<T> CreateMenu<T>(string? prompt, IEnumerable<T> items, Func<T, string> converter, Action<T> onSelect) where T : notnull;
    
    ActionItemMenu CreateActionMenu(string? prompt, IEnumerable<ActionItem> items, Action<ActionItem> onSelect);
}