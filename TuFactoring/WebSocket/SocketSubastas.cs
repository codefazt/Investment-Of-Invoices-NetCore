using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuFactoringModels;
using TuFactoring.Services;
using Microsoft.AspNetCore.Http;

namespace TuFactoring.WebSocket
{
    public class SocketSubastas : Hub
    {
        private readonly IAuctionService _aS;
        private readonly SignInManager<User> _signInManager;

        public SocketSubastas(IAuctionService aS, SignInManager<User> signInManager)
        {
            this._aS = aS;
            this._signInManager = signInManager;
        }

        public async Task<Task> Publicar([FromBody]Offert oferta)
        {
            
            Publications resultado = new Publications() { Error = new Exception("Invalid offert") };

            resultado = await this._aS.OfferPublication(oferta, oferta.Token);

            if(resultado.Errors != null)
            {
                return Clients.Caller.SendAsync("Publications", resultado);
            }

            if (resultado.Error != null)
            {
                return Clients.Caller.SendAsync("Publications", resultado);
            }

            resultado.Bids = new List<Bids>() { new Bids() { Factor = new People() { Id = oferta.Factor_id } } };

            if (oferta.Factor_id == resultado.Bids[0].Factor_id) 
            {
                resultado.isOffered = true;
            }
            

            return Clients.Group(resultado.Invoice.Country.Value + "").SendAsync("Publications", resultado);
        }

        public async Task<Task> Cierre(string identificador)
        {
            return Clients.Group(identificador).SendAsync("Cierre","close");
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public async void loginGroup(string group)
        {
             if (String.IsNullOrEmpty(group)) return;

            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }
    }
}
