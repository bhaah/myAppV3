using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApplication2.LogicLayer;
using WebApplication2.Singletons;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //private UserLogic UM = new UserLogic();

        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            List<string> list = new List<string>();
            foreach(User user in Users.UserLogic.Users)
            {
                list.Add(user.UserName);
            }
            return list.ToArray();
        }



        // POST api/<UserController>
        [HttpPost("register")]
        public IActionResult Post([FromForm] string email, [FromForm] string userName, [FromForm] string password)
        {
            Response res;
            try
            {
                var fun = Users.UserLogic.Register(email, userName, password);
                UserAccount.Account= fun;
                res = new Response(fun);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message,null);
            }
            return Ok(JsonSerializer.Serialize(res));
        }
        [HttpPost("login")]
        public IActionResult Post([FromForm] string email, [FromForm] string password)
        {
            Response res;
            try
            {
                var func = Users.UserLogic.login(email, password);
                UserAccount.Account= func;
                res = new Response(func);
            }
            catch (Exception ex)
            {
                res = new Response(ex);
            }
            return Ok(JsonSerializer.Serialize(res));
        }

        [HttpPost("logout")]
        public IActionResult PostLogout([FromForm] string email)
        {
            Response res;
            try
            {
               
                UserAccount.Account = null;
                res = new Response();
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message,null);
            }
            return Ok(JsonSerializer.Serialize(res));
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
