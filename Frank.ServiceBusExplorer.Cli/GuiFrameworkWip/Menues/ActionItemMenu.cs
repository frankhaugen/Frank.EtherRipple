using Frank.ServiceBusExplorer.Cli.GuiFrameworkWip.ActionItems;

using Spectre.Console;

namespace Frank.ServiceBusExplorer.Cli.GuiFrameworkWip.Menues;

public class ActionItemMenu : IMenu<ActionItem>
{
    private readonly SelectionPrompt<ActionItem> _prompt;
    private readonly Action<ActionItem>? _onSelect;

    public ActionItemMenu(string? prompt, IEnumerable<ActionItem> items, Action<ActionItem> onSelect)
    {
        _onSelect = onSelect;
        _prompt = new SelectionPrompt<ActionItem>()
            .AddChoices(items)
            .PageSize(10)
            .UseConverter(item => item.Name)
            .Title(prompt ?? "Please choose an option...");
    }
    
    public ActionItemMenu(string? prompt, IEnumerable<ActionItem> items) =>
        _prompt = new SelectionPrompt<ActionItem>()
            .AddChoices(items)
            .PageSize(10)
            .UseConverter(item => item.Name)
            .Title(prompt ?? "Please choose an option...");

    public SelectionPrompt<ActionItem> GetPrompt() => _prompt;
    
    public void Display()
    {
        var result = AnsiConsole.Prompt(_prompt);
        _onSelect?.Invoke(result);
    }
    
    public void Display(Action<ActionItem> onSelect)
    {
        var result = AnsiConsole.Prompt(_prompt);
        onSelect(result);
    }
}