using WebApplication2.DatabaseLayer;

namespace WebApplication2.LogicLayer.BoardFolder
{
    public class Task
    {

        //task properties:
        private int _id;
        private string _name;
        private string _description;
        private DateTime _taskDeadLine;
        private DateTime _taskStart;
        private int _status = 0;//0-new 'white'
                                //1-wating to start 'blue'
                                //2-inProgress 'red'
                                //3-done 'green'

        private int corId;
        private TaskDTO tdto;

        //geters and seters
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public DateTime TaskFor
        {
            get { return _taskDeadLine; }
            set { _taskDeadLine = value; }
        }

        public DateTime TaskStart
        {
            get { return _taskStart; }
            set { _taskStart = value; }
        }

        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        //const

        public Task(string name, string description, int id, DateTime deadLine,int corId)
        {
            Name = name;
            Description = description;
            Id = id;
            _taskDeadLine = deadLine;
            this.corId= corId;
            tdto = new TaskDTO(id,name,description,deadLine,new DateTime(),0,corId,true);
        }

        public Task(TaskDTO taskDTO)
        {
            _id = taskDTO.Id;
            _name = taskDTO.Name;
            _description = taskDTO.Description;
            _status = taskDTO.Status;
            _taskDeadLine = taskDTO.Deadline;
            _taskStart = taskDTO.ToStart;
            corId= taskDTO.CorId;
            tdto = taskDTO;
        }



        public void MoveTask()
        {
            if (_status < 3) _status++;
        }
    }
}
