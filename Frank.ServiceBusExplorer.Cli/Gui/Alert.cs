using Spectre.Console;

namespace Frank.ServiceBusExplorer.Cli.Gui;

public class Alert : IAlert
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
        AnsiConsole.WriteException(exception, ExceptionFormats.ShortenPaths | ExceptionFormats.ShowLinks);
        if (exception.InnerException != null) ShowException(exception.InnerException);
    }
}