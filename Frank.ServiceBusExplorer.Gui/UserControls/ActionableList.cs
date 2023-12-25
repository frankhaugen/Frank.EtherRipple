using System.Windows;
using System.Windows.Controls;

namespace Frank.ServiceBusExplorer.Gui.UserControls;

public class ActionableList : ItemsControl
{
    public static ActionableList Create(params ActionableListItem[] items)
    {
        var actionableList = new ActionableList();
        foreach (var item in items)
        {
            actionableList.Items.Add(item);
        }
        return actionableList;
    }
    
    public ActionableList()
    {
        // Custom initialization, if needed
    }
    
    protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        base.OnItemsChanged(e);
        // Handle items change if necessary
    }
    
    protected override DependencyObject GetContainerForItemOverride()
    {
        // Return a new container that is tailored for ActionableListItem
        return new ContentPresenter(); // Or a custom container
    }
    
    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return item is ActionableListItem;
    }
}