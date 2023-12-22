using Spectre.Console;

namespace Frank.ServiceBusExplorer.Cli;

public interface IConsoleNavigationService
{
    Task NavigateToAsync<T>(Guid id, IConsolePage from) where T : IConsolePage;
    Task GoBackAsync();
    IConsolePage? GetCurrentPage();
    string GetBreadcrumbs();
    
    IEnumerable<IConsolePage> GetNavigationPages();
    
    Tree GetNavigationTree();
}