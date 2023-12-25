using System.Windows;
using System.Windows.Controls;

namespace Frank.ServiceBusExplorer.Gui.UserControls;

public class SimpleDropdown<T> : ComboBox where T : class
{
    private Func<T, string> _displayMemberPath;

    public SimpleDropdown(Func<T, string> displayMemberPath)
    {
        _displayMemberPath = displayMemberPath;
    }

    public new IEnumerable<T> Items
    {
        get => (IEnumerable<T>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public static readonly DependencyProperty ItemsProperty =
        DependencyProperty.Register(nameof(Items), typeof(IEnumerable<T>), typeof(SimpleDropdown<T>), new PropertyMetadata(null, OnItemsChanged));

    private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SimpleDropdown<T> dropdown)
        {
            dropdown.ItemsSource = dropdown.Items;
        }
    }

    public T Selected
    {
        get => (T)this.SelectedItem;
        set => this.SelectedItem = value;
    }

    public static readonly DependencyProperty SelectedProperty =
        DependencyProperty.Register(nameof(Selected), typeof(T), typeof(SimpleDropdown<T>), new PropertyMetadata(null));

}