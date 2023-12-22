using Spectre.Console;

namespace Frank.ServiceBusExplorer.Gui;

public class Table<T> : ITable
{
    private readonly Table _table;

    public Table(IEnumerable<T> items, Func<T, string[]> converter)
    {
        var type = typeof(T);
        items = items.ToList();
        var table = new Table()
            .ShowHeaders()
            .Border(TableBorder.Rounded)
            .Title(type.Name);
        var properties = type.GetProperties();
    
        foreach (var property in properties)
        {
            table.AddColumn(property.Name);
        }
    
        foreach (var item in items)
        {
            table.AddRow(converter(item));
        }

        _table = table;
    }

    public void Display() => AnsiConsole.Write(_table);
}