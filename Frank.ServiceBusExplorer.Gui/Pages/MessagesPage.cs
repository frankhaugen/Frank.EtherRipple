using System.Windows.Controls;

using Azure.Messaging.ServiceBus;

using Frank.ServiceBusExplorer.Gui.UserControlFactories;
using Frank.ServiceBusExplorer.Models;

namespace Frank.ServiceBusExplorer.Gui.Pages;

using System.Windows.Controls;
using System.Windows;

public class MessagesPage : Page
{
    private readonly IServiceBusRepository _serviceBusRepository;
    private ListView _messagesListView;

    public MessagesPage(IServiceBusRepository serviceBusRepository)
    {
        _serviceBusRepository = serviceBusRepository;

        InitializeComponents();
    }

    private void InitializeComponents()
    {
        // Create a Grid layout
        var grid = new Grid();
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Menu row
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); // Messages list row

        // Create menu and add to Grid
        var menu = CreateMenu();
        Grid.SetRow(menu, 0);
        grid.Children.Add(menu);

        // Create messages list and add to Grid
        _messagesListView = new ListView();
        Grid.SetRow(_messagesListView, 1);
        grid.Children.Add(_messagesListView);

        // Set the Grid as the content of the page
        Content = grid;
    }

    private Menu CreateMenu()
    {
        var menu = new Menu();
        var refreshItem = new MenuItem { Header = "Refresh" };
        refreshItem.Click += RefreshItem_Click;
        menu.Items.Add(refreshItem);

        // Add more menu items as needed

        return menu;
    }

    private void RefreshItem_Click(object sender, RoutedEventArgs e)
    {
        RefreshMessagesList();
    }

    private void RefreshMessagesList()
    {
        // Clear the list
        _messagesListView.Items.Clear();

        // Get the messages
        var messages = _serviceBusRepository.GetMessagesAsync(ServiceBus.Name, Topic.Name, Subscription.Name, SubQueue.DeadLetter, CancellationToken.None).Result;

        // Add the messages to the list
        foreach (var message in messages)
        {
            _messagesListView.Items.Add(message);
        }
    }

    // Properties
    public ServiceBusEntity ServiceBus { get; set; }
    public TopicEntity Topic { get; set; }
    public SubscriptionEntity Subscription { get; set; }
    public SubQueue SubQueue { get; set; }
}
