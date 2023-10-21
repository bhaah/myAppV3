using myFirstAppSol.DatabaseLayer;
using MySql.Data.MySqlClient;
using System.Data.SQLite;
using WebApplication2.LogicLayer.BoardFolder;

namespace WebApplication2.DatabaseLayer
{
    public class NoteController : dbController<NoteDTO>
    {
        private const string TableName = "NoteTable" ;
        private const string ColId = "NoteId";
        private const string ColName = "name" ;
        private const string ColBoardId = "boardId" ;




        //constructor ----------------------
        public NoteController()
        {
        
        }

        //insert and geters----------------------

        public void Insert(NoteDTO noteDTO)
        {
            int id = noteDTO.Id ;
            string note = noteDTO.Note ;
            int boardId= noteDTO.BoardId ;
            Dictionary<string ,object> data = new Dictionary<string ,object>();
            data[ColId] = id ;
            data[ColName] = note ;
            data[ColBoardId] = boardId ;
            DBF.Insert(data,TableName) ;


        }

        public override List<NoteDTO> getDTO(MySqlDataReader reader)
        {
            List<NoteDTO> list = new List<NoteDTO>();
            while (reader.Read())
            {
                NoteDTO ndto = new NoteDTO((string)reader[ColName], Convert.ToInt32(reader[ColId]), Convert.ToInt32(reader[ColBoardId]), false);
                list.Add(ndto);
            }
            return list;
        }
        public int getMaxID()
        {
            return DBF.getMaxId(TableName,ColId);
        }


        public List<NoteDTO> getAllNotes(int boardId)
        {
            return DBF.getDTOs(TableName, this,$"WHERE {ColBoardId}={boardId}");
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
