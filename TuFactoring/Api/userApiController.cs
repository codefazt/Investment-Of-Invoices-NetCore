using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TuFactoringModels;
using TuFactoring.WebSocket;

namespace TuFactoring.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class userApiController : ControllerBase
    {
        private readonly IHubContext<SocketUser> _sk;

        public userApiController(IHubContext<SocketUser> sk)
        {
            _sk = sk;
        }

        // POST: api/UserApi
        [HttpPost]
        public JsonResult Post([FromBody] MessageUser msg)
        {
            try
            {
                enviarMensaje(msg);
            }
            catch (Exception)
            {
                return new JsonResult(0);
            }

            return new JsonResult(1);
        }


        private async void enviarMensaje(MessageUser msg)
        {
            message mensaje = new message()
            {
                body = msg.Body,
                title = msg.Title
            };

            await _sk.Clients.Groups(msg.Id).SendAsync("Message", mensaje);

        }

        private partial class message
        {
            public string body { get; set; }
            public string title { get; set; }
        }
    }
}