using WebApplication2.DatabaseLayer;
namespace WebApplication2.LogicLayer.BoardFolder
{
    public class CornerOfTasks
    {
        private int _corId;
        private string _name;
        private string _description;
        //private List<Task> _tasks;
        private tasksPage[] _pages;
        private int _progress;
        private CornerDTO _corDTO;
        private TaskController tc= new TaskController();
        

        //Constructor
        public CornerOfTasks(int boardId,int id, string name, string description)
        {
            _corId = id;
            _name = name;
            _description = description;
            _progress = 100;
            _pages = new tasksPage[] { new tasksPage("NEW"), new tasksPage("WAITING TO START"), new tasksPage("IN PROGRESS"), new tasksPage("DONE") };
            _corDTO = new CornerDTO(boardId,id,name, description,1,true);
        }

        public CornerOfTasks(CornerDTO corDTO)
        {
            _corDTO = corDTO;
            _corId = corDTO.Id;
            _name = corDTO.Name;
            _description = corDTO.Description;
            _pages = new tasksPage[] { new tasksPage("NEW"), new tasksPage("WAITING TO START"), new tasksPage("IN PROGRESS"), new tasksPage("DONE") };
            _progress = corDTO.Progress;
            List<TaskDTO> tasks = tc.getTasks(_corId);
            foreach(TaskDTO task in tasks)
            {
                Task TaskToAdd = new Task(task);
                _pages[TaskToAdd.Status].addTask(TaskToAdd);
            }

        }

        //geters & seters
        public int ID
        {
            get { return _corId; }
            set { _corId = value; }
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
        public int Progress
        {
            get
            {
                int inProgressTasks = _pages[2].numOfTasks();
                int doneTasks = _pages[3].numOfTasks();
                int allTasksInCorner = 0;
                foreach (tasksPage page in _pages)
                {
                    allTasksInCorner += page.numOfTasks();
                }
                if (allTasksInCorner != 0)
                {
                    float inProgressPoints = (float)(inProgressTasks) / 2;
                    Console.WriteLine(inProgressPoints.ToString());
                    float points = inProgressPoints + doneTasks;
                    Console.WriteLine(points.ToString());
                    float x = points / (float) allTasksInCorner;
                    Console.WriteLine(x.ToString());
                    return (int)(x*100);
                }
                return 100;
            }
        }



        //tasks behaviors :

        public void creatTask(string name, string description, int id, DateTime deadLine)
        {
            int taskId  = tc.getMaxID()+1;
            Task task = new Task(name, description, taskId, deadLine,_corId);
            _pages[0].addTask(task);
        }
        public void moveTask(int id)
        {
            Task toMove = null;
            int pageNum = -1;

            for (int page = 0; page < 4; page++)
            {

                try
                {
                    toMove = _pages[page].extractTaskWithId(id);
                    pageNum = page;
                }
                catch (Exception e) { }
            }
            if (pageNum >= 0 & pageNum < 3)
            {
                _pages[pageNum+1].addTask(toMove);
                toMove.MoveTask();
            }
        }
        public void EditName(int id, int status, string name)
        {
            _pages[status].EditName(id, name);
        }
        public void EditDesc(int id, int status, string desc)
        {
            _pages[status].EditDesc(id, desc);
        }
        public void EditDeadline(int id, int status, DateTime dateTime)
        {
            _pages[status].EditDeadline(id, dateTime);
        }
        public void removeDeadline()
        {

        }
        public void setTimeTodo(int id, int status, DateTime dateTime)
        {
            _pages[status].setTimeTodo(id, dateTime);
        }

        public List<Task> getTasks()
        {
            Console.WriteLine("we are here in eting the task of the corner");
            List<Task> tasks = new List<Task>();
            foreach(tasksPage page in _pages)
            {
                Dictionary<int, Task> dict = page.Tasks;
                foreach(Task task in dict.Values)
                {
                    tasks.Add(task);
                    Console.WriteLine(task.Name+"-task added");
                }
            }
            return tasks;
        }



    }
}
