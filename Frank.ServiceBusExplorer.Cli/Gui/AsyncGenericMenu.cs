using Spectre.Console;

namespace Frank.ServiceBusExplorer.Cli.Gui;

public class AsyncGenericMenu<T> : IAsyncMenu<T> where T : notnull
{
    private readonly SelectionPrompt<T> _prompt;
    private readonly Func<T, Task> _onSelect;
    
    public AsyncGenericMenu(string? prompt, IEnumerable<T> items, Func<T, string> converter, Func<T, Task> onSelect)
    {
        _onSelect = onSelect;
        _prompt = new SelectionPrompt<T>()
            .AddChoices(items)
            .PageSize(10)
            .UseConverter(converter)
            .Title(prompt ?? "Please choose an option...");
    }
    
    public Task DisplayAsync()
    {
        var result = AnsiConsole.Prompt(_prompt);
        return _onSelect(result);
    }
}