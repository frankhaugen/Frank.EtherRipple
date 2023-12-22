namespace Frank.ServiceBusExplorer.Cli.Gui;

public interface IMenu<T> where T : notnull
{
    void Display();
}