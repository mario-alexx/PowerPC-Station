using System;

namespace API.Errors;

/// <summary>
/// Represents a structured response for API errors.
/// </summary>
/// <param name="statusCode">The HTTP status code associated with the error.</param>
/// <param name="message">The error message to be displayed.</param>
/// <param name="details">Optional additional details about the error.</param>
public class ApiErrorResponse(int statusCode, string message, string? details)
{
  /// <summary>
  /// Gets or sets the HTTP status code associated with the error.
  /// </summary>  
  public int StatusCode { get; set; } = statusCode;

  /// <summary>
  /// Gets or sets the error message.
  /// </summary>
  public string Message { get; set; } = message;
  
  /// <summary>
  /// Gets or sets additional details about the error (optional).
  /// </summary>
  public string? Details { get; set; } = details;
}
