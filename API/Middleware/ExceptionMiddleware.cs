using System;
using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware;

/// <summary>
/// Middleware for handling exceptions globally in the application.
/// </summary>
/// <param name="env">The hosting environment, used to determine if detailed errors should be shown.</param>
/// <param name="next">The next middleware in the request pipeline.</param>
public class ExceptionMiddleware(IHostEnvironment env, RequestDelegate next)
{
  /// <summary>
  /// Invokes the middleware to handle exceptions in the request pipeline.
  /// </summary>
  /// <param name="context">The current HTTP context.</param>
  public async Task InvokeAsync(HttpContext context) 
  {
    try
    {
      await next(context);
    }
    catch (Exception ex)
    {
      await HandleExceptionAsync(context, ex, env);
    }
  }

  /// <summary>
  /// Handles the exception and returns an appropriate error response to the client.
  /// </summary>
  /// <param name="context">The HTTP context where the exception occurred.</param>
  /// <param name="ex">The exception that was thrown.</param>
  /// <param name="env">The hosting environment to determine if detailed error messages should be shown.</param>
  /// <returns>A task representing the asynchronous operation of handling the exception.</returns>
  private static Task HandleExceptionAsync(HttpContext context, Exception ex, IHostEnvironment env)
  {
    // Set the content type of the response to JSON format.
    context.Response.ContentType = "application/json";

    // Set the status code of the response to 500 (Internal Server Error).
    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

    // Create a detailed error response if the environment is Development; otherwise, provide a generic error message.
    var response = env.IsDevelopment()  
      ? new ApiErrorResponse(context.Response.StatusCode, ex.Message, ex.StackTrace)
      : new ApiErrorResponse(context.Response.StatusCode, ex.Message, "Internal Server Error");

    // Configure JSON serialization options to use camelCase naming for properties.
    var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

    // Serialize the error response to a JSON string using the specified options.
    var json = JsonSerializer.Serialize(response, options);
    // Write the JSON string to the response body.
    return context.Response.WriteAsync(json);
  }
}
