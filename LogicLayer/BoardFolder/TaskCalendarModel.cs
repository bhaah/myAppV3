namespace myFirstAppSol.LogicLayer.BoardFolder
{
    public class TaskCalendarModel
    {

        public Task Task { get; set; }
        public int BoardId { get; set; }


        public TaskCalendarModel(Task task,int boardId) 
        {
            Task= task;
            BoardId = boardIdl;
        }
    }
}
