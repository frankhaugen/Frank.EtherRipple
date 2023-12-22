namespace Frank.ServiceBusExplorer.Gui;

public interface IUserInput<T> where T : notnull
{
    T Display();
}