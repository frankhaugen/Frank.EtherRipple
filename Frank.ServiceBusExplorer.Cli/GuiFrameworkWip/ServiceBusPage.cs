using Frank.ServiceBusExplorer.Cli.Gui.ActionItems;
using Frank.ServiceBusExplorer.Cli.Gui.Menues;
using Frank.ServiceBusExplorer.Models;

using Spectre.Console;
using Spectre.Console.Rendering;

namespace Frank.ServiceBusExplorer.Cli.GuiFrameworkWip;

public class ServiceBusPage(IServiceBusRepository serviceBusRepository) : IPage
{
    public Guid Id { get; } = PageIds.ServiceBusPageId;
    public string Title => PageNames.ServiceBusPageName;
    
    private object? _data;
    
    private List<ServiceBusEntity> _serviceBuses = new List<ServiceBusEntity>();

    public IRenderable GetView()
    {
        var serviceBuses = serviceBusRepository.GetServiceBuses();
        _serviceBuses.AddRange(serviceBuses);

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .ShowHeaders()
            .Title("Service Buses")
            .AddColumn("Name");

        foreach (var serviceBus in serviceBuses)
        {
            table.AddRow(serviceBus.Name);
        }
        
        return table;
    }

    public SelectionPrompt<ActionItem> GetOptions()
    {
        var actions = _serviceBuses.Select(serviceBus => new ActionItem(serviceBus.Name, () => {  }));
        var menu = MenuFactory.CreateMenu("Please choose an option...", actions, item => item.Action());
        return menu.GetPrompt();
    }

    public void SetData(object data)
    {
        _data = data;
    }
}

public static class MenuFactory
{
    public static IAsyncMenu<AsyncActionItem> CreateAsyncMenu(string? prompt, IEnumerable<AsyncActionItem> items, Func<AsyncActionItem, Task> onSelect) => new AsyncMenu<AsyncActionItem>(prompt, items, item => item.Name, onSelect);
    
    public static IMenu<ActionItem> CreateMenu(string? prompt, IEnumerable<ActionItem> items, Action<ActionItem> onSelect) => new Menu<ActionItem>(prompt, items, item => item.Name, onSelect);
}