namespace Frank.ServiceBusExplorer.Cli.Gui;

public class ActionItemMenu(string? prompt, IEnumerable<ActionItem> items, Action<ActionItem> onSelect) : GenericMenu<ActionItem>(prompt, items, item => item.Name, onSelect);