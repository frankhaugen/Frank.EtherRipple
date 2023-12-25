using System.Windows.Controls;

namespace Frank.ServiceBusExplorer.Gui.UserControls;

public class PageView<T> : Frame where T : Page
{
    public PageView<T> Create(T child) => new(child);
    public PageView(T child)
    {
        Content = child;
        InitializeComponent();
    }
    
    public new T Content
    {
        get => (T)base.Content;
        init => base.Content = value;
    }

    private void InitializeComponent()
    {
        Width = Content.Width;
        Height = Content.Height;
        MinWidth = Content.MinWidth;
        MinHeight = Content.MinHeight;
        MaxWidth = Content.MaxWidth;
        MaxHeight = Content.MaxHeight;
    }
}