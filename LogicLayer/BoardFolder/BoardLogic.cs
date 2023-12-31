﻿

using myFirstAppSol.LogicLayer.BoardFolder;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WebApplication2.DatabaseLayer;
using WebApplication2.Singletons;

namespace WebApplication2.LogicLayer.BoardFolder
{
    public class BoardLogic
    {
        private Dictionary<int, Board> _board;
        private Board currBoard;
        private string email;
        private BoardController bc = new BoardController();
        private AVLTaskCalendar avl = new AVLTaskCalendar();

        public List<TaskCalendarModel> Calendar
        {
            get { return avl.Read(); }
        }





        //


        public Node deleteFromAvl(int boardId, int corId, int taskId)
        {
            Console.WriteLine("we entered the delete taskcalendar in boardLogic");
            Task t = _board[boardId].getTask(corId, taskId);
            TaskCalendarModel tcm = new TaskCalendarModel(t, boardId, corId);
            string json = JsonConvert.SerializeObject(tcm);
            Console.WriteLine("so we must delete this task :" + json + " from avl");
            return avl.delete(tcm);
        }



        //constructor
        public BoardLogic(string email)
        {
            this.email = email;
            List<BoardDTO> boards = bc.getBoards(email);
            _board = new Dictionary<int, Board>();
            foreach (BoardDTO board in boards)
            {
                _board.Add(board.Id, new Board(board));
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
            Board newBoard = new Board(id, name, email);
            _board.Add(id, newBoard);
            return newBoard;
        }
        public Dictionary<int, CornerOfTasks> getInBoard(int id)
        {
            currBoard = _board[id];
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

        public Dictionary<int, CornerOfTasks> getCorners()
        {
            checkBoard();
            return currBoard.getAllCorners();
        }
        public void creatCorner(int idCor, string name, string desc)
        {
            checkBoard();
            currBoard.addCorner(name, desc);
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
        public Task creatTask(int corId, int taskId, string taskName, string taskDesc, DateTime dateTime)
        {
            checkBoard();
            Task toRet = currBoard.creatTask(corId, taskId, taskName, taskDesc, dateTime);
            avl.Insert(new TaskCalendarModel(toRet, currBoard.ID, corId));
            return toRet;
        }
        public void deleteTask(int boardId, int corId, int taskId)
        {
            checkBoard();
            if (currBoard.ID == boardId)
            {
                currBoard.deleteTask(corId, taskId);
                deleteFromAvl(currBoard.ID, corId, taskId);
            }
        }
        public void moveTask(int corID, int taskId)
        {
            checkBoard();
            Task moved = currBoard.moveTask(corID, taskId);
            TaskCalendarModel TCM = new TaskCalendarModel(moved, currBoard.ID, corID);
            deleteFromAvl(currBoard.ID, corID, taskId);
            switch (TCM.Task.Status)
            {
                case 0:
                    avl.Insert(TCM);
                    break;
                case 1:
                    break;
                case 2:
                    avl.Insert(TCM);
                    break;
                case 3:
                    Users.UserLogic.addCoins(email, 8, true);
                    Console.WriteLine("coins added to " + email);
                    break;
                default:
                    break;

            }
        }

        public void moveTaskFromOut(int boardId,int corId,int taskId) {
            Board board = _board[boardId];
            Task moved = board.moveTask(corId, taskId);
            TaskCalendarModel TCM = new TaskCalendarModel(moved, board.ID, corId);
            
            deleteFromAvl(boardId, corId, taskId);
            switch (TCM.Task.Status)
            {
                case 0:
                    avl.Insert(TCM);
                    break;
                case 1:
                    break;
                case 2:
                    avl.Insert(TCM);
                    break;
                case 3:
                    Users.UserLogic.addCoins(email, 8, true);
                    Console.WriteLine("coins added to " + email);
                    break;
                default:
                    break;

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
            deleteFromAvl(currBoard.ID, corId, taskId);
            Task t=currBoard.editDeadline(corId, taskId, status, dateTime);
           
            avl.Insert(new TaskCalendarModel(t,currBoard.ID,corId));


        }
        public void editTaskStartTime(int corId,int taskId,int status,DateTime dateTime) 
        {
            checkBoard();
            Task t =currBoard.setTimeTodo(corId, taskId, status, dateTime);
            if(t.Status== 0)
            {
                moveTask(corId, taskId);
            }
        }
        public void editTaskStartTimeFromCalendar(int boardId,int corId, int taskId, int status, DateTime dateTime)
        {
            Board board = _board[boardId];
            Task t = board.setTimeTodo(corId, taskId, status, dateTime);
            if (t.Status == 0)
            {
                moveTaskFromOut(boardId,corId, taskId);
            }
        }



        public void checkTasksToMove()
        {
            foreach(Board b in _board.Values)
            {
                Dictionary<int,List<Task>> _cornerTasksToMove = b.checkTasksToMove();
                foreach(int corId in _cornerTasksToMove.Keys)
                {
                    Console.WriteLine("we are in corner with id: " + corId + "tasks to move count : " + _cornerTasksToMove[corId].Count());
                    foreach(Task t in _cornerTasksToMove[corId])
                    {
                        if (t.Status == 1)
                        {
                            moveTaskFromOut(b.ID, corId, t.Id);
                        }
                    }
                }
            }
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
                    Console.WriteLine("is this task " + t.Task.Name + " in the avl tree: " + avl.Search(t));
                    avl.Insert(t);
                }
            }
        }
        public void getAllTasksWithDeadline(DateTime deadline) { }


    }
}
