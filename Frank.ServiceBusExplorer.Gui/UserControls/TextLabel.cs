using System.Windows.Controls;

namespace Frank.ServiceBusExplorer.Gui.UserControls;

public class TextLabel : GroupBox
{
    private readonly Label _label;
    private string _text;

    public TextLabel(string header, string text)
    {
        Header = header;
        _text = text;
        _label = new Label();
        _label.Content = _text;
        _label.Height = 32;
        base.Content = _label;
    }

    public new string Content
    {
        get => _label.Content as string ?? string.Empty;
        set => _label.Content = value;
    }
}