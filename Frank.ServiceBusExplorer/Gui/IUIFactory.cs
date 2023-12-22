using System.Text.Json;

namespace Frank.ServiceBusExplorer.Gui;

public interface IUIFactory
{
    IAlert CreateAlert();
    
    IUserInput<string> CreateStringInput(string prompt);
    
    IMenu<T> CreateMenu<T>(string? prompt, IEnumerable<T> items, Func<T, string> converter, Action<T> onSelect) where T : notnull;
    
    IAsyncMenu<T> CreateAsyncMenu<T>(string? prompt, IEnumerable<T> items, Func<T, string> converter, Func<T, Task> onSelectAsync);
    
    ActionItemMenu CreateActionMenu(string? prompt, IEnumerable<ActionItem> items, Action<ActionItem> onSelect);
    
    ITable CreateTable<T>(IEnumerable<T> items, Func<T, string[]> converter);
    
    IPage CreateJsonPage(string jsonDocument);
}