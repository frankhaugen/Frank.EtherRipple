using Frank.ServiceBusExplorer.Cli.Gui.Menues;
using Frank.ServiceBusExplorer.Cli.Gui.Pages;

namespace Frank.ServiceBusExplorer.Cli.Gui;

public class UiFactory : IUIFactory
{
    public IAlert CreateAlert() => new Alert();

    public IMenu<T> CreateMenu<T>(string? prompt, IEnumerable<T> items, Func<T, string> converter, Action<T> onSelect) where T : notnull
        => new Menu<T>(prompt, items, converter, onSelect);

    public IAsyncMenu<T> CreateAsyncMenu<T>(string? prompt, IEnumerable<T> items, Func<T, string> converter, Func<T, Task> onSelectAsync) where T : notnull
        => new AsyncMenu<T>(prompt, items, converter, onSelectAsync);

    public IPage CreateTextPage(string heading, string body) => new TextPage(heading, body);
}