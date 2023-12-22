using Spectre.Console;

namespace Frank.ServiceBusExplorer.Cli.Gui.Menues;

public class AsyncMenu<T>(string? prompt, IEnumerable<T> items, Func<T, string> converter, Func<T, Task> onSelect)
    : IAsyncMenu<T>
    where T : notnull
{
    private readonly SelectionPrompt<T> _prompt = new SelectionPrompt<T>()
        .AddChoices(items)
        .PageSize(10)
        .UseConverter(converter)
        .Title(prompt ?? "Please choose an option...");

    public Task DisplayAsync()
    {
        var result = AnsiConsole.Prompt(_prompt);
        return onSelect(result);
    }
}