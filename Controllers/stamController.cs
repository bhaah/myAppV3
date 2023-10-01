using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

public class HiService
{
    public bool Hi = false;
}
public class _hiService
{
    public static HiService hi= new HiService();
    public static bool Hi
    {
        get { return hi.Hi; }
        set { hi.Hi = value; }
    }
}

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class stamController : ControllerBase
    {

        //private readonly HiService _hiService;

        public stamController()
        {
            
            Console.WriteLine("CONS OF CONT");
        }

        // GET: api/<stamController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            Console.WriteLine(_hiService.Hi);
            _hiService.Hi = true;
            Console.WriteLine(_hiService.Hi + " this after changing");
            return new string[] { "hi from api" };
        }

        // GET api/<stamController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            TaskObject t = new TaskObject("sdf", "sdf");
            Response r= new Response(t);
            return JsonSerializer.Serialize(_hiService.Hi);
        }

        // POST api/<stamController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<stamController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<stamController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
