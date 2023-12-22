using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

using Frank.ServiceBusExplorer.Gui.Extensions;

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
                services.AddScoped<Application>();

                services.AddHostedService<Worker>();

                services.AddScoped<MainWindow>();
                services.AddHostedService<WindowHost>();
            })
            .Build();

        host.Run();
    }

    [DllImport("kernel32")]
    static extern bool AllocConsole();
}