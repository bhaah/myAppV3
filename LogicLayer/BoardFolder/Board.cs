using WebApplication2.DatabaseLayer;

namespace WebApplication2.LogicLayer.BoardFolder
{
    public class Board
    {
        private int _id;
        private string _name;
        private Dictionary<int, CornerOfTasks> _corners;
        private Dictionary<int,Note> _notes;
        private BoardDTO bdto;
        private CornerController cc = new CornerController();
        
        //cons
        public Board(int id, string name,string email)
        {
            _id = id;
            _name = name;
            _notes = new Dictionary<int, Note>();
            _corners = new Dictionary<int, CornerOfTasks>();
            int cornerId = cc.getMaxID() + 1;
            _corners.Add(cornerId, new CornerOfTasks(id,cornerId, "Tasks", "without corners"));
            bdto = new BoardDTO(id, name,email,true);
            bdto.persist();
        }

        public Board(BoardDTO dto)
        {
            _id = dto.Id;
            _name = dto.Name;
            _notes = new Dictionary<int, Note>();
            _corners = new Dictionary<int, CornerOfTasks>();
            List<CornerDTO> corners = cc.GetCorner(_id);
            foreach (CornerDTO corner in corners)
            {
                _corners.Add(corner.Id, new CornerOfTasks(corner));
            }

            bdto = dto;
        }

        //geters

        public int ID
        {
            get { return _id; }
        }
        public string Name
        {
            get { return _name; }
        }

        //corners:
        public Dictionary<int,CornerOfTasks> getAllCorners()
        {
            return _corners;
        }


        public void addCorner( string name, string desc)
        {
            int cornerId = cc.getMaxID() + 1;
            CornerOfTasks newCor = new CornerOfTasks(_id,cornerId, name, desc);
            Console.WriteLine($"we are in Board.addCorner and the id is {cornerId} \n and the id in the corner is:{newCor.ID}");
            _corners.Add(cornerId, newCor);
        }
        public void removeCorner(int id)
        {
            _corners.Remove(id);
        }
        public int getCornerProgress(int id)
        {
            if(!_corners.ContainsKey(id)) { throw new ArgumentException("there is no corner with this id"); }
            return _corners[id].Progress;
        }   

        public List<Task> getAllTasksInCor(int id)
        {
            if (!_corners.ContainsKey(id)) { throw new ArgumentException("there is no corner with this id"); }
            return _corners[id].getTasks();
        }
        //tasks:

        public void creatTask(int CorID, int taskId, string name, string desc, DateTime dateTime)
        {
            _corners[CorID].creatTask(name, desc, taskId, dateTime);
        }
        public void moveTask(int CorID, int taskId)
        {
            _corners[CorID].moveTask(taskId);
        }
        public void editName(int CorId, int taskid, int status, string name)
        {
            _corners[CorId].EditName(taskid, status, name);
        }
        public void editDesc(int corId, int taskId, int status, string desc)
        {
            _corners[corId].EditDesc(taskId, status, desc);
        }
        public void editDeadline(int corId, int taskID, int status, DateTime dateTime)
        {
            _corners[corId].EditDeadline(taskID, status, dateTime);
        }
        public void removeDeadline() { }
        public void setTimeTodo(int corId, int taskId, int status, DateTime dateTime)
        {
            _corners[corId].setTimeTodo(taskId, status, dateTime);
        }



        //notes

        public Dictionary<int,Note> getAllNotes() 
        {
            return _notes;
        }
        public void addNote(int id,string content) 
        {
            _notes.Add(id,new Note(id,content));            
        }
        public void removeNote(int id) 
        {
            _notes.Remove(id);
        }



    }
}
