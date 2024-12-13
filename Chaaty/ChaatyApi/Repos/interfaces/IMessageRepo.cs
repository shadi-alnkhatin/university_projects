using ChaatyApi.DTOs;
using ChaatyApi.Entities;
using ChaatyApi.Helpers;
using ChaatyApi.Params;

namespace ChaatyApi.Repos.interfaces
{
    public interface IMessageRepo
    {
        void AddGroup(Group group);
        void RemoveConnection(Connection connection);
        Task<Connection> GetConnectionAsync(string connectionId);
        Task<Group> GetGroupMessageAsync(string groupName);
        void AddMessage(Message message);
        void DeleteMessage(Message message);

        Task<Message> GetMessageAsync(int id);
        Task<IEnumerable<UserDto>> GetUsersFromChatAsync(string UserId);
        Task<IEnumerable<MessageDto>> GetMessageThreadAsync(string currentUsedId,string recipentUserID);
        Task<bool> SaveAllAsync();

    }
}
