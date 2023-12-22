using Frank.ServiceBusExplorer.Cli.GuiFrameworkWip.ActionItems;

using Spectre.Console;

namespace Frank.ServiceBusExplorer.Cli.GuiFrameworkWip;

public class ConsoleWindow
{
    private readonly IEnumerable<IPage> pages;
    private IPage currentPage;
    private readonly List<Breadcrumb> _breadcrumbs = [];

    public ConsoleWindow(IEnumerable<IPage> pages)
    {
        this.pages = pages;
    }
    public Func<IPage, Task> OnPageChangeRequest;

    public async Task DisplayPageAsync(IPage page)
    {
        RenderLayout();
        var content = await page.GetViewAsync();
        AnsiConsole.Write(content);
        
    }
    
    public void Show()
    {
        NavigateTo(PageIds.RootPageId);
        while (true)
        {
            DisplayCurrentPage().GetAwaiter().GetResult();
        }
    }
    
    public async Task DisplayCurrentPage()
    {
        RenderLayout();
        var content = await currentPage.GetViewAsync();
        AnsiConsole.Write(content);

        var prompt = currentPage.GetOptions();
        
        if (_breadcrumbs.Count > 1)
        {
            prompt.AddChoice(new ActionItem("Go back", GoBack));
        }
        
        prompt.AddChoices(pages.Select(p => new ActionItem(p.Title, () => NavigateTo(p.Id))));
        
        prompt.AddChoice(new ActionItem("Exit", () =>
        {
            AnsiConsole.Clear();
            AnsiConsole.WriteLine("Goodbye!");
            Environment.Exit(0);
        }));
        
        var result = AnsiConsole.Prompt(prompt);
        result.Action();
    }

    private void RenderLayout()
    {
        AnsiConsole.Clear();
        UpdateTitle();
        UpdateBreadcrumbs();
    }

    public void NavigateTo(Guid pageId)
    {
        var page = pages.FirstOrDefault(p => p.Id == pageId);
        if (page != null)
        {
            currentPage = page;

            if (pageId != PageIds.RootPageId) // Check if it's not the RootPage
            {
                var existingBreadcrumbIndex = _breadcrumbs.FindIndex(b => b.Id == pageId);
                if (existingBreadcrumbIndex >= 0)
                {
                    _breadcrumbs.RemoveRange(existingBreadcrumbIndex, _breadcrumbs.Count - existingBreadcrumbIndex);
                }
                else
                {
                    _breadcrumbs.Add(new Breadcrumb { Name = page.Title, Id = page.Id });
                }
            }

            UpdateBreadcrumbs();
        }
    }


    public void GoBack()
    {
        if (_breadcrumbs.Count > 1)
        {
            _breadcrumbs.RemoveAt(_breadcrumbs.Count - 1);
            var previousPageId = _breadcrumbs.Last().Id;
            var page = pages.FirstOrDefault(p => p.Id == previousPageId);
            if (page != null)
            {
                currentPage = page;
                UpdateBreadcrumbs();
            }
        }
        else
        {
            AnsiConsole.WriteLine("Cannot go back any further");
        }
    }


    private void UpdateBreadcrumbs()
    {
        var breadcrumbString = string.Join(" > ", _breadcrumbs.Select(b => b.Name));
        var breadcrumbs = new Panel(breadcrumbString)
            .RoundedBorder()
            .BorderColor(Color.Chartreuse2_1);
    
        AnsiConsole.Write(breadcrumbs);
    }


    private static void UpdateTitle()
    {
        var title = new FigletText("Frank's Service Bus Explorer")
            .Centered()
            .Color(Color.Green1);
        
        AnsiConsole.Write(title);
    }
}