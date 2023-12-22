using Spectre.Console;

namespace Frank.ServiceBusExplorer.Cli.Gui;

public class JsonPage : IPage
{
    private readonly Panel _panel;
    
    public JsonPage(string jsonDocument)
    {
        _panel = new Panel(jsonDocument)
            .Header("Some JSON in a panel")
            .Collapse()
            .RoundedBorder()
            .BorderColor(Color.Yellow);
    }

    public void Display()
    {
        AnsiConsole.Write(_panel);
    }
}