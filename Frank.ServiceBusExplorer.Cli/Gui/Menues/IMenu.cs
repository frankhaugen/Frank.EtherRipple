namespace Frank.ServiceBusExplorer.Cli.Gui.Menues;

public interface IMenu<T> where T : notnull
{
    void Display();
}