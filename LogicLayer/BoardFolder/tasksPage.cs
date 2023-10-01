

namespace WebApplication2.LogicLayer.BoardFolder
{
    public class tasksPage
    {
        private string _title;
        private Dictionary<int,Task> _tasks;


        //consructor
        public tasksPage(string tittle)
        {
            _title = tittle;
            _tasks= new Dictionary<int,Task>();
        }
        // geters and seters
        public string Title
        {
            get { return _title; }
        }
        public Dictionary<int,Task> Tasks
        {
            get { return _tasks; }
        }

        //func for out use

        public int numOfTasks()
        {
            return _tasks.Count;
        }

        //task behavior:

        public Task getTaskWithId(int id)
        {
            foreach (Task t in _tasks.Values)
            {
                if (t.Id == id) return t;
            }
            throw new ArgumentException("there is no task with thisid");
        }

        public Task extractTaskWithId(int id)
        {
            if(!_tasks.ContainsKey(id)) throw new ArgumentException("there is no task with id:" + id );
            Task t= _tasks[id];
            _tasks.Remove(id);
            return t;
        }

        public void addTask(Task t)
        {
            _tasks.Add(t.Id,t);
        }



        //task props -----
        public void EditName(int id,  string name) 
        {
            if(!_tasks.ContainsKey(id)) { throw new ArgumentException("in this page there is no task id :"+id); }
            _tasks[id].Name = name;
        }
        public void EditDesc(int id,  string desc)
        {
            if (!_tasks.ContainsKey(id)) { throw new ArgumentException("in this page there is no task id :" + id); }
            _tasks[id].Description = desc;
        }
        public void EditDeadline(int id, DateTime dateTime) 
        {
            if (!_tasks.ContainsKey(id)) { throw new ArgumentException("in this page there is no task id :" + id); }
            _tasks[id].TaskFor=dateTime;
        }
        public void removeDeadline(int id) {
            if (!_tasks.ContainsKey(id)) { throw new ArgumentException("in this page there is no task id :" + id); }
            //_tasks[id].TaskFor = null;
        }
        public void setTimeTodo(int id,DateTime dateTime) 
        {
            if (!_tasks.ContainsKey(id)) { throw new ArgumentException("in this page there is no task id :" + id); }
            _tasks[id].TaskStart=dateTime;
        }
    }
}
