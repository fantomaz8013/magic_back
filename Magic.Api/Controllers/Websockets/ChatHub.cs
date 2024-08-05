using Magic.Common.Models.Websocket;
using Magic.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Magic.Api.Controllers.Websockets;

[Authorize]
public class ChatHub : Hub
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IUserService _userService;

    private readonly static ChatHistory chatHistory = new();

    public ChatHub(IServiceProvider serviceProvider, IUserService userService)
    {
        _serviceProvider = serviceProvider;
        _userService = userService;
    }

    public async Task NewMessage(string message)
    {
        var callerUser = await _userService.CurrentUser();
        var chatMessage = new ChatMessage(Guid.NewGuid(), callerUser.Login, message);
        chatHistory.Add(chatMessage);
        await Clients.All.SendAsync("messageReceived", chatMessage);
    }

    public override async Task OnConnectedAsync()
    {
        // var scope = _serviceProvider.CreateScope();
        // var db = scope.ServiceProvider.GetRequiredService<DataBaseContext>();

        await Clients.Caller.SendAsync("historyReceived", chatHistory.GetMessages());

        await base.OnConnectedAsync();
    }
}

public class ChatHistory
{
    private readonly List<ChatMessage> _messages = new();

    public int Count => _messages.Count;

    public void Add(ChatMessage message)
    {
        lock (_messages)
        {
            _messages.Add(message);
        }
    }

    public List<ChatMessage> GetMessages()
    {
        lock (_messages)
        {
            return _messages.ToList();
        }
    }
}