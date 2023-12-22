using Frank.ServiceBusExplorer;
using Frank.ServiceBusExplorer.Gui;

using Microsoft.Extensions.Hosting;

using Spectre.Console;

public class HostService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IUIFactory _uiFactory;
    private readonly IServiceBusConfigurationService _serviceBusConfigurationService;

    public HostService(IHostApplicationLifetime hostApplicationLifetime, IUIFactory uiFactory, IServiceBusConfigurationService serviceBusConfigurationService)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _uiFactory = uiFactory;
        _serviceBusConfigurationService = serviceBusConfigurationService;
    }

    public void Start()
    {
        try
        {
            DisplayRootMenu();
        }
        catch (Exception e)
        {
            var alert = _uiFactory.CreateAlert();
            alert.ShowException(e);
        }
    }
    
    public void DisplayRootMenu()
    {
        AnsiConsole.MarkupLine("[green]Welcome to the Service Bus Explorer[/]");
        var actions = new[]
        {
            new ActionItem { Name = "Display Service Bus Configuration", Action = DisplayServiceBusConfiguration },
            new ActionItem { Name = "Exit", Action = () => _hostApplicationLifetime.StopApplication() }
        };
        var menu = _uiFactory.CreateActionMenu("Select an action", actions, selectedItem => selectedItem.Action.Invoke());
        menu.Display();
        AnsiConsole.MarkupLine("[green]Goodbye[/]");
    }
    
    public void DisplayServiceBusConfiguration()
    {
        var configurationItems = _serviceBusConfigurationService.GetServiceBusConfigurationItems();

        ServiceBusConfigurationItem? choice = null;
        var menu = _uiFactory.CreateMenu<ServiceBusConfigurationItem>("Select", configurationItems, item => item.Name, selectedItem => choice = selectedItem);
        menu.Display();
        AnsiConsole.MarkupLine($"You chose [green]{choice!.Name}[/]");
        DisplayShutDownHaltingMessage();
    }
    
    public void DisplayShutDownHaltingMessage()
    {
        AnsiConsole.MarkupLine("Press any key to exit...");
        Console.ReadKey();
        _hostApplicationLifetime.StopApplication();
    }
}