using Spectre.Console;

namespace Frank.ServiceBusExplorer.Gui;

public class SpectreAlert : IAlert
{
    public void ShowMessage(string message)
    {
        AnsiConsole.MarkupLine(message);
    }

    public void ShowError(string error)
    {
        AnsiConsole.MarkupLine($"[red bold]{error}[/]");
    }
    
    public void ShowException(Exception exception)
    {
        AnsiConsole.MarkupLine($"[red bold]{exception.Message}[/]");
        AnsiConsole.MarkupLine($"[red bold]{exception.StackTrace}[/]");
        if (exception.InnerException != null) ShowException(exception.InnerException);
    }
}