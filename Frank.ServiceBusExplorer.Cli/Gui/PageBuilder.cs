namespace Frank.ServiceBusExplorer.Cli;

public class PageBuilder
{
    private List<IConsolePage> pages = new List<IConsolePage>();

    public PageBuilder AddPage<T>(Guid id, string displayName, Guid? parentId = null) where T : IConsolePage, new()
    {
        var page = new T
        {
            Id = id,
            DisplayName = displayName,
            ParentId = parentId
        };
        pages.Add(page);
        return this;
    }

    public IEnumerable<IConsolePage> Build()
    {
        return pages;
    }
}