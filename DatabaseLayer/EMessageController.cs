using System.Data.SQLite;
using WebApplication2.DatabaseLayer;

namespace myFirstAppSol.DatabaseLayer
{
    public class EMessageController
    {
        private const string tableName = "EMessages";
        private const string colId = "id";
        private const string colContent = "content";
        private const string colTime = "timeToSend";
        private const string colEmail = "email";

        // ==========
        private string path ;
        private string con;

        //cons ========
        public EMessageController()
        {
            path = DBF.path;
            con = DBF.con;
        }
        //===========INSERT AND GETTERS==================================================
        public void Insert(EMessageDTO emdto)
        {
            int id = emdto.Id;
            string content = emdto.Content;
            string email = emdto.Email;
            string time= emdto.Time.ToString(DBF.timeFormat);
            using(SQLiteConnection connection = new SQLiteConnection(con))
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                try
                {
                    command.CommandText = $"INSERT INTO {tableName} ({colId},{colContent},{colEmail},{colTime}) VALUES ({id},@conVal,@emailVal,'{time}')";
                    DBF.convertVP(command, @"conVal", content);
                    DBF.convertVP(command, @"emailVal", email);
                    DBF.prepare(command, connection);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    DBF.printEx(command, ex);

                }
                finally
                {
                    DBF.end(command, connection);
                }
            }

        }
        public List<EMessageDTO> getForEmail(string email)
        {
            List<EMessageDTO> list = new List<EMessageDTO>();
            using(SQLiteConnection connection = new SQLiteConnection(con))
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                SQLiteDataReader reader = null;
                try
                {
                    command.CommandText = $"SELECT * FROM {tableName} WHERE {colEmail} LIKE '%{email}%'";
                    DBF.prepare(command, connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        //Console.WriteLine(reader.ToString());
                        EMessageDTO toAdd = new EMessageDTO(Convert.ToInt32(reader[colId]), (string)reader[colContent],(string)reader[colEmail],DateTime.Parse((string)reader[colTime]),false);
                        list.Add(toAdd);
                        //Console.WriteLine(toAdd.Content);
                    }
                }
                catch(Exception ex) { DBF.printEx(command, ex);
                }
                finally
                {
                    if(reader != null) reader.Close();
                    DBF.end(command, connection);
                }

            }
            return list;
        }

        public List<EMessageDTO> getInDate(DateTime date)
        {
            string time = date.ToString(DBF.timeFormat);
            time = time.Substring(0, 10);
            List<EMessageDTO> list = new List<EMessageDTO>();
            using(SQLiteConnection connection =new SQLiteConnection(con))
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                SQLiteDataReader reader = null;
                try
                {
                    command.CommandText = $"SELECT * FROM {tableName} WHERE {colTime} LIKE '%{time}%'";
                    DBF.prepare(command,connection); reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        EMessageDTO emdto = new EMessageDTO(Convert.ToInt32(reader[colId]), (string)reader[colContent], (string)reader[colEmail], DateTime.Parse((string)reader[colTime]),false);
                        list.Add(emdto);
                    }
                }
                catch(Exception ex)
                {
                    DBF.printEx(command, ex);
                }
                finally { 
                    if(reader != null) reader.Close();
                    DBF.end(command, connection);
                }
            }
            return list;
        }
        public int getMaxId()
        {
            return DBF.getMaxId(tableName, colId);
        }
    }
}
