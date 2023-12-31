﻿using Microsoft.AspNetCore.Mvc;
using WebApplication2.LogicLayer;
using WebApplication2.Singletons;
using WebApplication2.LogicLayer.BoardFolder;
using System.Text.Json;
using WebApplication2.DatabaseLayer;
using myFirstAppSol.LogicLayer;
using Microsoft.AspNetCore.Cors;
using myFirstAppSol;
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


        [HttpPost("connect")]
        public IActionResult postConnect([FromForm]string email, [FromForm]string password)
        {
            
            ChatHub c = new ChatHub();
            c.listen(email);
            return Ok();
        }



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

        //-------------------CALENDAR--------------------

        [HttpPost("calendar")]
        public IActionResult postCalendar([FromForm] string email, [FromForm]string password)
        {
            Response res;
            try
            {
                var boards = checkUser(email, password);
                res = new Response(boards.Calendar.ToArray());
            }
            catch(Exception ex)
            {
                res = new Response(ex.Message, null);
            }
            return Ok(JsonSerializer.Serialize(res));
            
        }


        [HttpPost("deleteCalendarTask")]
        public IActionResult postDelete([FromForm] string email, [FromForm]string password, [FromForm] int boardId, [FromForm]int corId, [FromForm] int taskId)
        {
            Response res;
            try
            {
                BoardLogic bl =checkUser(email, password);
                res = new Response(bl.deleteFromAvl(boardId,corId,taskId));
            }
            catch(Exception ex)
            {
                res = new Response(ex.Message);
            }
            return Ok(JsonSerializer.Serialize(res));
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
                
                Dictionary<int, CornerOfTasks> corners = boards.getInBoard(BoardId); ;
                List<CornerOfTasks> result = new List<CornerOfTasks>();
                foreach (CornerOfTasks corner in corners.Values)
                {
                    result.Add(corner);
                    Console.WriteLine($"{corner.Name} with id :{corner.ID}");
                }
                res = new Response(result.ToArray());
                
                
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

        [HttpPost("getCorners")]
        public IActionResult postcor([FromForm] string email, [FromForm] string password)
        {
            Response res;
            
            try
            {
                BoardLogic bl = checkUser(email,password);
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

        [HttpPost("refreshTasks")]
        public IActionResult PostRefresh([FromForm] string email, [FromForm]string password)
        {
            Response res;
            Console.WriteLine("hi we are there");
            try
            {
                var b = checkUser(email, password);
                b.checkTasksToMove();
                res = new Response();
            }
            catch(Exception e)
            {
                res = new Response(e.Message);
            }
            return Ok(JsonSerializer.Serialize(res));
        }



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

        [HttpPost("moveTaskFromCalendar")]
        public IActionResult PostMoveTask([FromForm] string email, [FromForm] string password, [FromForm]int boardId,[FromForm] int corId, [FromForm] int taskId)
        {
            Response res;
            try
            {
                BoardLogic bl = checkUser(email, password);
                bl.moveTaskFromOut(boardId,corId, taskId);
                res = new Response();
            }
            catch (Exception ex)
            {
                res = new Response("error in moving task.\n" + ex.Message, null);
            }
            return Ok(JsonSerializer.Serialize(res));
        }

        [HttpPost("UpdateName")] 
        public IActionResult PostEditTaskName([FromForm] string email, [FromForm] string password, [FromForm] int corId, [FromForm] int taskId, [FromForm] int status, [FromForm] string Name)
        {
            Response res;
            try
            {
                BoardLogic bl =checkUser(email, password); 
                bl.editTaskName(corId,taskId,status,Name);
                res = new Response();
            }
            catch(Exception ex)
            {
                res = new Response("error in editing the name of task.\n" + ex.Message, null);

            }
            return Ok(JsonSerializer.Serialize(res));
        }

        [HttpPost("UpdateDesc")]
        public IActionResult PostEditTaskDesc([FromForm] string email, [FromForm] string password, [FromForm] int corId, [FromForm] int taskId, [FromForm] int status, [FromForm] string Desc)
        {
            Response res;
            try
            {
                BoardLogic bl = checkUser(email, password);
                bl.editTaskDesc(corId, taskId, status, Desc);
                res = new Response();
            }
            catch (Exception ex)
            {
                res = new Response("error in editing the desc of task.\n" + ex.Message, null);

            }
            return Ok(JsonSerializer.Serialize(res));
        }

        [HttpPost("UpdateDeadline")]
        public IActionResult PostEditTaskDeadline([FromForm] string email, [FromForm] string password, [FromForm] int corId, [FromForm] int taskId, [FromForm] int status, [FromForm] DateTime Deadline)
        {
            Response res;
            try
            {
                BoardLogic bl = checkUser(email, password);
                bl.editTaskDeadline(corId, taskId, status, Deadline);
                res = new Response();
            }
            catch (Exception ex)
            {
                res = new Response("error in editing the deadline time of task.\n" + ex.Message, null);

            }
            return Ok(JsonSerializer.Serialize(res));
        }

        [HttpPost("UpdateStartTime")]
        public IActionResult PostEditTaskStartTime([FromForm] string email, [FromForm] string password, [FromForm] int corId, [FromForm] int taskId,[FromForm] DateTime StartTime ,[FromForm] int status )
        {
            Response res;
            try
            {
                BoardLogic bl = checkUser(email, password);
                bl.editTaskStartTime(corId, taskId, status, StartTime);
                res = new Response();
            }
            catch (Exception ex)
            {
                res = new Response("error in editing the start time of task.\n" + ex.Message, null);

            }
            return Ok(JsonSerializer.Serialize(res));
        }
        [HttpPost("UpdateStartTimeFromCalendar")]
        public IActionResult PostEditTaskStartTimeCalendar([FromForm] string email, [FromForm] string password, [FromForm]int boardId, [FromForm] int corId, [FromForm] int taskId, [FromForm] DateTime StartTime, [FromForm] int status)
        {
            Response res;
            try
            {
                BoardLogic bl = checkUser(email, password);
                bl.editTaskStartTimeFromCalendar(boardId,corId, taskId, status, StartTime);
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
               
                res = new Response(bl.addNote(id, content));

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
        [HttpPost("getRandomMessage")]
        public IActionResult getRandomMessages([FromForm] string email, [FromForm]string password)
        {
            Response res;
            try
            {
                Message ms;
                
                MessageLogic ml = new MessageLogic();
                ms = ml.getRandomMessage();
                res = new Response(ms);
                
                
            }
            catch(Exception ex)
            {
                res = new Response(ex.Message, null);
            }
            return Ok(JsonSerializer.Serialize(res));
        }

        [HttpPost("getEmailMessages")]
        public IActionResult getEmailMessages([FromForm] string email, [FromForm] string password)
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
            catch (Exception ex)
            {
                res = new Response(ex.Message, null);
            }
            return Ok(JsonSerializer.Serialize(res));
        }


        // ----------------------PROFILE

        [HttpPost("userProfile")]
        public IActionResult getProfile([FromForm] string email, [FromForm] string password)
        {
            Response res;

            try
            {
                checkUser(email, password);
                User user = Users.UserLogic.GetUser(email);
                Profile p = user.Profile;
                res = new Response(p);
            }
            catch(Exception ex)
            {
                res = new Response(ex.Message,null);
            }
            return Ok(JsonSerializer.Serialize(res));
        }


        [HttpPost("purchase")]
        public IActionResult postPurchase([FromForm] string email, [FromForm] string password, [FromForm]string avatar)
        {
            Response res;

            try
            {
                checkUser(email, password);
               
                
                res = new Response(Users.UserLogic.purchase(email,avatar));
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message, null);
            }
            return Ok(JsonSerializer.Serialize(res));
        }

        [HttpPost("addCoins")]
        public IActionResult postAddCoin([FromForm]string email, [FromForm]string password, [FromForm]int amount)
        {
            Response res;

            try
            {
                checkUser(email, password);

                Users.UserLogic.addCoins(email, amount, false);
                res = new Response();
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message, null);
            }
            return Ok(JsonSerializer.Serialize(res));
        }

        [HttpPost("setAvatar")]
        public IActionResult postSetAvatar([FromForm]string email, [FromForm]string password, [FromForm]string avatar)
        {
            Response res;

            try
            {
                checkUser(email, password);


                res = new Response(Users.UserLogic.setAvatar(email, avatar));
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message, null);
            }
            return Ok(JsonSerializer.Serialize(res));
        }


        [HttpPost("getStoreAvatars")]
        public IActionResult postStoreAvatars([FromForm] string email, [FromForm]string password)
        {
            Response res;
            try
            {
                checkUser(email, password);
                res = new Response(Users.UserLogic.getStoreAvatars(email));
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
