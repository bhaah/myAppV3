using myFirstAppSol.DatabaseLayer;
using System.Data.SQLite;
using MySql.Data.MySqlClient;
using Npgsql;

namespace WebApplication2.DatabaseLayer
{
    public class BoardController : dbController<BoardDTO>
    {
        private const string tableName = "Board";
        private const string ColId = "boardId";
        private const string ColName = "nameBoard";
        private const string ColEmail = "UserEmail";




        //consructor -----------------
        public BoardController()
        {
           
        }

        //geters and insert ========================================


        //this to get the dtos from reader
        public override List<BoardDTO> getDTO(NpgsqlDataReader reader) 
        {
            List<BoardDTO> boardDTOs= new List<BoardDTO>();
            while (reader.Read())
            {
                BoardDTO toAdd = new BoardDTO(Convert.ToInt32(reader[ColId]), (string)reader[ColName], (string)reader[ColEmail], false);
                boardDTOs.Add(toAdd);
            }
            return boardDTOs;
        }


       
        public void addBoard(BoardDTO board)
        {

            int id = board.Id;
            string name = board.Name;
            string email = board.Email;
            Dictionary<string,object> data = new Dictionary<string,object>();
            data.Add(ColId, id);
            data.Add(ColName, name);
            data.Add(ColEmail, email);
            DBF.Insert(data, tableName);
            
        }

        public List<BoardDTO> getBoards(string email)
        {
            string addition = $"WHERE {ColEmail} = '{email}'";
            return DBF.getDTOs<BoardDTO>(tableName,this,addition);
        }

        public int getMaxID()
        {
            return DBF.getMaxId(tableName, ColId);
        }
        //update method : ------------------------------

        public void updateName(int id,string name)
        {
            DBF.Update(tableName, ColName,name,ColId,id);
        }


        //delete from database -----------------
        public void deleteBoad(int id)
        {
            DBF.delete(tableName,ColId,id);
        }
    }
}
