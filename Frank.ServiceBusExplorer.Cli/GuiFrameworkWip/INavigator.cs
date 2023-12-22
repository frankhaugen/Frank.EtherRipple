using Frank.ServiceBusExplorer.Cli.GuiFrameworkWip.ActionItems;
using Frank.ServiceBusExplorer.Models;

using Spectre.Console;

namespace Frank.ServiceBusExplorer.Cli.GuiFrameworkWip;

public interface INavigator
{
    Task NavigateToAsync(Guid pageId);
    Task NavigateToAsync(Guid pageId, object data);
    Task GoBackAsync();
    IPage GetCurrentPage();
    event Func<SelectionPrompt<AsyncActionItem>, Task> MenuUpdated; // Event to notify when the menu is updated
    Task UpdateMenuOptionsAsync(SelectionPrompt<AsyncActionItem> getPromptAsync);
}