using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Frank.ServiceBusExplorer.Gui.Commands;

using Material.Icons;
using Material.Icons.WPF;

namespace Frank.ServiceBusExplorer.Gui.UserControls;

public class IconButton : Button
{
    public static IconButton Create(MaterialIconKind kind, Action action, string tooltip)
    {
        return new IconButton(new MaterialIcon() {Kind = kind}, new RelayCommand(o => action.Invoke())) {ToolTip = tooltip};
    }
    
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        "Icon", typeof(MaterialIcon), typeof(IconButton), new PropertyMetadata(default(MaterialIcon)));

    public MaterialIcon Icon
    {
        get => (MaterialIcon)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public IconButton(MaterialIcon icon, ICommand command)
    {
        Icon = icon;
        Command = command;
        InitializeComponents();
    }
    
    private void InitializeComponents()
    {
        FontFamily = new FontFamily("Awesome"); // Set the font family to the FontAwesome font

        // Set the style of the button
        Background = Brushes.Transparent;
        BorderBrush = Brushes.Transparent;
        Foreground = Brushes.Black; // Set default icon color
        Cursor = Cursors.Hand;

        // Bind the Content of the button to the Icon property
        SetBinding(ContentProperty, new System.Windows.Data.Binding(nameof(Icon)) { Source = this });

        // Event handlers for mouse enter and leave
        MouseEnter += (s, e) => Background = Brushes.LightGray; // Change color on hover
        MouseLeave += (s, e) => Background = Brushes.Transparent; // Revert on mouse leave
    }
}