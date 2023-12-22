using System.Windows.Controls;

namespace Frank.ServiceBusExplorer.Gui.UserControls;

public class ListViewModel<T> : ListView
{
    public ListViewModel(IEnumerable<T> items)
    {
        DataContext = this;
        ItemsSource = items;
    }
}