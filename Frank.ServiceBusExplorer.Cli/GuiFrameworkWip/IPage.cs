using Frank.ServiceBusExplorer.Cli.GuiFrameworkWip.ActionItems;

using Spectre.Console;
using Spectre.Console.Rendering;

namespace Frank.ServiceBusExplorer.Cli.GuiFrameworkWip;

public interface IPage
{
    Guid Id { get; }
    string Title { get; }
    Task<IRenderable> GetViewAsync();
    SelectionPrompt<ActionItem> GetOptions();
    void SetData(object data); // Optional: For passing data to the page
}