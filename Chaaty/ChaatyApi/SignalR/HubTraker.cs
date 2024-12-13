namespace ChaatyApi.SignalR
{
    public class HubTraker
    {
        private readonly Dictionary<string,List<string>> _onlineUsers = new Dictionary<string,List<string>>();

        public Task AddConnctedUser(string id , string connectionId)
        {
            lock(_onlineUsers) 
            {
                if (_onlineUsers.ContainsKey(id))
                {
                    _onlineUsers[id].Add(connectionId);
                }
                else
                {
                    _onlineUsers.Add(id, new List<string>() { connectionId});
                }
            }
            return Task.CompletedTask;
        }
        public Task RemoveDisconnectedUser(string id,string connectionId) 
        {
            lock(_onlineUsers)
            {
                if (!_onlineUsers.ContainsKey(id))
                {
                    return Task.CompletedTask;
                }

                _onlineUsers[id].Remove(connectionId);

                if (_onlineUsers[id].Count == 0)
                {
                    _onlineUsers.Remove(id);
                }
                
            }
            return Task.CompletedTask;
        }
        public Task<string[]> GetOnlineUsers()
        {
            string[] users;
            lock(_onlineUsers)
            {
                users = _onlineUsers.Select(t=>t.Key).ToArray();
            }

            return Task.FromResult(users);
        }

        public Task<List<string>> GetUserConnections(string userId)
        {
            List<string> connections;
            lock(_onlineUsers)
            {
                connections=_onlineUsers.GetValueOrDefault(userId);
                
            }   
            return Task.FromResult(connections);    
        }
    }
}
