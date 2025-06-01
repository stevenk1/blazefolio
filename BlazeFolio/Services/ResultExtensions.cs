using CSharpFunctionalExtensions;
using MudBlazor;

namespace BlazeFolio.Services;

/// <summary>
/// Provides extension methods for handling Result objects in a standardized way
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Handles a Result object by performing different actions for success and failure cases
    /// </summary>
    /// <typeparam name="T">The type of the result value</typeparam>
    /// <param name="result">The Result object to handle</param>
    /// <param name="snackbar">The snackbar service for displaying notifications</param>
    /// <param name="successMessage">Message to display on success</param>
    /// <param name="onSuccess">Action to perform on success, receives the result value</param>
    /// <returns>True if the result was successful, false otherwise</returns>
    public static bool HandleResult<T>(this Result<T> result, ISnackbar snackbar, string successMessage, Action<T> onSuccess)
    {
        if (result.IsSuccess)
        {
            snackbar.Add(successMessage, Severity.Success);
            onSuccess(result.Value);
            return true;
        }
        else
        {
            snackbar.Add($"Error: {result.Error}", Severity.Error);
            return false;
        }
    }

    /// <summary>
    /// Handles a Result object by performing different actions for success and failure cases
    /// </summary>
    /// <param name="result">The Result object to handle</param>
    /// <param name="snackbar">The snackbar service for displaying notifications</param>
    /// <param name="successMessage">Message to display on success</param>
    /// <param name="onSuccess">Action to perform on success</param>
    /// <returns>True if the result was successful, false otherwise</returns>
    public static bool HandleResult(this Result result, ISnackbar snackbar, string successMessage, Action onSuccess)
    {
        if (result.IsSuccess)
        {
            snackbar.Add(successMessage, Severity.Success);
            onSuccess();
            return true;
        }
        else
        {
            snackbar.Add($"Error: {result.Error}", Severity.Error);
            return false;
        }
    }

    /// <summary>
    /// Handles a Result object by displaying appropriate messages without additional actions
    /// </summary>
    /// <param name="result">The Result object to handle</param>
    /// <param name="snackbar">The snackbar service for displaying notifications</param>
    /// <param name="successMessage">Message to display on success</param>
    /// <returns>True if the result was successful, false otherwise</returns>
    public static bool HandleResult(this Result result, ISnackbar snackbar, string successMessage)
    {
        return result.HandleResult(snackbar, successMessage, () => { });
    }

    /// <summary>
    /// Handles a Result object by displaying appropriate messages without additional actions
    /// </summary>
    /// <typeparam name="T">The type of the result value</typeparam>
    /// <param name="result">The Result object to handle</param>
    /// <param name="snackbar">The snackbar service for displaying notifications</param>
    /// <param name="successMessage">Message to display on success</param>
    /// <returns>True if the result was successful, false otherwise</returns>
    public static bool HandleResult<T>(this Result<T> result, ISnackbar snackbar, string successMessage)
    {
        return result.HandleResult(snackbar, successMessage, _ => { });
    }

    /// <summary>
    /// Handles a Result object by performing different actions for success and failure cases
    /// </summary>
    /// <param name="result">The Result object to handle</param>
    /// <param name="snackbar">The snackbar service for displaying notifications</param>
    /// <param name="successMessage">Message to display on success</param>
    /// <param name="onSuccess">Action to perform on success</param>
    /// <param name="onFailure">Action to perform on failure</param>
    /// <returns>True if the result was successful, false otherwise</returns>
    public static bool HandleResult(this Result result, ISnackbar snackbar, string successMessage, Action onSuccess, Action onFailure)
    {
        if (result.IsSuccess)
        {
            snackbar.Add(successMessage, Severity.Success);
            onSuccess();
            return true;
        }
        else
        {
            snackbar.Add($"Error: {result.Error}", Severity.Error);
            onFailure();
            return false;
        }
    }

    /// <summary>
    /// Handles a Result object by performing different actions for success and failure cases
    /// </summary>
    /// <typeparam name="T">The type of the result value</typeparam>
    /// <param name="result">The Result object to handle</param>
    /// <param name="snackbar">The snackbar service for displaying notifications</param>
    /// <param name="successMessage">Message to display on success</param>
    /// <param name="onSuccess">Action to perform on success, receives the result value</param>
    /// <param name="onFailure">Action to perform on failure</param>
    /// <returns>True if the result was successful, false otherwise</returns>
    public static bool HandleResult<T>(this Result<T> result, ISnackbar snackbar, string successMessage, Action<T> onSuccess, Action onFailure)
    {
        if (result.IsSuccess)
        {
            snackbar.Add(successMessage, Severity.Success);
            onSuccess(result.Value);
            return true;
        }
        else
        {
            snackbar.Add($"Error: {result.Error}", Severity.Error);
            onFailure();
            return false;
        }
    }

    /// <summary>
    /// Handles a Result object with custom error message format
    /// </summary>
    /// <param name="result">The Result object to handle</param>
    /// <param name="snackbar">The snackbar service for displaying notifications</param>
    /// <param name="successMessage">Message to display on success</param>
    /// <param name="errorMessagePrefix">Prefix for the error message</param>
    /// <param name="onSuccess">Action to perform on success</param>
    /// <returns>True if the result was successful, false otherwise</returns>
    public static bool HandleResult(this Result result, ISnackbar snackbar, string successMessage, string errorMessagePrefix, Action onSuccess)
    {
        if (result.IsSuccess)
        {
            snackbar.Add(successMessage, Severity.Success);
            onSuccess();
            return true;
        }
        else
        {
            snackbar.Add($"{errorMessagePrefix}: {result.Error}", Severity.Error);
            return false;
        }
    }

    /// <summary>
    /// Handles a Result object with custom error message format
    /// </summary>
    /// <typeparam name="T">The type of the result value</typeparam>
    /// <param name="result">The Result object to handle</param>
    /// <param name="snackbar">The snackbar service for displaying notifications</param>
    /// <param name="successMessage">Message to display on success</param>
    /// <param name="errorMessagePrefix">Prefix for the error message</param>
    /// <param name="onSuccess">Action to perform on success, receives the result value</param>
    /// <returns>True if the result was successful, false otherwise</returns>
    public static bool HandleResult<T>(this Result<T> result, ISnackbar snackbar, string successMessage, string errorMessagePrefix, Action<T> onSuccess)
    {
        if (result.IsSuccess)
        {
            snackbar.Add(successMessage, Severity.Success);
            onSuccess(result.Value);
            return true;
        }
        else
        {
            snackbar.Add($"{errorMessagePrefix}: {result.Error}", Severity.Error);
            return false;
        }
    }
}
