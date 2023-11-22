namespace myFirstAppSol.LogicLayer.BoardFolder
{
    public class TaskCalendarModel
    {
       
        public WebApplication2.LogicLayer.BoardFolder.Task Task { get; set; }
        public int BoardId { get; set; }


        public TaskCalendarModel(WebApplication2.LogicLayer.BoardFolder.Task task,int boardId) 
        {
            Task= task;
            BoardId = boardId;
        }


        public int compareTo(TaskCalendarModel other)
        {
            return DateTime.Compare(Task.TaskFor,other.Task.TaskFor);
        }
    }
}
