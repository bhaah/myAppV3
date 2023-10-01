using System.Data.SqlClient;
using System.Data.SQLite;
using System.Runtime.CompilerServices;

namespace WebApplication2.DatabaseLayer
{
    public class TaskController
    {
        //table props
        private const string TableName = "Tasks";
        private const string ColId = "TaskId";
        private const string ColName = "Name";
        private const string ColDesc = "Description";
        private const string ColDeadline = "Deadline";
        private const string ColToStart = "toStart";
        private const string ColStatus = "status";
        private const string ColCorId = "corId";
        private const string timeFormat = "yyyy-MM-dd HH:mm:ss";




        //for connection
        private string path;
        private string connectionString;

        //constructor
        public TaskController()
        {
            path = DBF.path;
            connectionString = DBF.con;
        }

        //get and insert querys
        public int getMaxID()
        {
            int toRet = 0;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                int res = -1;
                SQLiteDataReader reader = null;
                try
                {
                    command.CommandText = $"SELECT MAX({ColId}) AS maxId FROM {TableName}";
                    DBF.prepare(command, connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (!Convert.IsDBNull(reader["maxId"]))
                        {
                            toRet = Convert.ToInt32(reader["maxId"]);
                        }
                    }
                }
                catch (Exception ex) { DBF.printEx(command, ex); }
                finally
                {
                    if (reader != null) reader.Close();
                    DBF.end(command, connection);
                }
            }
            return toRet;
        }
        public void Insert(TaskDTO taskDTO)
        {
            int id = taskDTO.Id;
            string name = taskDTO.Name;   
            string description = taskDTO.Description;
            string deadline = taskDTO.Deadline.ToString(timeFormat);
            string toStart = taskDTO.ToStart.ToString(timeFormat);
            int corId = taskDTO.CorId;
            int status = taskDTO.Status;
            using(SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                try
                {
                    command.CommandText = $"INSERT INTO {TableName} ({ColId}, {ColName}, {ColDesc}, {ColDeadline}, {ColToStart}, {ColStatus}, {ColCorId}) VALUES (@idVal, @nameVal, @descVal, @dlVal, @tsVal ,@statusVal, @corIdVal)";
                    DBF.convertVP(command, @"idVal", id);
                    DBF.convertVP(command, @"nameVal", name);
                    DBF.convertVP(command, @"descVal", description);
                    DBF.convertVP(command, @"dlVal", deadline);
                    DBF.convertVP(command, @"tsVal" ,toStart);
                    DBF.convertVP(command, @"statusVal", status);
                    DBF.convertVP(command, @"corIdVal", corId);
                    DBF.prepare(command,connection);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    DBF.printEx(command,ex);

                }
                finally
                {
                    DBF.end(command, connection);
                }
            }
        }
        public List<TaskDTO> getTasks(int coriD)
        {
            List<TaskDTO> toRet = new List<TaskDTO>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                SQLiteDataReader reader = null;
                try
                {
                    command.CommandText = $"SELECT * FROM {TableName} WHERE {ColCorId}={coriD}";
                    DBF.prepare(command, connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {

                        string dateTimeDeadLine = (string)reader[ColDeadline];
                        DateTime deadLine = DateTime.Parse(dateTimeDeadLine);
                        string dateTimeToStart = (string)reader[ColToStart];
                        DateTime toStart= DateTime.Parse(dateTimeToStart);

                        TaskDTO tdto = new TaskDTO(Convert.ToInt32(reader[ColId]), (string)reader[ColName], (string)reader[ColDesc], deadLine, toStart, Convert.ToInt32(reader[ColStatus]), Convert.ToInt32(reader[ColCorId]), false);
                        toRet.Add(tdto);
                    }

                }
                catch (Exception ex) { DBF.printEx(command, ex); }
                finally
                {
                    if (reader != null) { reader.Close(); }
                    DBF.end(command, connection);
                }
            }
            return toRet;

        }



        //update querys

        public void updateName(string name,int id)
        {
            using(SQLiteConnection connection = new SQLiteConnection())
        }



        //delet query



    }
}
