using System.Windows;
using System.Windows.Controls;

namespace Frank.ServiceBusExplorer.Gui.UserControls;

public class ServiceBusMessageDetailsGrid : Grid
{
    private readonly GroupBox[,] _groupBoxes = new GroupBox[3, 3];

    public ServiceBusMessageDetailsGrid()
    {
        // Define columns
        ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        // Define rows
        RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        RowDefinitions.Add(new RowDefinition { Height = new GridLength(3, GridUnitType.Star) });
        RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

        // Initialize group boxes and add them to the grid
        InitializeGroupBoxes();
    }

    private void InitializeGroupBoxes()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                var groupBox = new GroupBox();
                SetRow(groupBox, row);
                SetColumn(groupBox, col);

                // Special case for the center cell
                if (row == 1 && col == 0)
                {
                    SetColumnSpan(groupBox,3);
                    
                }
                
                var scrollViewer = new ScrollViewer
                {
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto
                };
                groupBox.Content = scrollViewer;

                if (row == 1 && col >= 1)
                {
                    continue;
                }
                
                Children.Add(groupBox);
                _groupBoxes[row, col] = groupBox;
            }
        }
    }

    public void SetCellContent<T>(int row, int col, T content, string header) where T : UIElement
    {
        if (row < 0 || row > 2 || col < 0 || col > 2)
            throw new ArgumentOutOfRangeException("Row and column indices must be between 0 and 2.");

        var groupBox = _groupBoxes[row, col];

        if (groupBox.Content is ScrollViewer scrollViewer)
        {
            scrollViewer.Content = content;
        }
        else
        {
            groupBox.Content = content;
        }
        
        groupBox.Header = header;
    }
}