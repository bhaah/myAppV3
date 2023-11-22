

using myFirstAppSol.LogicLayer.BoardFolder;
using WebApplication2.DatabaseLayer;
using WebApplication2.Singletons;

namespace WebApplication2.LogicLayer.BoardFolder
{
    public class BoardLogic
    {
        private Dictionary<int, Board> _board;
        private Board currBoard;
        private string email;
        private BoardController bc=new BoardController();
        private AVLTaskCalendar avl = new AVLTaskCalendar();


        //constructor
        public BoardLogic(string email)
        {
            this.email = email;
            List<BoardDTO> boards = bc.getBoards(email);
            _board = new Dictionary<int, Board>();
            foreach (BoardDTO board in boards)
            {
                _board.Add(board.Id,new Board(board));
            }
            InsertTasksInAVL();
        }

        

        // geters for model

        public Dictionary<int, Board> Boards
        {
            get { return _board; }
        }

        // controling the boards list
        public Board creatBoard(string name) 
        {
            int id = bc.getMaxID() + 1;
            Board newBoard = new Board(id, name,email);
            _board.Add(id, newBoard);
            return newBoard;
        }
        public Dictionary<int, CornerOfTasks> getInBoard(int id) 
        {
            currBoard= _board[id];
            return currBoard.getAllCorners();
        }
        public void getOutBoard() 
        {
            currBoard = null;
        }
        public void deleteBoard(int id) 
        {
            _board[id].deleteBoard();
            _board.Remove(id);
        }


        // controlling the corners

        public Dictionary<int,CornerOfTasks> getCorners()
        {
            checkBoard();
            return currBoard.getAllCorners();
        }
        public void creatCorner(int idCor,string name,string desc) 
        {
            checkBoard();
            currBoard.addCorner(name,desc);
        }
        public void deleteCorner(int id) 
        {
            checkBoard();
            currBoard.removeCorner(id);
        }
        public int getCornerProgress(int id) 
        {
            checkBoard();
            return currBoard.getCornerProgress(id);
        }
        public List<Task> getCornerTasks(int id) 
        {
            checkBoard();
            return currBoard.getAllTasksInCor(id);
        }


        //funs :

        private void checkBoard()
        {
            if (currBoard == null) { throw new ArgumentException("choose a board"); }
        }
        
        //tasks:
        public Task creatTask(int corId,int taskId,string taskName,string taskDesc,DateTime dateTime) 
        {
            checkBoard();
            Task toRet =  currBoard.creatTask(corId,taskId,taskName,taskDesc,dateTime);
            avl.Insert(new TaskCalendarModel(toRet, toRet.Id));
            return toRet;
        }
        public void deleteTask(int boardId,int corId,int taskId) 
        {
            checkBoard();
            if(currBoard.ID==boardId)
            {
                currBoard.deleteTask(corId,taskId);
            }
        }
        public void moveTask(int corID,int taskId) 
        {
            checkBoard();
            Task moved=currBoard.moveTask(corID, taskId);
            if(moved.Status==3)
            {
                Users.UserLogic.addCoins(email, 8,true);
                Console.WriteLine("coins added to " + email);
            }
            if(moved.Status == 2)
            {
                avl.Insert(new TaskCalendarModel(moved, currBoard.ID));
            }
        }
        public void editTaskName(int corId,int taskId,int status,string name) 
        {
            checkBoard();
            currBoard.editName(corId, taskId, status, name);
        }
        public void editTaskDesc(int corId,int taskId,int status,string desc) 
        {
            checkBoard();
            currBoard.editDesc(corId, taskId, status, desc);
        }
        public void editTaskDeadline(int corId,int taskId,int status,DateTime dateTime) 
        {
            checkBoard();
            currBoard.editDeadline(corId, taskId, status, dateTime);
        }
        public void editTaskStartTime(int corId,int taskId,int status,DateTime dateTime) 
        {
            checkBoard();
            currBoard.setTimeTodo(corId, taskId, status, dateTime);
        }


        //notes
        public Note[] getAllNotes() 
        {
            checkBoard();
            Dictionary<int,Note> dict = currBoard.getAllNotes();
            Note[] result = new Note[dict.Count];
            int index = 0;
            foreach(Note note in dict.Values)
            {
                result[index] = note;
                index++;
            }
            return result;
        }
        public Note addNote(int id,string content) 
        {
            checkBoard();
            return currBoard.addNote(id, content);
        }
        public void removeNote(int id) 
        {
            checkBoard();
            currBoard.removeNote(id);
        }

        public void editNote(int id,string note)
        {
            checkBoard();
            currBoard.updateNote(note, id);
        }


        //celendar
        public void InsertTasksInAVL() 
        {
            
           
            foreach(Board b in _board.Values)
            {
                List<TaskCalendarModel> tasks = b.GetCalendarTasks();
                foreach(TaskCalendarModel t in tasks)
                {
                    avl.Insert(t);
                }
            }
        }
        public void getAllTasksWithDeadline(DateTime deadline) { }


    }
}
