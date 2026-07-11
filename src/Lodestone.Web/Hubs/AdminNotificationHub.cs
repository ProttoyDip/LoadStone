using Microsoft.AspNetCore.SignalR;

namespace Lodestone.Web.Hubs;

public class AdminNotificationHub : Hub
{
    public const string Route = "/hubs/admin-notifications";
}
