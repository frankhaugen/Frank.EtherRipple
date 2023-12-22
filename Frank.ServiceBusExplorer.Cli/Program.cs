// See https://aka.ms/new-console-template for more information

using Frank.ServiceBusExplorer;
using Frank.ServiceBusExplorer.Cli;
using Frank.ServiceBusExplorer.Cli.Gui;
using Frank.ServiceBusExplorer.Cli.GuiFrameworkWip;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings());

// ServiceBus
builder.Services.AddSingleton<IServiceBusConfiguration>(new ServiceBusConfiguration(new FileInfo(Path.Combine(AppContext.BaseDirectory, "ServiceBusConfigurationItems.json"))));
builder.Services.AddSingleton<IUIFactory, UiFactory>();
builder.Services.AddSingleton<IServiceBusRepository, ServiceBusRepository>();
builder.Services.AddSingleton<IConsoleNavigationService, ConsoleNavigationService>();
builder.Services.AddSingleton<HostService>();

// Framework
builder.Services.AddSingleton<ConsoleWindow>();
builder.Services.AddTransient<IPage, ServiceBusPage>();
builder.Services.AddTransient<IPage, SomeOtherPage>();
builder.Services.AddSingleton<IPage, RootPage>();

var app = builder.Build();

// Framework
var consoleWindow = app.Services.GetRequiredService<ConsoleWindow>();
consoleWindow.Show();

// ServiceBus
// var hostService = app.Services.GetRequiredService<HostService>();
// await hostService.StartAsync();