using Microsoft.Extensions.Logging;
using MudBlazor;

namespace BlazeFolio.Services;

/// <summary>
/// Provides centralized error handling functionality for the application
/// </summary>
public class ErrorHandler
{
    private readonly ISnackbar _snackbar;
    private readonly ILogger<ErrorHandler> _logger;

    public ErrorHandler(ISnackbar snackbar, ILogger<ErrorHandler> logger)
    {
        _snackbar = snackbar;
        _logger = logger;
    }

    /// <summary>
    /// Handle exceptions by logging them and displaying a user-friendly notification
    /// </summary>
    /// <param name="ex">The exception that occurred</param>
    /// <param name="friendlyMessage">Optional custom message to display to the user. If not provided, the exception message will be used.</param>
    /// <param name="context">Optional context information about where the error occurred</param>
    public void HandleException(Exception ex, string? friendlyMessage = null, string? context = null)
    {
        // Log the exception with context information
        var logMessage = string.IsNullOrEmpty(context) 
            ? $"An error occurred: {ex.Message}" 
            : $"Error in {context}: {ex.Message}";
        
        _logger.LogError(ex, logMessage);
        
        // Display user-friendly notification
        var displayMessage = string.IsNullOrEmpty(friendlyMessage) 
            ? $"Error: {ex.Message}" 
            : friendlyMessage;
        
        _snackbar.Add(displayMessage, Severity.Error);
    }
}
