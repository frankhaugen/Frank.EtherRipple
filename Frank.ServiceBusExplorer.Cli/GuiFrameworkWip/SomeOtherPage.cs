using Frank.ServiceBusExplorer.Cli.Gui.ActionItems;

using Spectre.Console;
using Spectre.Console.Rendering;

namespace Frank.ServiceBusExplorer.Cli.GuiFrameworkWip;

public class SomeOtherPage : IPage
{
    public Guid Id { get; } = PageIds.SomeOtherPageId;
    public string Title => PageNames.SomeOtherPageName;
    
    private object? _data;

    public IRenderable GetView()
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Yellow)
            .Expand()
            .ShowHeaders()
            .ShowFooters()
            .AddColumn("Namae")
            .AddColumn("Typfe")
            .AddColumn("Valaaue");
        table.AddRow("Namffe", "Type", "Value");
        table.AddRow("Nffame", "Type", "Value");
        table.AddRow("Namwwe", "Type", "Value");
        
        return table;
    }

    public SelectionPrompt<ActionItem> GetOptions()
    {
        return new SelectionPrompt<ActionItem>();
    }


    public void SetData(object data)
    {
        _data = data;
    }
}