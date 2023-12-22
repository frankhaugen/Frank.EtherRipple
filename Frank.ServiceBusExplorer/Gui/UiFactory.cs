using Frank.ServiceBusExplorer.Gui.UserInputs;

namespace Frank.ServiceBusExplorer.Gui;

public class UiFactory : IUIFactory
{
    public IAlert CreateAlert() => new SpectreAlert();

    public IUserInput<string> CreateStringInput(string promptText) => new SpectreStringUserInput(promptText);
    
    public IMenu<T> CreateMenu<T>(string? prompt, IEnumerable<T> items, Func<T, string> converter, Action<T> onSelect) where T : notnull
        => new GenericMenu<T>(prompt, items, converter, onSelect);

    public ActionItemMenu CreateActionMenu(string? prompt, IEnumerable<ActionItem> items, Action<ActionItem> onSelect) => new(prompt, items, onSelect);
}