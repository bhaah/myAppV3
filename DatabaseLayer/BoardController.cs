using System.Data.SQLite;

namespace WebApplication2.DatabaseLayer
{
    public class BoardController
    {
        private const string tableName = "Board";
        private const string ColId = "boardId";
        private const string ColName = "name";
        private const string ColEmail = "UserEmail";



        private string path;
        private string connectionString;


        public BoardController()
        {
            path = DBF.path;
            connectionString = DBF.con;
        }


        public void addBoard(BoardDTO board)
        {
            int id = board.Id;
            string name = board.Name;
            string email = board.Email;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                int res = -1;
                try
                {
                    command.CommandText = $"INSERT INTO {tableName} ({ColId}, {ColName}, {ColEmail}) VALUES (@idVal, @nameVal, @emailVal)";
                    DBF.convertVP(command, @"idVal", id);
                    DBF.convertVP(command, @"nameVal", name);
                    DBF.convertVP(command, @"emailVal", email);
                    DBF.prepare(command, connection);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    DBF.printEx(command, ex);
                }
                finally { DBF.end(command, connection); }
            }
        }



        public List<BoardDTO> getBoards(string email)
        {
            List<BoardDTO> boardDTOs= new List<BoardDTO>();
            using(SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                int res = -1;
                SQLiteDataReader reader= null;
                try
                {
                    command.CommandText = $"SELECT * FROM {tableName} WHERE {ColEmail} = @emailVal";
                   
                    DBF.convertVP(command, @"emailVal", email);
                 
                    connection.Open();
                    command.Prepare();
                   
                    reader= command.ExecuteReader();
                    Console.WriteLine(reader.ToString());
                    while (reader.Read())
                    {
                        Console.WriteLine("DEBUG:hi from BoadController.getBoads");
                        BoardDTO toAdd = new BoardDTO(Convert.ToInt32(reader[ColId]), (string)reader[ColName], (string)reader[ColEmail], false); 
                        boardDTOs.Add(toAdd);
                        Console.WriteLine($"board with name :{toAdd.Name} . added!");
                    }

                }
                catch (Exception ex)
                {
                    DBF.printEx(command, ex);
                }
                finally {
                    if(reader != null) reader.Close();
                    DBF.end(command, connection);}
                return boardDTOs;
            }
        }



        public int getMaxID()
        {
            int toRet= 0;
            using(SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                int res = -1;
                SQLiteDataReader reader= null;
                try
                {
                    command.CommandText = $"SELECT MAX({ColId}) AS maxId FROM {tableName}";
                    DBF.prepare(command, connection);
                    reader= command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (!Convert.IsDBNull(reader["maxId"]))
                        {
                            toRet = Convert.ToInt32(reader["maxId"]);
                        }
                    }
                }
                catch(Exception ex) { DBF.printEx(command, ex); }
                finally { if(reader != null) reader.Close();
                DBF.end(command, connection);}
            }
            return toRet;
        }

    }
}
