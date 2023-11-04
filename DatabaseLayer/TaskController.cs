using myFirstAppSol.DatabaseLayer;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Runtime.CompilerServices;

namespace WebApplication2.DatabaseLayer
{
    public class TaskController:dbController<TaskDTO>
    {
        //table props
        private const string TableName = "Tasks";
        private const string ColId = "TaskId";
        private const string ColName = "TaskName";
        private const string ColDesc = "Description";
        private const string ColDeadline = "Deadline";
        private const string ColToStart = "toStart";
        private const string ColStatus = "status";
        private const string ColCorId = "corId";
       




        //constructor
        public TaskController()
        {
          
        }

        //get and insert querys
        public int getMaxID()
        {
            return DBF.getMaxId(TableName, ColId);
        }
        public void Insert(TaskDTO taskDTO)
        {
            int id = taskDTO.Id;
            string name = taskDTO.Name;   
            string description = taskDTO.Description;
            string deadline = taskDTO.Deadline.ToString(DBF.timeFormat);
            string toStart = taskDTO.ToStart.ToString(DBF.timeFormat);
            int corId = taskDTO.CorId;
            int status = taskDTO.Status;
            Dictionary<string,object> data = new Dictionary<string,object>();
            data.Add(ColName, name);
            data.Add(ColDesc, description);
            data.Add(ColId, id);
            data.Add(ColDeadline, deadline);
            data.Add(ColToStart, toStart);
            data.Add(ColStatus, status);
            data.Add(ColCorId, corId);
            DBF.Insert(data, TableName);



           
        }


        public override List<TaskDTO> getDTO(MySqlDataReader reader)
        {
            List<TaskDTO> toRet= new List<TaskDTO>();
            while (reader.Read())
            {

                string dateTimeDeadLine = (string)reader[ColDeadline];
                DateTime deadLine = DateTime.Parse(dateTimeDeadLine);
                string dateTimeToStart = (string)reader[ColToStart];
                DateTime toStart = DateTime.Parse(dateTimeToStart);

                TaskDTO tdto = new TaskDTO(Convert.ToInt32(reader[ColId]), (string)reader[ColName], (string)reader[ColDesc], deadLine, toStart, Convert.ToInt32(reader[ColStatus]), Convert.ToInt32(reader[ColCorId]), false);
                toRet.Add(tdto);
            }
            return toRet;
        }
        public List<TaskDTO> getTasks(int coriD)
        {
            return DBF.getDTOs(TableName, this, $"WHERE {ColCorId}={coriD}");

        }



        //update querys -------------

        public void updateName(string name, int id)
        {
            DBF.Update(TableName, ColName,name,ColId, id);
        }

        public void updateDesc(int id,string desc)
        {
            DBF.Update(TableName,ColDesc,desc,ColId, id);
        }

        public void updateStatus(int id, int status)
        {
            DBF.Update(TableName,ColStatus,status,ColId, id);
        }
        public void updateDeadline(int id,DateTime deadLine)
        {
            string deadline = deadLine.ToString(DBF.timeFormat);
            DBF.Update(TableName,ColDeadline,deadline,ColId, id);
        }
        public void updateTimeToStart(int id, DateTime timeToStartPar)
        {
            string toStart = timeToStartPar.ToString(DBF.timeFormat);
            DBF.Update(TableName, ColToStart, toStart, ColId, id);
        }

        //delet query

        public void deleteTask(int id)
        {
            DBF.delete(TableName, ColId,id);
        }

    }
}
