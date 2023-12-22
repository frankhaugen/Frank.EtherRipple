using Spectre.Console;

namespace Frank.ServiceBusExplorer.Cli;

public class ConsoleNavigationService : IConsoleNavigationService
{
    private readonly IEnumerable<IConsolePage> _pages;
    private readonly Stack<IConsolePage> pageStack = new();

    public ConsoleNavigationService(IEnumerable<IConsolePage> pages)
    {
        _pages = pages;
    }

    public Task NavigateToAsync<T>() where T : IConsolePage
    {
        var page = _pages.OfType<T>().Single();
        pageStack.Push(page);
        return page.DisplayAsync();
    }

    public async Task NavigateToAsync<T>(Guid id, IConsolePage from) where T : IConsolePage
    {
        
        var page = _pages.OfType<T>().Single(p => p.Id == id);
        
        if (page.ParentId != null && page.ParentId != from.Id)
        {
            await NavigateToAsync<ErrorPage>();
            return;
        }

        await NavigateToAsync<T>();
    }

    public async Task GoBackAsync()
    {
        if (pageStack.Count > 1)
        {
            pageStack.Pop();
            var previousPage = pageStack.TryPeek(out var page) ? page : null;
            if (previousPage == null)
                return;
            await previousPage.DisplayAsync();
        }
        await Task.CompletedTask;
    }

    public IConsolePage? GetCurrentPage() => pageStack.TryPeek(out var page) ? page : null;

    public string GetBreadcrumbs() => string.Join(" > ", pageStack.Reverse().Select(p => p?.GetType().Name));
    public IEnumerable<IConsolePage> GetNavigationPages() => _pages;
    public Tree GetNavigationTree()
    {
        var tree = new Tree(GetBreadcrumbs());
        foreach (var page in _pages)
        {
            tree.AddNode(page.GetType().Name);
        }
        return tree;
    }
}