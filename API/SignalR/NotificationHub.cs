using System.Collections.Concurrent;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR;

/// <summary>
/// SignalR hub for managing user notifications, including handling connections and disconnections.
/// </summary>
[Authorize]
public class NotificationHub : Hub
{ 
  /// <summary>
  /// A thread-safe dictionary to keep track of user email and connection ID mappings.
  /// </summary>
  private static readonly ConcurrentDictionary<string, string> UserConnections = new();

  /// <summary>
  /// Handles the event when a client connects to the SignalR hub.
  /// </summary>
  /// <returns>A task representing the asynchronous operation.</returns>
  public override Task OnConnectedAsync()
  {
    var email = Context.User?.GetEmail();

    if(!string.IsNullOrEmpty(email)) UserConnections[email] = Context.ConnectionId;

    return base.OnConnectedAsync();
  }

  /// <summary>
  /// Handles the event when a client disconnects from the SignalR hub.
  /// </summary>
  /// <param name="exception">Optional exception that may have caused the disconnection.</param>
  /// <returns>A task representing the asynchronous operation.</returns>
  public override Task OnDisconnectedAsync(Exception? exception)
  {
    var email = Context.User?.GetEmail();

    if(!string.IsNullOrEmpty(email)) UserConnections.TryRemove(email, out _);
    return base.OnDisconnectedAsync(exception);
  }
  
  /// <summary>
  /// Retrieves the connection ID for a given user's email address.
  /// </summary>
  /// <param name="email">The email of the user to look up.</param>
  /// <returns>The connection ID associated with the user, or null if not found.</returns>
  public static string? GetConnectionIdByEmail(string email)
  {
    UserConnections.TryGetValue(email, out var connectionId);

    return connectionId;
  }
}
