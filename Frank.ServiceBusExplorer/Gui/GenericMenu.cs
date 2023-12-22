using Spectre.Console;

namespace Frank.ServiceBusExplorer.Gui;

public class GenericMenu<T>(string? prompt, IEnumerable<T> items, Func<T, string> converter, Action<T> onSelect)
    : IMenu<T> where T : notnull
{
    private readonly SelectionPrompt<T> _prompt = new SelectionPrompt<T>()
        .AddChoices(items)
        .PageSize(10)
        .UseConverter(converter)
        .Title(prompt ?? "Please choose an option...");

    public void Display()
    {
        var result = AnsiConsole.Prompt(_prompt);
        onSelect(result);
    }
}