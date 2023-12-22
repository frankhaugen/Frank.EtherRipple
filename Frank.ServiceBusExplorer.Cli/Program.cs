// See https://aka.ms/new-console-template for more information

using Frank.ServiceBusExplorer;
using Frank.ServiceBusExplorer.Cli;
using Frank.ServiceBusExplorer.Cli.Gui;
using Frank.ServiceBusExplorer.Infrastructure;
using Frank.ServiceBusExplorer.Infrastructure.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings());

builder.Services.AddSingleton<IServiceBusConfigurationService>(new ServiceBusConfigurationService(new FileInfo(Path.Combine(AppContext.BaseDirectory, "ServiceBusConfigurationItems.json"))));
builder.Services.AddSingleton<IUIFactory, UiFactory>();
builder.Services.AddSingleton<IServiceBusMenuService, ServiceBusMenuService>();
builder.Services.AddSingleton<IServiceBusRepository, ServiceBusRepository>();
builder.Services.AddSingleton<HostService>();

var app = builder.Build();

var hostService = app.Services.GetRequiredService<HostService>();

await hostService.StartAsync();