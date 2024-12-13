using AutoMapper;
using ChaatyApi.DTOs;
using ChaatyApi.Entities;
using ChaatyApi.Repos;
using ChaatyApi.Repos.interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace ChaatyApi.SignalR
{
    [Authorize]
    public class MessageHub:Hub
    {
        private readonly IMessageRepo messageRepo;
        private readonly IMapper mapper;
        private readonly IUserRepo userRepo;
        private readonly IHubContext<PHub> phubContext;
        private readonly HubTraker traker;

        public MessageHub(IMessageRepo messageRepo,IMapper mapper
            ,IUserRepo userRepo ,IHubContext<PHub> PhubContext,HubTraker traker)
        {
            this.messageRepo = messageRepo;
            this.mapper = mapper;
            this.userRepo = userRepo;
            phubContext = PhubContext;
            this.traker = traker;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                var userId = Context.User?.Identity.GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    throw new HubException("User is not authenticated.");
                }

                var httpContext = Context.GetHttpContext();
                var otherUser = httpContext.Request.Query["userId"].ToString();
                if (string.IsNullOrEmpty(otherUser))
                {
                    throw new HubException("Other user ID is not provided.");
                }

                var groupName = GetGroupName(userId, otherUser);
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                await AddGroup(groupName);
                var messages = await messageRepo.GetMessageThreadAsync(userId, otherUser);

                await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex+ "An error occurred during OnConnectedAsync.");
                throw;
            }
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await RemoveGroup();
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDto createMessage)
        {
            var userID = Context.User.Identity.GetUserId();
            var currentUser = await userRepo.GetByIdAsyncIncludigPhotos(userID);
            var recipient = await userRepo.GetByIdAsyncIncludigPhotos(createMessage.UserID);

            if (currentUser == null || recipient == null)
            {
                throw new HubException("there is a problem with the one of users");
            }
            var message = new Message
            {
                Sender = currentUser,
                Recipient = recipient,
                Content = createMessage.content,
                SenderFirstName = currentUser.FirstName,
                RecipientFirstName = recipient.FirstName
            };
            var GroupName = GetGroupName(userID,recipient.Id);
            var group =await messageRepo.GetGroupMessageAsync(GroupName);

            if(group.Connections.Any(x=>x.UserId==recipient.Id))
            {
                message.DateRead = DateTime.Now;
            }
            else
            {
                var connections = await traker.GetUserConnections(recipient.Id);
                if(connections != null)
                {
                    var firstName = currentUser.FirstName;
                    await phubContext.Clients.Clients(connections).SendAsync("NewMessageNotify",
                      firstName  );
                     
                }
            }
            messageRepo.AddMessage(message);
            if (await messageRepo.SaveAllAsync())
            {
                await Clients.Group(GroupName).SendAsync("NewMessage", mapper.Map<MessageDto>(message));
               
            }
        }

        private async Task<bool> AddGroup(string groupName)
        {
            var group=await messageRepo.GetGroupMessageAsync(groupName);
            var connection = new Connection(Context.ConnectionId,Context.User.Identity.GetUserId());

            if (group == null)
            {
                group=new Group(groupName);
                messageRepo.AddGroup(group);
            }
            group.Connections.Add(connection);
            return await messageRepo.SaveAllAsync();
        }
        private async Task RemoveGroup()
        {
            var connection = await messageRepo.GetConnectionAsync(Context.ConnectionId);
            messageRepo.RemoveConnection(connection);
            await messageRepo.SaveAllAsync();
        }
        private string GetGroupName(string userId, string otherUserId)
        {
            return string.CompareOrdinal(userId, otherUserId) < 0
                ? $"{userId}-{otherUserId}"
                : $"{otherUserId}-{userId}";
        }

    }
}
