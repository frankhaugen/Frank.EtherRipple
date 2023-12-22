namespace Frank.ServiceBusExplorer.Cli.Gui;

public interface IAlert
{
    void ShowMessage(string message);
    void ShowError(string error);
    void ShowException(Exception exception);
}