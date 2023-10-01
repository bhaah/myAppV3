using Microsoft.AspNetCore.Mvc;
using WebApplication2.LogicLayer;
using WebApplication2.Singletons;
using WebApplication2.LogicLayer.BoardFolder;
using System.Text.Json;
using WebApplication2.DatabaseLayer;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication2.Controllers
{


    public class UserModel
    {
        private string _email;
        private string _password;
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // GET: api/<HomeController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        //----- private funs ----

        private BoardLogic checkUser(string email,string password)
        {
            if( !Users.UserLogic.checkUser(email,password)) 
            throw new Exception("the given user wrong");
            return BoardsOfUser.getUserBoards(email);

        }

        //-----------------------

        // GET api/<HomeController>/5
        [HttpGet("Boards")]
        public IActionResult Get([FromQuery] string email, [FromQuery] string password)
        {
            Response res;
            if (Users.UserLogic.checkUser(email,password))
            {
                try
                {

                    BoardLogic board = BoardsOfUser.getUserBoards(email);
                    res = new Response(board);

                }
                catch (Exception ex)
                {
                    res = new Response(ex.Message,null);
                }
                return Ok(JsonSerializer.Serialize(res));
            }
            else
            {
                res = new Response("the given user is wrong");
                return Ok(JsonSerializer.Serialize(res));
            }
            
        }



        //--------- BOARDS MANGMENT ---------


        // POST api/<HomeController>
        [HttpPost("creatBoard")]
        public void Post([FromForm] string email, [FromForm] string password,[FromForm]string nameOfBoard,[FromForm]int BoardId)
        {
            Response res;
            try
            {
                BoardLogic boards = checkUser(email, password);
                boards.creatBoard(nameOfBoard);
                //BoardsOfUser.setBoardLoic(boards, email);
                res= new Response();
                Console.WriteLine(res.ToString());
            }
            catch(Exception ex)
            {
                res = new Response(ex.Message,null);
            }
            Console.WriteLine(JsonSerializer.Serialize(res));
            
        }

        [HttpPost("getInBoard")]
        public void Post([FromForm] string email,[FromForm]string password,[FromForm]int BoardId)
        {
            Response res;
            try
            {
                BoardLogic boards = checkUser(email, password);
                boards.getInBoard(BoardId);
                res= new Response();
                
            }
            catch(Exception ex)
            {
                res = new Response(ex.Message,null);
            }
            Console.WriteLine(JsonSerializer.Serialize(res));
        }

        [HttpPost("getOutFromBoard")]
        public void Post([FromForm] string email, [FromForm] string passowd)
        {
            Response res;
            try
            {
                BoardLogic board = checkUser(email,passowd);
                board.getOutBoard();
                res= new Response();
            }
            catch(Exception ex)
            {
                res = new Response(ex.Message, null);
            }
            Console.WriteLine(JsonSerializer.Serialize(res));
        }

        //------------- CORNERS

        [HttpGet("getCorners")]
        public IActionResult Get([FromQuery] UserModel user)
        {
            Response res;
            
            try
            {
                BoardLogic bl = checkUser(user.Email,user.Password);
                Dictionary<int, CornerOfTasks> corners = bl.getCorners();
                List<CornerOfTasks> result = new List<CornerOfTasks>();
                foreach(CornerOfTasks corner in corners.Values) 
                {
                    result.Add(corner);
                    Console.WriteLine($"{corner.Name} with id :{corner.ID}");
                }
                res = new Response(result.ToArray());
                Console.WriteLine(JsonSerializer.Serialize(res));
                
            }
            catch(Exception ex)
            {
                res = new Response(ex.Message, null);
            }
            return Ok(JsonSerializer.Serialize(res));
        }



        [HttpPost("creatCorner")]
        public IActionResult Post([FromForm] string email, [FromForm] string password, [FromForm] int corId, [FromForm] string name, [FromForm] string desc)
        {
            Response res;
            try
            {
                BoardLogic bl = checkUser(email,password);
                bl.creatCorner(corId, name, desc);
                res= new Response();
                Console.WriteLine("corner : "+name+" ,added!");
            }
            catch(Exception ex)
            {
                res = new Response("error at adding corner:\n"+ex.Message,null);
            }
            return Ok(JsonSerializer.Serialize(res));
        }

        [HttpPost("deleteCorner")]
        public IActionResult PostDeleteCorner([FromForm] string email, [FromForm] string password, [FromForm] int corId)
        {
            Response res;
            try
            {
                BoardLogic bl = checkUser(email,password);
                bl.deleteCorner(corId);
                res= new Response();
                Console.WriteLine("corner -" + corId + " deleted");
            }
            catch(Exception ex)
            {
                res = new Response("error with deleting corner.\n" + ex.Message, null);
            }
            return Ok(JsonSerializer.Serialize(res));
        }


        [HttpPost("getCornerProgress")]
        public IActionResult PostProgress([FromForm] string email,[FromForm] string password, [FromForm] int id)
        {
            Response res;
            try
            {
                BoardLogic bl = checkUser(email,password);
                int a =bl.getCornerProgress(id);
                res= new Response(a);
                
            }
            catch(Exception ex)
            {
                res = new Response("error in geting progress.\n"+ex.Message);
            }
            return Ok(JsonSerializer.Serialize(res));
        }

        [HttpPost("getTasksInCorner")]
        public IActionResult PostGetTasksInCorner([FromForm] string email, [FromForm] string password, [FromForm] int id)
        {
            Console.WriteLine("sdsa");
            Response res;
            try
            {
                BoardLogic bl = checkUser(email,password);
                var result = bl.getCornerTasks(id);
                res= new Response(result.ToArray());
            }
            catch(Exception ex)
            {
                res = new Response("error in getting tasks.\n" + ex.Message,null);
            }
            return Ok(JsonSerializer.Serialize(res));
        }



        //--------- TASK MANGMENT ------

        [HttpPost("creatTask")]
        public IActionResult PostTask([FromForm] string email,[FromForm] string password,[FromForm] int corId, [FromForm] int taskId, [FromForm] string taskName, [FromForm] int id, [FromForm] string desc, [FromForm] DateTime dateTime)
        {
            Response res;
            try
            {
                BoardLogic bl = checkUser(email, password);
                bl.creatTask(corId,taskId,taskName,desc,dateTime);
                res = new Response();
            }
            catch(Exception ex)
            {
                res = new Response("error in creating task \n" + ex.Message, null);
            }
            return Ok(JsonSerializer.Serialize(res));
        }

        [HttpPost("moveTask")]
        public IActionResult PostMoveTask([FromForm] string email, [FromForm]string password, [FromForm] int corId, [FromForm] int taskId)
        {
            Response res;
            try
            {
                BoardLogic bl = checkUser(email, password);
                bl.moveTask(corId,taskId);
                res = new Response();
            }
            catch(Exception ex)
            {
                res = new Response("error in moving task.\n" + ex.Message, null);
            }
            return Ok(JsonSerializer.Serialize(res));
        }

        [HttpPost("UpdateName")] 
        public IActionResult PostEditTaskName([FromForm] string email, [FromForm] string password, [FromForm] int corId, [FromForm] int taskIad, [FromForm] int status, [FromForm] string newName)
        {
            Response res;
            try
            {
                BoardLogic bl =checkUser(email, password);
                bl.editTaskName(corId,taskIad,status,newName);
                res = new Response();
            }
            catch(Exception ex)
            {
                res = new Response("error in editing the name of task.\n" + ex.Message, null);

            }
            return Ok(JsonSerializer.Serialize(res));
        }

        [HttpPost("UpdateDesc")]
        public IActionResult PostEditTaskDesc([FromForm] string email, [FromForm] string password, [FromForm] int corId, [FromForm] int taskIad, [FromForm] int status, [FromForm] string desc)
        {
            Response res;
            try
            {
                BoardLogic bl = checkUser(email, password);
                bl.editTaskDesc(corId, taskIad, status, desc);
                res = new Response();
            }
            catch (Exception ex)
            {
                res = new Response("error in editing the desc of task.\n" + ex.Message, null);

            }
            return Ok(JsonSerializer.Serialize(res));
        }

        [HttpPost("UpdateDeadline")]
        public IActionResult PostEditTaskDeadline([FromForm] string email, [FromForm] string password, [FromForm] int corId, [FromForm] int taskIad, [FromForm] int status, [FromForm] DateTime time)
        {
            Response res;
            try
            {
                BoardLogic bl = checkUser(email, password);
                bl.editTaskDeadline(corId, taskIad, status, time);
                res = new Response();
            }
            catch (Exception ex)
            {
                res = new Response("error in editing the deadline time of task.\n" + ex.Message, null);

            }
            return Ok(JsonSerializer.Serialize(res));
        }

        [HttpPost("UpdateStartTime")]
        public IActionResult PostEditTaskStartTime([FromForm] string email, [FromForm] string password, [FromForm] int corId, [FromForm] int taskIad,[FromForm] DateTime time ,[FromForm] int status )
        {
            Response res;
            try
            {
                BoardLogic bl = checkUser(email, password);
                bl.editTaskStartTime(corId, taskIad, status, time);
                res = new Response();
            }
            catch (Exception ex)
            {
                res = new Response("error in editing the start time of task.\n" + ex.Message, null);

            }
            return Ok(JsonSerializer.Serialize(res));
        }

        //---------NOTES MANGMENT-----------

        [HttpPost("getNotesOfBoard")]
        public IActionResult PostNotes([FromForm] string email, [FromForm] string password)
        {
            Response res;
            try
            {
                BoardLogic bl = checkUser(email, password);
                Note[] result = bl.getAllNotes();
                res = new Response(result);
            }
            catch (Exception ex)
            {
                res = new Response("error in geting all the notes of board.\n"+ex.Message);
            }
            return Ok(JsonSerializer.Serialize(res));
        }

        [HttpPost("addNote")]
        public IActionResult PostAddNote([FromForm] string email, [FromForm]string password, [FromForm] int id, [FromForm]string content)
        {
            Response res;
            try
            {
                BoardLogic bl = checkUser(email,password);
                bl.addNote(id,content);
                res = new Response();

            }
            catch (Exception ex)
            {
                res = new Response("error at adding note.\n" + ex.Message);
            }
            return Ok(JsonSerializer.Serialize(res));
        }




        // ----- mangment 


        [HttpGet("deleteAllData")]
        public IActionResult GetClean() 
        {
            DBF.cleanData("Tasks");
            DBF.cleanData("CornerTable");
            DBF.cleanData("Board");
            DBF.cleanData("Users");
            return Ok();
        }

        // PUT api/<HomeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<HomeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
