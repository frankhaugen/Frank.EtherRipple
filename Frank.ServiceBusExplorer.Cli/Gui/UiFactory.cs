using Frank.ServiceBusExplorer.Cli.Gui.UserInputs;

namespace Frank.ServiceBusExplorer.Cli.Gui;

public class UiFactory : IUIFactory
{
    public IAlert CreateAlert() => new SpectreAlert();

    public IUserInput<string> CreateStringInput(string promptText) => new SpectreStringUserInput(promptText);
    
    public IMenu<T> CreateMenu<T>(string? prompt, IEnumerable<T> items, Func<T, string> converter, Action<T> onSelect) where T : notnull
        => new GenericMenu<T>(prompt, items, converter, onSelect);

    public IAsyncMenu<T> CreateAsyncMenu<T>(string? prompt, IEnumerable<T> items, Func<T, string> converter, Func<T, Task> onSelectAsync) where T : notnull
        => new AsyncGenericMenu<T>(prompt, items, converter, onSelectAsync);

    public ActionItemMenu CreateActionMenu(string? prompt, IEnumerable<ActionItem> items, Action<ActionItem> onSelect) 
        => new(prompt, items, onSelect);

    public ITable CreateTable<T>(IEnumerable<T> items, Func<T, string[]> converter) => new Table<T>(items, converter);
    public IPage CreateJsonPage(string jsonDocument) => new JsonPage(jsonDocument);
}