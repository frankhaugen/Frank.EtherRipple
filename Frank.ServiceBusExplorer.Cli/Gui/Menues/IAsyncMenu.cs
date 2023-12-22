namespace Frank.ServiceBusExplorer.Cli.Gui.Menues;

public interface IAsyncMenu<T>
{
    Task DisplayAsync();
}