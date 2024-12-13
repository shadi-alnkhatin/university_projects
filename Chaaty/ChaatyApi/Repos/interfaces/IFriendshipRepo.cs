using ChaatyApi.DTOs;
using Microsoft.AspNet.Identity;

namespace ChaatyApi.Repos.interfaces
{
    public interface IFriendshipRepo
    {
        Task SendFriendRequestAsync(string sender, string receiver);
        Task AcceptFriendRequestAsync(int friendshipId);
        Task RejectFriendRequestAsync(int friendshipId);
        Task<IEnumerable<FriendshipDto>> GetFriendsAsync(string userId);
        Task<IEnumerable<FriendshipDto>> GetFriendsRequestAsync(string userId);
        Task<bool> IsThereRequestFriendRequest(int friendId);
    }
}
