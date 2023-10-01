using System.Data.SQLite;

namespace WebApplication2.DatabaseLayer
{
    public class CornerController
    {

        private const string TableName = "CornerTable";
        private const string ColId = "CorId";
        private const string ColName = "name";
        private const string ColDesc = "desc";
        private const string ColProgress = "progress";
        private const string ColBoardId = "boardId";

        //for database connection
        private string path;
        private string conectionString;


        public CornerController()
        {
            path = DBF.path;
            conectionString = DBF.con;
        }



        //insert and geterss


        public void InsertCorner(CornerDTO cor)
        {
            int id = cor.Id;
            int boardId = cor.BoardId;
            string name = cor.Name;
            string desc = cor.Description;
            float progress = cor.Progress;
            using (SQLiteConnection connection = new SQLiteConnection(conectionString))
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                try
                {
                    command.CommandText = $"INSERT INTO {TableName} ({ColId}, {ColName}, {ColDesc}, {ColProgress}, {ColBoardId}) VALUES (@idVal, @nameVal, @descVal, @proVal, @bidVal)";
                    DBF.convertVP(command, @"idVal", id);
                    DBF.convertVP(command, @"nameVal", name);
                    DBF.convertVP(command, @"descVal", desc);
                    DBF.convertVP(command, @"proVal", progress);
                    DBF.convertVP(command, @"bidVal", boardId);
                    DBF.prepare(command, connection);
                    command.ExecuteNonQuery();


                }
                catch (Exception ex) { DBF.printEx(command, ex); }
                finally { DBF.end(command, connection); }
            }
        }


        public List<CornerDTO> GetCorner(int BoardId)
        {
            List<CornerDTO> result = new List<CornerDTO>();
            using(SQLiteConnection connection = new SQLiteConnection(conectionString))
            {
                SQLiteCommand command = new SQLiteCommand(connection) ;
                SQLiteDataReader reader = null;
                try
                {
                    command.CommandText = $"SELECT * FROM {TableName} WHERE {ColBoardId}={BoardId}";
                    DBF.prepare(command, connection);
                    reader = command.ExecuteReader();
                    while(reader.Read())
                    {
                        CornerDTO cor = new CornerDTO(BoardId, Convert.ToInt32(reader[ColId]), (string)reader[ColName], (string)reader[ColDesc],Convert.ToInt32(reader[ColProgress]),false);
                        result.Add(cor);
                        Console.WriteLine($"the corner :{cor.Id} , in board {BoardId} , returned from database");
                    }
                }
                catch(Exception ex) { DBF.printEx(command, ex);}
                finally { 
                    if(reader != null) reader.Close();
                    DBF.end(command, connection);}
               
            }
            return result;
        }

        public int getMaxID()
        {
            int toRet = 0;
            using (SQLiteConnection connection = new SQLiteConnection(conectionString))
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

    }
}
