using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Lodestone.Web.Hubs;

/// <summary>Real-time peer-support chat rooms.</summary>
[Authorize]
public class PeerChatHub : Hub
{
    public const string Route = "/hubs/peer-chat";

    public async Task SendMessage(string room, string message)
        => await Clients.Group(room).SendAsync("ReceiveMessage", Context.UserIdentifier, message);

    public async Task JoinRoom(string room)
        => await Groups.AddToGroupAsync(Context.ConnectionId, room);
}
