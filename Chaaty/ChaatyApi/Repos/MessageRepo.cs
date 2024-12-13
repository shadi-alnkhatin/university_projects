using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChaatyApi.Data;
using ChaatyApi.DTOs;
using ChaatyApi.Entities;
using ChaatyApi.Helpers;
using ChaatyApi.Params;
using ChaatyApi.Repos.interfaces;
using Microsoft.EntityFrameworkCore;

namespace ChaatyApi.Repos
{
    public class MessageRepo : IMessageRepo
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IUserRepo userRepo;

        public MessageRepo(ApplicationDbContext context , IMapper mapper,IUserRepo userRepo)
        {
            this.context = context;
            this.mapper = mapper;
            this.userRepo = userRepo;
        }

        public void AddGroup(Group group)
        {
            context.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            context.Messages.Remove(message);
        }

        public async Task<Connection> GetConnectionAsync(string connectionId)
        {
           return await context.Connections.FindAsync(connectionId);
        }

        public async Task<Group> GetGroupMessageAsync(string groupName)
        {
            return await context.Groups.Include(x => x.Connections).
                FirstOrDefaultAsync(x=>x.Name==groupName);
        }

        public async Task<Message> GetMessageAsync(int id)
        {
          return await context.Messages.FindAsync(id);
        }

        public async Task<IEnumerable<UserDto>> GetUsersFromChatAsync(string UserID)
        {
            var query =await context.Messages
                  .OrderByDescending(m => m.MessageSent)
                  .Where(u => (u.RecipientId == UserID) || 
                  (u.SenderId == UserID))
                  .ToListAsync();
            var UsersId = new List<string>();
            var Users = new List<UserDto>();
            foreach (var q in query)
            {
                if(q.RecipientId == UserID) 
                {
                    if (!UsersId.Contains(q.SenderId))
                    {
                        UsersId.Add(q.SenderId);
                    }
                }
                else
                {
                    if (!UsersId.Contains(q.RecipientId))
                       { UsersId.Add(q.RecipientId); }
                }
            }
            foreach (var id in UsersId)
            {
               var user= await userRepo.GetByIdAsyncIncludigPhotos2(id);
                Users.Add(user);
            }

            return Users;
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThreadAsync(string currentUsedId, string recipentUserID)
        {
            var messages = await context.Messages
                .Where(m => m.Recipient.Id == currentUsedId && m.RecipientDeleted == false
                        && m.Sender.Id == recipentUserID
                        || m.Recipient.Id == recipentUserID
                        && m.Sender.Id == currentUsedId && m.SenderDeleted == false
                )
                .OrderBy(m => m.MessageSent)
                .ToListAsync();

            var unreadMessages = messages.Where(d => d.DateRead == null &&
                                                d.RecipientId == currentUsedId)
                                                 .ToList();

            if(unreadMessages.Any())
            {
                foreach(var message in unreadMessages)
                {
                    message.DateRead = DateTime.Now;
                }
                await context.SaveChangesAsync();
            }

            return mapper.Map<IEnumerable<MessageDto>>(messages);
        }

        public  void RemoveConnection(Connection connection)
        {
            context.Connections.Remove(connection);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}


