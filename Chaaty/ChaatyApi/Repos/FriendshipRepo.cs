using AutoMapper.QueryableExtensions;
using AutoMapper;
using ChaatyApi.Data;
using ChaatyApi.DTOs;
using ChaatyApi.Entities;
using ChaatyApi.Repos.interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChaatyApi.Repos
{
    public class FriendshipRepo : IFriendshipRepo
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public FriendshipRepo(ApplicationDbContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public async Task<bool> IsThereRequestFriendRequest(int friendId)
        {
            var isThereRequest = await context.Friendships.AnyAsync(i => i.Id == friendId);
            return isThereRequest;
        }

        public async Task SendFriendRequestAsync(string sender, string receiver)
        { 
            var isThereRequestBefore = await context.Friendships
            .AnyAsync(x => x.SenderUserId == sender && x.ReceiverUserId == receiver ||
            x.SenderUserId == receiver && x.ReceiverUserId == sender);

            if (isThereRequestBefore)
            {
                throw new InvalidOperationException("You cannot send two requests to the same user.");
            }
            var friendship = new FriendShip
            {
                SenderUserId = sender,
                ReceiverUserId=receiver,
                FriendshipStatus="Pending"
            };
            context.Friendships.Add(friendship);
            await context.SaveChangesAsync();
        }

        public async Task RejectFriendRequestAsync(int friendshipId)
        {
            var friendship = await context.Friendships.FindAsync(friendshipId);
            if (friendship != null)
            {
                context.Friendships.Remove(friendship);
                await context.SaveChangesAsync();
            }

        }

        public async Task AcceptFriendRequestAsync(int friendshipId)
        {
            var friendship = await context.Friendships.FindAsync(friendshipId);
            if(friendship!=null)
            {
                friendship.FriendshipStatus = "Accepted";
                context.Friendships.Update(friendship);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<FriendshipDto>> GetFriendsRequestAsync(string userId)
        {
            var FriendsRequest= await  context.Friendships
                .Include(i=>i.Sender)
                .ThenInclude(i=>i.Photos)
                .Where(i => i.ReceiverUserId == userId && i.FriendshipStatus=="Pending")
                .ToListAsync();

            return mapper.Map<IEnumerable<FriendshipDto>>(FriendsRequest);
        }  

        public async Task<IEnumerable<FriendshipDto>> GetFriendsAsync(string userId)
        {
            var userFriends = await context.Friendships
            .Where(f => (f.SenderUserId == userId || f.ReceiverUserId == userId) &&
                 f.FriendshipStatus == "Accepted")
            .Select(f => new FriendshipDto
             {
                FriendshipId=f.Id,
                 Id = f.SenderUserId == userId ? f.Receiver.Id : f.Sender.Id,
                 FirstName = f.SenderUserId == userId ? f.Receiver.FirstName : f.Sender.FirstName,
                 LastName = f.SenderUserId == userId ? f.Receiver.LastName : f.Sender.LastName,
                 PhotoUrl = f.SenderUserId == userId
                     ? f.Receiver.Photos.FirstOrDefault(p => p.IsMain).Url
                     : f.Sender.Photos.FirstOrDefault(p => p.IsMain).Url,
             })
            .ToListAsync();
            return userFriends;
        }

    }
}
