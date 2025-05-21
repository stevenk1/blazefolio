using MudBlazor.Services;
using BlazeFolio.Components;
using BlazeFolio.Application;
using BlazeFolio.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
    
// Register ErrorHandler service
builder.Services.AddScoped<BlazeFolio.Services.ErrorHandler>();

// Add application services
builder.Services.AddApplication();

// Generate connection string for LiteDB
string dbPath = Path.Combine(builder.Environment.ContentRootPath, "Data", "blazefolio.db");
string connectionString = $"Filename={dbPath};Connection=shared";

// Add infrastructure services
builder.Services.AddInfrastructure(connectionString);

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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();