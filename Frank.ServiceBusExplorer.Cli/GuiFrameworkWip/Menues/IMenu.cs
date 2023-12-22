using Spectre.Console;

namespace Frank.ServiceBusExplorer.Cli.GuiFrameworkWip.Menues;

public interface IMenu<T> where T : notnull
{
    SelectionPrompt<T> GetPrompt();
    
    void Display();
    
    void Display(Action<T> onSelect);
}