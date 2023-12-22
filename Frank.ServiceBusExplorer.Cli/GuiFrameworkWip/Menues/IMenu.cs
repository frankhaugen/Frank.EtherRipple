using Spectre.Console;
using Spectre.Console.Rendering;

namespace Frank.ServiceBusExplorer.Cli.Gui.Menues;

public interface IMenu<T> where T : notnull
{
    SelectionPrompt<T> GetPrompt();
    
    void Display();
    
    void Display(Action<T> onSelect);
}

public interface IAsyncMenu<T> where T : notnull
{
    Task<SelectionPrompt<T>> GetPromptAsync();
    
    Task DisplayAsync();
    
    Task DisplayAsync(Func<T, Task> onSelect);
}