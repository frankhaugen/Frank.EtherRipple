using Spectre.Console;

namespace Frank.ServiceBusExplorer.Cli;

public class RootPage : IConsolePage
{
    public Guid Id { get; init; }

    public string DisplayName { get; init; }

    public Guid? ParentId { get; set; } = null;

    public async Task DisplayAsync()
    {
        var menu = new SelectionPrompt<IConsolePage>()
            .AddChoices(navigationService.GetNavigationPages())
            .Title("Please choose an option...")
            .UseConverter(p => p.DisplayName);
        var result = AnsiConsole.Prompt(menu);
        await navigationService.NavigateToAsync(result);
    }
}