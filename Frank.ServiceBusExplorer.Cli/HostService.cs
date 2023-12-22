using Frank.ServiceBusExplorer.Cli.Gui;

using Microsoft.Extensions.Hosting;

using Spectre.Console;

namespace Frank.ServiceBusExplorer.Cli;

public class HostService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IUIFactory _uiFactory;
    private readonly IServiceBusMenuService _serviceBusMenuService;

    public HostService(IHostApplicationLifetime hostApplicationLifetime, IUIFactory uiFactory, IServiceBusMenuService serviceBusMenuService)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _uiFactory = uiFactory;
        _serviceBusMenuService = serviceBusMenuService;
    }

    public async Task StartAsync()
    {
        try
        {
            await DisplayRootMenuAsync();
        }
        catch (Exception e)
        {
            var alert = _uiFactory.CreateAlert();
            alert.ShowException(e);
        }
        DisplayShutDownHaltingMessage();
    }

    private async Task DisplayRootMenuAsync()
    {
        var figlet = new FigletText("Frank's Service Bus Explorer")
            .Centered()
            .Color(Color.Green);
        AnsiConsole.Write(figlet);
        var actions = new[]
        {
            new AsyncActionItem() { Name = "Display Service Bus Configuration", Action = DisplayServiceBusConfiguration },
            new AsyncActionItem { Name = "Exit", Action = async () => _hostApplicationLifetime.StopApplication() }
        };
        var menu = _uiFactory.CreateAsyncMenu("Select an action", actions, item => item.Name, selectedItem => selectedItem.Action());
        await menu.DisplayAsync();
        AnsiConsole.MarkupLine("[green]Goodbye[/]");
    }

    private async Task DisplayServiceBusConfiguration() 
        => await _serviceBusMenuService.DisplayAsync(_hostApplicationLifetime.ApplicationStopping);

    private void DisplayShutDownHaltingMessage()
    {
        AnsiConsole.MarkupLine("Press any key to exit...");
        Console.ReadKey();
        _hostApplicationLifetime.StopApplication();
    }
}