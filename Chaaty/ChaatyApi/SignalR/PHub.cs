using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
namespace ChaatyApi.SignalR
{
    [Authorize]
    public class PHub:Hub
    {
        private readonly HubTraker traker;

        public PHub(HubTraker traker)
        {
            this.traker = traker;
        }
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User.Identity.GetUserId();
            await traker.AddConnctedUser(userId,Context.ConnectionId);
            await Clients.Others.SendAsync("UserOnline", userId);
           
            var onlineUsers= await traker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", onlineUsers);
        }
        public override  async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.User.Identity.GetUserId();
            await traker.RemoveDisconnectedUser(userId, Context.ConnectionId);
            await Clients.Others.SendAsync("UserOfline", Context.User.Identity.GetUserId());

            var onlineUsers = traker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", onlineUsers);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
