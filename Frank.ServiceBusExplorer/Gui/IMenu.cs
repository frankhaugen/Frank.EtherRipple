namespace Frank.ServiceBusExplorer.Gui;

public interface IMenu<T> where T : notnull
{
    void Display();
}