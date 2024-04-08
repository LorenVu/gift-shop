using AutoMapper;
using GiftShop.Domain.Commons.Helpers;
using GiftShop.Domain.Entities;
using GiftShop.Infastructure.DataAccess;
using GiftShop.Infastructure.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GiftShop.Infastructure.SignalR;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class ChatHub : Hub
{
    private readonly Dictionary<string, string> connectionUsers = new Dictionary<string, string>();
    private readonly static List<UserViewModel> _connections = new List<UserViewModel>();
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ChatHub(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async override Task OnConnectedAsync()
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == HttpHelpers.UserID);

            if (user is not null)
            {
                var userViewModel = _mapper.Map<ApplicationUser, UserViewModel>(user);
                userViewModel.Device = GetDevice();
                userViewModel.CurrentRoom = string.Empty;

                if (!_connections.Any(x => x.UserName.Equals(HttpHelpers.UserName)))
                {
                    _connections.Add(userViewModel);
                    connectionUsers.Add(user.Id, Context.ConnectionId);
                }
                await Clients.Caller.SendAsync("getProfileInfo", userViewModel);
            }
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("onError", "OnConnected:" + ex.Message);

        }
    }

    public async override Task OnDisconnectedAsync(Exception? exception)
    {
        try
        {
            var user = _connections.Where(x => x.UserName == HttpHelpers.UserName).First();
            _connections.Remove(user);
            connectionUsers.Remove(HttpHelpers.UserID);

            // Tell other users to remove you from their list
            await Clients.OthersInGroup(user.CurrentRoom).SendAsync("removeUser", user);
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("onError", "OnDisconnected: " + ex.Message);
        }
    }

    public async Task GetCurrentUserInRoom(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }

    public async Task SendMessagePrivate(string message)
    {
        await Clients.User(connectionUsers[HttpHelpers.UserID]).SendAsync("ReceiveMessage", HttpHelpers.UserName, message);
    }

    public async Task SendMessage(string message)
    {
        var timestamp = DateTime.Now;
        await Clients.All.SendAsync("ReceiveMessage", HttpHelpers.UserName, new { message, timestamp });
    }

    private string GetDevice()
    {
        var device = Context.GetHttpContext().Request.Headers["Device"].ToString();
        if (!string.IsNullOrEmpty(device) && (device.Equals("Desktop") || device.Equals("Mobile")))
            return device;

        return "Web";
    }
}
