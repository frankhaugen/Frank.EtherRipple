// See https://aka.ms/new-console-template for more information

using Frank.ServiceBusExplorer;
using Frank.ServiceBusExplorer.Cli;
using Frank.ServiceBusExplorer.Cli.Gui;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings());

builder.Services.AddSingleton<IServiceBusConfiguration>(new ServiceBusConfiguration(new FileInfo(Path.Combine(AppContext.BaseDirectory, "ServiceBusConfigurationItems.json"))));
builder.Services.AddSingleton<IUIFactory, UiFactory>();
builder.Services.AddSingleton<IServiceBusRepository, ServiceBusRepository>();
builder.Services.AddSingleton<IConsoleNavigationService, ConsoleNavigationService>();

builder.Services.AddSingleton<IConsolePage, RootPage>();

builder.Services.AddSingleton<HostService>();

var app = builder.Build();

var hostService = app.Services.GetRequiredService<HostService>();

await hostService.StartAsync();