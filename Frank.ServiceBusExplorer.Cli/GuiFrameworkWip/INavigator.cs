namespace Frank.ServiceBusExplorer.Cli.GuiFrameworkWip;

public interface INavigator
{
    Task NavigateToAsync(Guid pageId);
    Task GoBackAsync();
    IPage GetCurrentPage();
}