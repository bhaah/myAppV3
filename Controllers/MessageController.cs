using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace myFirstAppSol.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public MessageController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> PostMessage([FromBody] string message)
        {
            // Process the message and then send it to the clients
            await _hubContext.Clients.All.SendAsync("ReceiveMessage",  message);

            return Ok();
        }
    }
}
