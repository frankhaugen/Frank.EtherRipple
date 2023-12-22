using Frank.ServiceBusExplorer.Cli.Gui.Menues;
using Frank.ServiceBusExplorer.Cli.Gui.Pages;

namespace Frank.ServiceBusExplorer.Cli.Gui;

public interface IUIFactory
{
    IAlert CreateAlert();
    
    IMenu<T> CreateMenu<T>(string? prompt, IEnumerable<T> items, Func<T, string> converter, Action<T> onSelect) where T : notnull;
    
    IAsyncMenu<T> CreateAsyncMenu<T>(string? prompt, IEnumerable<T> items, Func<T, string> converter, Func<T, Task> onSelectAsync) where T : notnull;
    
    IPage CreateTextPage(string heading, string body);
}