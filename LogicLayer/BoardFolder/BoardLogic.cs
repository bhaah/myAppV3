

using WebApplication2.DatabaseLayer;

namespace WebApplication2.LogicLayer.BoardFolder
{
    public class BoardLogic
    {
        private Dictionary<int, Board> _board;
        private Board currBoard;
        private string email;
        private BoardController bc=new BoardController();

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
        }

        

        // geters for model

        public Dictionary<int, Board> Boards
        {
            get { return _board; }
        }

        // controling the boards list
        public void creatBoard(string name) 
        {
            int id = bc.getMaxID() + 1;
            Board newBoard = new Board(id, name,email);
            _board.Add(id, newBoard);
        }
        public void getInBoard(int id) 
        {
            currBoard= _board[id];
        }
        public void getOutBoard() 
        {
            currBoard = null;
        }
        public void deleteBoard(int id) 
        {
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
        public void creatTask(int corId,int taskId,string taskName,string taskDesc,DateTime dateTime) 
        {
            checkBoard();
            currBoard.creatTask(corId,taskId,taskName,taskDesc,dateTime);
        }
        public void deleteTask() 
        {
            checkBoard();
            
        }
        public void moveTask(int corID,int taskId) 
        {
            checkBoard();
            currBoard.moveTask(corID, taskId);
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
        public void addNote(int id,string content) 
        {
            checkBoard();
            currBoard.addNote(id, content);
        }
        public void removeNote() { }


        //celendar
        public void getAllTasksIn(DateTime start) { }
        public void getAllTasksWithDeadline(DateTime deadline) { }


    }
}
