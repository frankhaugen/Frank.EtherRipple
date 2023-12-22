using Spectre.Console;

namespace Frank.ServiceBusExplorer.Cli.GuiFrameworkWip.Menues;

public class Menu<T>(string? prompt, IEnumerable<T> items, Func<T, string> converter, Action<T> onSelect)
    : IMenu<T>
    where T : notnull
{
    private readonly SelectionPrompt<T> _prompt = new SelectionPrompt<T>()
        .AddChoices(items)
        .PageSize(10)
        .UseConverter(converter)
        .Title(prompt ?? "Please choose an option...");

    public SelectionPrompt<T> GetPrompt() => _prompt;

    public void Display()
    {
        var result = AnsiConsole.Prompt(_prompt);
        onSelect(result);
    }

    public void Display(Action<T> onSelect2)
    {
        var result = AnsiConsole.Prompt(_prompt);
        onSelect2(result);
    }
}