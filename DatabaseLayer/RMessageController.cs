using System.Data.SQLite;
using WebApplication2.DatabaseLayer;

namespace myFirstAppSol.DatabaseLayer
{
    public class RMessageController
    {
        private const string tableName = "RMessage";
        private const string colId = "id";
        private const string colContent = "content";

        private string path;
        private string con;



        public RMessageController()
        {
            path = DBF.path;
            con = DBF.con;
        }

        public void Insert(RMessageDTO rmdto)
        {
            int id = rmdto.Id;
            string content = rmdto.Content;
            using(SQLiteConnection connection= new SQLiteConnection(con))
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                try
                {
                    command.CommandText = $"INSERT INTO {tableName} ({colId},{colContent}) VALUES ({id},@conVal)";
                    DBF.convertVP(command, @"conVal", content);
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

        public List<RMessageDTO> getRMessages()
        {
            List<RMessageDTO> toRet = new List<RMessageDTO>();
            using(SQLiteConnection con= new SQLiteConnection(con))
            {
                SQLiteCommand command = new SQLiteCommand(con);
                SQLiteDataReader reader = null;
                try
                {
                    command.CommandText = $"SELECT * FROM {tableName} ";
                    DBF.prepare(command, con);
                    reader = command.ExecuteReader();
                    while(reader.Read())
                    {
                        RMessageDTO rm=new RMessageDTO(Convert.ToInt32(reader[colId]),(string)reader[colContent],false);
                        toRet.Add(rm);
                    }
                }
                catch(Exception ex)
                {
                    DBF.printEx(command, ex);
                }
                finally
                {
                    if(reader != null) reader.Close();
                    DBF.end(command, con);
                }
            }
            return toRet;
        }

        public int getMaxId()
        {
            return DBF.getMaxId(tableName, colId);
        }

    }
}
