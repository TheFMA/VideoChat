using Microsoft.AspNetCore.SignalR;
using Video_Chat.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Microsoft.AspNetCore.Cors;

namespace Video_Chat.Hubs
{
    public interface IChatClient
    {
        public Task ReceiveMessage(string userName, string message);
    }
    public class ChatHub : Hub<IChatClient>
    {
        private readonly IDistributedCache _cache;

        public ChatHub(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task JoinChat(UserConnection connection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, connection.chatRoom);

            var stringConnection = JsonSerializer.Serialize(connection);

            await _cache.SetStringAsync(Context.ConnectionId, stringConnection);

            await Clients
                .Group(connection.chatRoom)
                .ReceiveMessage("Admin", $"{connection.userName} присоединился к чату");
        }
            //*"applicationUrl": "https://localhost:7212;http://localhost:5145",*/

        public async Task SendMessage(string message)
        {
            var stringConnection = await _cache.GetAsync(Context.ConnectionId);

            var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

            if (connection != null)
            {
                await Clients
               .Group(connection.chatRoom)
               .ReceiveMessage(connection.userName, message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var stringConnection = await _cache.GetAsync(Context.ConnectionId);
            var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

            if (connection != null)
            {
                await _cache.RemoveAsync(Context.ConnectionId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.chatRoom);

                await Clients
                .Group(connection.chatRoom)
                .ReceiveMessage("Admin", $"{connection.userName} вышел из чата");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}


/*using Microsoft.AspNetCore.SignalR;
using ChatReact.Models;
using System;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Specialized;

namespace ChatReact.Hubs
{
    public interface IChatClient
    {
        Task ReceiveMessage(string userName, string message);
    }

    public class ChatHub : Hub<IChatClient>
    {
        //public readonly ConcurrentDictionary<string, UserConnection> _connections = new ConcurrentDictionary<string, UserConnection>();
        //public UserConnection connect;
        public UserConnection connect;
        public async Task JoinChat(UserConnection connection)
        {
            //_connections.TryAdd(Context.ConnectionId, connection);
            connect = (UserConnection)connection.Clone();

            await Groups.AddToGroupAsync(Context.ConnectionId, connection.chatRoom);

            await Clients.Group(connection.chatRoom).ReceiveMessage("Admin", $"{connection.userName} присоединился к чату");
        }

        public async Task SendMessage(string message)
        {
            *//* if (_connections.TryGetValue(Context.ConnectionId, out var connection))
             {
                 await Clients.Group(connection.chatRoom).ReceiveMessage(connection.userName, message);
             }*//*
            await Clients.Group( connect.chatRoom).ReceiveMessage(connect.userName, message);
        }

        *//*public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (_connections.TryRemove(Context.ConnectionId, out var disconnectedUser))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, disconnectedUser.chatRoom);
                await Clients.Group(disconnectedUser.chatRoom).ReceiveMessage("Admin", $"{disconnectedUser.userName} вышел из чата");
            }

            await base.OnDisconnectedAsync(exception);
        }*//*
    }
}*/
