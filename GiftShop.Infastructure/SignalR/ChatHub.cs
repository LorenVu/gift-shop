using GiftShop.Infastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace GiftShop.Infastructure.SignalR;

[Authorize]
public class ChatHub : Hub
{
    public Dictionary<string, string> ConnectionUser = new Dictionary<string, string>();
    public readonly static List<UserViewModel> _Connections = new List<UserViewModel>();

    public override Task OnConnectedAsync()
    {


        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception); 
    }
}
