namespace Frank.ServiceBusExplorer.Cli.GuiFrameworkWip;

public class Navigator : INavigator
{
    private readonly ConsoleWindow consoleWindow;
    private readonly IEnumerable<IPage> pages;
    private readonly Stack<Guid> pageHistory = new Stack<Guid>();

    public Navigator(ConsoleWindow consoleWindow, IEnumerable<IPage> pages)
    {
        this.consoleWindow = consoleWindow;
        this.pages = pages;
    }

    public async Task NavigateToAsync(Guid pageId)
    {
        var page = pages.FirstOrDefault(p => p.Id == pageId);
        if (page != null)
        {
            pageHistory.Push(pageId);
            await consoleWindow.DisplayPageAsync(page);
        }
    }

    public async Task GoBackAsync()
    {
        if (pageHistory.Count > 1)
        {
            pageHistory.Pop(); // Current page
            var previousPageId = pageHistory.Peek();
            await NavigateToAsync(previousPageId);
        }
    }

    public IPage GetCurrentPage()
    {
        var currentPageId = pageHistory.Peek();
        return pages.FirstOrDefault(p => p.Id == currentPageId);
    }
}