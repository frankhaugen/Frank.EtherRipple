using Spectre.Console;

namespace Frank.ServiceBusExplorer.Cli;

public class ErrorPage : IConsolePage
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string DisplayName { get; init; } = "Error";
    public Guid? ParentId { get; set; } = null;

    public Task DisplayAsync()
    {
        AnsiConsole.MarkupLine("[red]Error![/]");
        return Task.CompletedTask;
    }
}