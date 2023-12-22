using Frank.ServiceBusExplorer.Cli.Gui.ActionItems;

using Spectre.Console;
using Spectre.Console.Rendering;

namespace Frank.ServiceBusExplorer.Cli.GuiFrameworkWip;

public interface IPage
{
    Guid Id { get; }
    string Title { get; }
    IRenderable GetView();
    SelectionPrompt<ActionItem> GetOptions();
    void SetData(object data); // Optional: For passing data to the page
}