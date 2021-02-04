using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuFactoringModels;
using TuFactoring.Services;

namespace TuFactoring.WebSocket
{
    public class SocketUser : Hub
    {
        private readonly IAuctionService _aS;
        private readonly SignInManager<User> _signInManager;

        public SocketUser(IAuctionService aS, SignInManager<User> signInManager)
        {
            this._aS = aS; 
            this._signInManager = signInManager;
        }

        public async Task<Task> Message(MessageUser message)
        {
            if (message == null || message.Title == "" || message.Body == "" || message.Id == "") return Clients.Caller.SendAsync("Message", "Mensaje Invalido");

            return Clients.Group(message.Id).SendAsync("Message", message);
        }


        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "1234");
            await base.OnConnectedAsync();
        }

        public async void loginGroup(string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }

    }
}
