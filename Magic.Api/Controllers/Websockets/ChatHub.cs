using Magic.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Magic.Api.Controllers.Websockets;

[Authorize]
public class ChatHub : Hub
{
    private readonly IServiceProvider _serviceProvider;

    public ChatHub(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task NewMessage(string username, string message) =>
        await Clients.All.SendAsync("messageReceived", username, message);

    public override async Task OnConnectedAsync()
    {
        var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DataBaseContext>();

        var messages = await db.User.ToListAsync();

        foreach (var message in messages)
        {
            await Clients.Caller.SendAsync("messageReceived", message.Login,
                "Привет, я тут оставлял сообщения, мне похуй");
        }

        await base.OnConnectedAsync();
    }
}