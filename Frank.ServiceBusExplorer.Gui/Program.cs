using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

using Frank.ServiceBusExplorer.Gui.DialogFactories;
using Frank.ServiceBusExplorer.Gui.Extensions;
using Frank.ServiceBusExplorer.Gui.UserControlFactories;
using Frank.ServiceBusExplorer.Gui.UserControls;

namespace Frank.ServiceBusExplorer.Gui;

internal class Program
{
    [STAThread]
    public static void Main(params string[] args)
    {
        AllocConsole();

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                context.SetContentPathToApplicationDirectory();

                services.AddSingleton<IServiceBusConfiguration>(new ServiceBusConfiguration(new FileInfo(Path.Combine(AppContext.BaseDirectory, "ServiceBusConfigurationItems.json"))));
                services.AddSingleton<IServiceBusRepository, ServiceBusRepository>();

                // Factories
                services.AddSingleton<IMessageDetailsWindowFactory, MessageDetailsWindowFactory>();
                services.AddSingleton<IServiceBusTreeViewItemFactory, ServiceBusTreeViewItemFactory>();
                services.AddSingleton<IServiceBusTopicTreeViewItemFactory, ServiceBusTopicTreeViewItemFactory>();
                services.AddSingleton<IServiceBusSubscriptionTreeViewItemFactory, ServiceBusSubscriptionTreeViewItemFactory>();
                services.AddSingleton<IServiceBusTreeViewFactory, ServiceBusTreeViewFactory>();
                services.AddSingleton<IListViewModelFactory, ListViewModelFactory>();
                services.AddSingleton<IServiceBusMessagesExpandersFactory, ServiceBusMessagesExpandersFactory>();
                
                
                services.AddSingleton<ServiceBusTreeView>();
                services.AddScoped<Application>();

                services.AddHostedService<Worker>();

                services.AddScoped<MainWindow>();
                services.AddHostedService<WindowHost>();
                
                services.BuildServiceProvider(new ServiceProviderOptions() { ValidateOnBuild = true, ValidateScopes = true });
            })
            .Build();

        host.Run();
    }

    [DllImport("kernel32")]
    static extern bool AllocConsole();
}