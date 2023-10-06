using System.Data.SQLite;

namespace WebApplication2.DatabaseLayer
{
    public class NoteController
    {
        private const string TableName = "NoteTable" ;
        private const string ColId = "NoteId";
        private const string ColName = "name" ;
        private const string ColBoardId = "boardId" ;


        //for connection :--------------------------
        private string path ;
        private string con ;


        //constructor ----------------------
        public NoteController()
        {
            path = DBF.path;
            con = DBF.con;
        }

        //insert and geters----------------------

        public void Insert(NoteDTO noteDTO)
        {
            int id = noteDTO.Id ;
            string note = noteDTO.Note ;
            int boardId= noteDTO.BoardId ;
            using(SQLiteConnection connection = new SQLiteConnection(con))
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                try
                {
                    command.CommandText = $"INSERT INTO {TableName} ({ColId},{ColName},{ColBoardId}) VALUES ({id}, @noteVal, {boardId}) ";
                    DBF.convertVP(command, @"noteVal", note);
                    DBF.prepare(command, connection);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    DBF.printEx(command, ex);
                }
                finally
                {
                    DBF.end(command,connection);
                }
            }




        }


        public int getMaxID()
        {
            int toRet = 0;
            using (SQLiteConnection connection = new SQLiteConnection(con))
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


        public List<NoteDTO> getAllNotes(int boardId)
        {
            List<NoteDTO> toRet = new List<NoteDTO>();
            using(SQLiteConnection connection = new SQLiteConnection(con)) 
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                SQLiteDataReader reader = null;
                try
                {
                    command.CommandText = $"SELECT * FROM {TableName} WHERE {ColBoardId}={boardId}";
                    DBF.prepare(command, connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        NoteDTO ndto = new NoteDTO((string)reader[ColName], Convert.ToInt32(reader[ColId]), boardId,false);
                        toRet.Add(ndto);
                    }
                }
                catch(Exception ex)
                {
                    DBF.printEx(command, ex);
                }
                finally
                {
                    if (reader != null) reader.Close();
                    DBF.end(command, connection);
                }
            }
            return toRet;
        }



        // update method: -------------------------

        public void updateNote(int id,string note)
        {
            DBF.Update(TableName, ColName, note, ColId, id);
        }


        //delete method: -----------------------
        public void deleteNote(int id)
        {
            DBF.delete(TableName, ColId, id);
        }

    }
}
