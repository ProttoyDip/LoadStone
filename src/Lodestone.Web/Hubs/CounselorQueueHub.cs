using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Lodestone.Web.Hubs;

/// <summary>Real-time counselor risk-queue updates.</summary>
[Authorize(Policy = "CanViewRiskQueue")]
public class CounselorQueueHub : Hub
{
    public const string Route = "/hubs/counselor-queue";

    public async Task BroadcastQueueUpdate(object payload)
        => await Clients.All.SendAsync("QueueUpdated", payload);
}
