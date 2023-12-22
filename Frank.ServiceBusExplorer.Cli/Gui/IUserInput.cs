namespace Frank.ServiceBusExplorer.Cli.Gui;

public interface IUserInput<T> where T : notnull
{
    T Display();
}