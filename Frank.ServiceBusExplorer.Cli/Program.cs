// See https://aka.ms/new-console-template for more information

using Frank.ServiceBusExplorer;
using Frank.ServiceBusExplorer.Cli.GuiFrameworkWip;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings());

// ServiceBus
builder.Services.AddSingleton<IServiceBusConfiguration>(new ServiceBusConfiguration(new FileInfo(Path.Combine(AppContext.BaseDirectory, "ServiceBusConfigurationItems.json"))));
builder.Services.AddSingleton<IServiceBusRepository, ServiceBusRepository>();

// UI
builder.Services.AddSingleton<ConsoleWindow>();
builder.Services.AddTransient<IPage, ServiceBusPage>();
builder.Services.AddTransient<IPage, TopicsPage>();
builder.Services.AddSingleton<IPage, RootPage>();
builder.Services.AddSingleton<INavigator, Navigator>();

var app = builder.Build();

// Framework
var navigator = app.Services.GetRequiredService<INavigator>();
await navigator.NavigateToAsync(PageIds.RootPageId);
