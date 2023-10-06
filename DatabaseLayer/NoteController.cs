using System.Data.SQLite;

namespace WebApplication2.DatabaseLayer
{
    public class NoteController
    {
        private const string TableName = "NoteTable" ;
        private const string ColId = "NoteId";
        private const string ColName = "name" ;
        private const string ColBoardId = "boardId" ;


        //for connection :
        private string path ;
        private string con ;


        //constructor 
        public NoteController()
        {
            path = DBF.path;
            con = DBF.con;
        }

        //insert and geters

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





    }
}
