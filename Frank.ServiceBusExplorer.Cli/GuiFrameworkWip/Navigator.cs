using Frank.ServiceBusExplorer.Cli.GuiFrameworkWip.ActionItems;
using Frank.ServiceBusExplorer.Models;

using Spectre.Console;

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
        
        consoleWindow.OnPageChangeRequest = DisplayPageAsync;
        MenuUpdated += this.consoleWindow.OnMenuUpdateRequestedAsync;
    }

    public event Func<SelectionPrompt<AsyncActionItem>, Task> MenuUpdated;
    
    public async Task UpdateMenuOptionsAsync(SelectionPrompt<AsyncActionItem> selectionPrompt)
    {
        await MenuUpdated.Invoke(selectionPrompt);
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

    public async Task NavigateToAsync(Guid pageId, object data)
    {
        var page = pages.FirstOrDefault(p => p.Id == pageId);
        if (page != null)
        {
            page.SetData(data);
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
    
    private async Task DisplayPageAsync(IPage page)
    {
        var view= await page.GetViewAsync();
        AnsiConsole.Write(view);
    }
}