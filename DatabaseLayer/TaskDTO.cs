namespace WebApplication2.DatabaseLayer
{
    public class TaskDTO
    {
        private int _id;
        private string _name;
        private string _description;
        private DateTime _deadline;
        private DateTime _toStart;
        private int _status;
        private int _corId;
        
        //for database
        private TaskController tc=new TaskController();
        private bool isPersisted;


        //geters & seters
        public int Id { 
            get { return _id; }
            
        }
        public string Name
        {
            get { return _name; }
            set { 
                if(isPersisted)
                {
                    tc.updateName(value, Id);
                }
                
                _name = value; }
        }
        public string Description
        {
            get { return _description; }
            set 
            {
                if (isPersisted)
                {
                    tc.updateDesc(Id, value);
                }
                
                _description = value; }
        }
        public DateTime Deadline
        {
            get { return _deadline; }
            set
            {
                if (isPersisted)
                {
                    tc.updateDeadline(Id, value);
                }
                _deadline = value;
            }
        }
        public DateTime ToStart
        {
            get { return _toStart; }
            set
            {
                if (isPersisted)
                {
                    tc.updateTimeToStart(Id, value);
                }
                _toStart = value;
            }
        }
        public int Status
        {
            get { return _status; }
            set
            {
                Console.WriteLine("this debug is from -set status in taskDTO");
                if (isPersisted)
                {
                    Console.WriteLine("dto is persisted and now we are about to update it in the db");
                    tc.updateStatus(Id, value);
                }
                _status = value;
            }
        }

        public int CorId
        {
            get { return _corId; }
        }



        public TaskDTO(int id, string name, string description, DateTime deadline, DateTime toStart, int status, int corId,bool toPersist)
        {
            _id = id;
            Name = name;
            Description = description;
            Deadline = deadline;
            ToStart = toStart;
            Status = status;
            _corId = corId;
            if (toPersist)
            {
                persist();
            }
            else
            {
                isPersisted= true;
            }
        }


        public void persist()
        {
            tc.Insert(this);
            isPersisted= true;
            
        }

        public void delete()
        {
            tc.deleteTask(Id);
            isPersisted= false;
        }
    }
}
