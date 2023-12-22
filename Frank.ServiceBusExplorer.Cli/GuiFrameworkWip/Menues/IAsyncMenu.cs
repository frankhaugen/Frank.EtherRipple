using Spectre.Console;

namespace Frank.ServiceBusExplorer.Cli.GuiFrameworkWip.Menues;

public interface IAsyncMenu<T> where T : notnull
{
    Task<SelectionPrompt<T>> GetPromptAsync();
    
    Task DisplayAsync();
    
    Task DisplayAsync(Func<T, Task> onSelect);
}