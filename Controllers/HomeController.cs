using Microsoft.AspNetCore.Mvc;
using WebApplication2.LogicLayer;
using WebApplication2.Singletons;
using WebApplication2.LogicLayer.BoardFolder;
using System.Text.Json;
using WebApplication2.DatabaseLayer;
using myFirstAppSol.LogicLayer;
using Microsoft.AspNetCore.Cors;
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
        [HttpPost("Boards")]
        public IActionResult Get([FromForm] string email, [FromForm] string password)
        {
            Response res;
            if (Users.UserLogic.checkUser(email,password))
            {
                try
                {

                    BoardLogic board = BoardsOfUser.getUserBoards(email);
                    Board[] boards = board.Boards.Values.ToList().ToArray();
                    res = new Response(boards);

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
        public IActionResult Post([FromForm] string email, [FromForm] string password,[FromForm]string nameOfBoard,[FromForm]int BoardId)
        {
            Response res;
            try
            {
                BoardLogic boards = checkUser(email, password);
                
                //BoardsOfUser.setBoardLoic(boards, email);
                res= new Response(boards.creatBoard(nameOfBoard));
                Console.WriteLine(res.ToString());
            }
            catch(Exception ex)
            {
                res = new Response(ex.Message,null);
            }
            Console.WriteLine(JsonSerializer.Serialize(res));
            return Ok(JsonSerializer.Serialize(res));
            
        }

        [HttpPost("getInBoard")]
        public IActionResult Post([FromForm] string email,[FromForm]string password,[FromForm]int BoardId)
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
            return Ok(JsonSerializer.Serialize(res));
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

        [HttpPost("deleteBoard")]
        public IActionResult deleteBoardPost([FromForm] string email, [FromForm] string passowd, [FromForm] int boardId)
        {
            Response res;
            try{
                BoardLogic bl = checkUser(email, passowd);
                bl.deleteBoard(boardId);
                res= new Response();
            }
            catch(Exception ex) 
            {
                res = new Response(ex.Message, null); 
            }
            return Ok(JsonSerializer.Serialize(res));
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
               
                res = new Response(bl.creatTask(corId, taskId, taskName, desc, dateTime));
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
        public IActionResult PostEditTaskName([FromForm] string email, [FromForm] string password, [FromForm] int corId, [FromForm] int taskId, [FromForm] int status, [FromForm] string newName)
        {
            Response res;
            try
            {
                BoardLogic bl =checkUser(email, password);
                bl.editTaskName(corId,taskId,status,newName);
                res = new Response();
            }
            catch(Exception ex)
            {
                res = new Response("error in editing the name of task.\n" + ex.Message, null);

            }
            return Ok(JsonSerializer.Serialize(res));
        }

        [HttpPost("UpdateDesc")]
        public IActionResult PostEditTaskDesc([FromForm] string email, [FromForm] string password, [FromForm] int corId, [FromForm] int taskId, [FromForm] int status, [FromForm] string desc)
        {
            Response res;
            try
            {
                BoardLogic bl = checkUser(email, password);
                bl.editTaskDesc(corId, taskId, status, desc);
                res = new Response();
            }
            catch (Exception ex)
            {
                res = new Response("error in editing the desc of task.\n" + ex.Message, null);

            }
            return Ok(JsonSerializer.Serialize(res));
        }

        [HttpPost("UpdateDeadline")]
        public IActionResult PostEditTaskDeadline([FromForm] string email, [FromForm] string password, [FromForm] int corId, [FromForm] int taskId, [FromForm] int status, [FromForm] DateTime time)
        {
            Response res;
            try
            {
                BoardLogic bl = checkUser(email, password);
                bl.editTaskDeadline(corId, taskId, status, time);
                res = new Response();
            }
            catch (Exception ex)
            {
                res = new Response("error in editing the deadline time of task.\n" + ex.Message, null);

            }
            return Ok(JsonSerializer.Serialize(res));
        }

        [HttpPost("UpdateStartTime")]
        public IActionResult PostEditTaskStartTime([FromForm] string email, [FromForm] string password, [FromForm] int corId, [FromForm] int taskId,[FromForm] DateTime time ,[FromForm] int status )
        {
            Response res;
            try
            {
                BoardLogic bl = checkUser(email, password);
                bl.editTaskStartTime(corId, taskId, status, time);
                res = new Response();
            }
            catch (Exception ex)
            {
                res = new Response("error in editing the start time of task.\n" + ex.Message, null);

            }
            return Ok(JsonSerializer.Serialize(res));
        }

        [HttpPost("deleteTask")]
        public IActionResult deleteTaskPost([FromForm] string email, [FromForm] string password, [FromForm] int boardId, [FromForm] int corId, [FromForm] int taskId)
        {
            Response res;
            try
            {
                BoardLogic bl = checkUser(email, password);
                bl.deleteTask(boardId, corId, taskId);
                res = new Response();
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
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

        [HttpPost("deleteNote")]
        public IActionResult deleteNotePost([FromForm] string email, [FromForm] string password, [FromForm] int noteId)
        {
            Response res;
            try{
                BoardLogic bl = checkUser(email,password);
                bl.removeNote(noteId);
                res = new Response();
            }
            catch(Exception ex) { res = new Response(ex.Message, null); }
            return Ok(JsonSerializer.Serialize(res));
        }
        [HttpPost("editNote")]
        public IActionResult editNotPost([FromForm] string email, [FromForm] string password, [FromForm] int noteId, [FromForm] string noteToChange)
        {
            Response res;
            try
            {
                BoardLogic bl = checkUser(email,password);
                bl.editNote(noteId,noteToChange);
                res = new Response();
            }
            catch(Exception ex)
            {
                res = new Response(ex.Message, null);
            }
            return Ok(JsonSerializer.Serialize(res));
        }
        //===============================messages 




        [HttpPost("addMessage")]
        public IActionResult addMessage([FromForm] string email, [FromForm] string password, [FromForm] string content, [FromForm] DateTime time)
        {
            Response res;
            try
            {
                BoardLogic bl = checkUser(email,password);
                if(bl != null)
                {
                    MessageLogic ml = new MessageLogic();
                    ml.setEmailMessage(content,email,time);
                }
                res = new Response();
            }
            catch(Exception ex)
            {
                res = new Response(ex.Message, null);
            }
            return Ok(JsonSerializer.Serialize(res));
        }

        [HttpPost("addRandomMessage")]
        public IActionResult addRMessage([FromForm] string content)
        {
            Response res;
            try
            {
                MessageLogic ml = new MessageLogic();
                ml.setRandomMessage(content);
                res = new Response();
            }
            catch(Exception ex)
            {
                res = new Response(ex.Message,null);
            }
            return Ok(JsonSerializer.Serialize(res));
        }
        [HttpPost("getEmailMessages")]
        public IActionResult getEmailMessages([FromForm] string email, [FromForm]string password)
        {
            Response res;
            try
            {
                Message[] ms;
                if (checkUser(email, password) == null) { throw new Exception(""); }
                    MessageLogic ml = new MessageLogic();
                    ms = ml.getMessagesForEmail(email);
                    res = new Response(ms);
                
                
            }
            catch(Exception ex)
            {
                res = new Response(ex.Message, null);
            }
            return Ok(JsonSerializer.Serialize(res));
        }

        // ----- mangment 


        [HttpGet("deleteAllData")]
        public IActionResult GetClean() 
        {
           
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
