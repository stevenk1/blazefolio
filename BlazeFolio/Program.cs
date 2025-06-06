using MudBlazor.Services;
using BlazeFolio.Components;
using BlazeFolio.Application;
using BlazeFolio.Infrastructure.Extensions;
using MudBlazor;
using BlazeFolio.Hubs;
using BlazeFolio.Infrastructure.StockMarket.BackgroundServices;
using BlazeFolio.Infrastructure.StockMarket.Services;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 4000;
    config.SnackbarConfiguration.HideTransitionDuration = 200;
    config.SnackbarConfiguration.ShowTransitionDuration = 200;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
    
// Register services
builder.Services.AddScoped<BlazeFolio.Services.ErrorHandler>();
builder.Services.AddScoped<BlazeFolio.Services.MarketPriceClient>();

// Add application services
builder.Services.AddApplication();

// Generate connection string for LiteDB
string dbPath = Path.Combine(builder.Environment.ContentRootPath, "Data", "blazefolio.db");
string connectionString = $"Filename={dbPath};Connection=shared";

// Add infrastructure services
builder.Services.AddInfrastructure(connectionString);

// Register the MarketPriceService
builder.Services.AddScoped<MarketPriceService>();

// Register the background service
builder.Services.AddHostedService<MarketPriceBackgroundService>();

// Add SignalR
builder.Services.AddSignalR();

// Configure logging
builder.Logging
    .ClearProviders() // Optional: Clear default providers
    .AddConsole()    // Add console logging
    .AddDebug();     // Add debug logging

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Map SignalR hub
app.MapHub<MarketPriceHub>("/marketPriceHub");

app.Run();