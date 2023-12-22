using Spectre.Console;

namespace Frank.ServiceBusExplorer.Cli.Gui;

public class GenericMenu<T> : IMenu<T> where T : notnull
{
    private readonly SelectionPrompt<T> _prompt;
    private readonly Action<T> _onSelect;

    public GenericMenu(string? prompt, IEnumerable<T> items, Func<T, string> converter, Action<T> onSelect)
    {
        _onSelect = onSelect;
        _prompt = new SelectionPrompt<T>()
            .AddChoices(items)
            .PageSize(10)
            .UseConverter(converter)
            .Title(prompt ?? "Please choose an option...");
    }

    public void Display()
    {
        var result = AnsiConsole.Prompt(_prompt);
        _onSelect(result);
    }
}