using myFirstAppSol.DatabaseLayer;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Npgsql;
using System.Data.SQLite;
using WebApplication2.LogicLayer.BoardFolder;

namespace WebApplication2.DatabaseLayer
{
    public class CornerController : dbController<CornerDTO>
    {

        private const string TableName = "Corners";
        private const string ColId = "CorId";
        private const string ColName = "nameCor";
        private const string ColDesc = "des";
        private const string ColProgress = "progress";
        private const string ColBoardId = "boardId";

        


        public CornerController()
        {
            
        }



        //insert and geterss------------------------------


        public void InsertCorner(CornerDTO cor)
        {
            int id = cor.Id;
            int boardId = cor.BoardId;
            string name = cor.Name;
            string desc = cor.Description;
            float progress = cor.Progress;
            Dictionary<string, object> data = new Dictionary<string, object>();
            data[ColId] = id;
            data[ColName] = name;
            data[ColDesc] = desc;
            data[ColProgress] = progress;
            data[ColBoardId] = boardId;
            DBF.Insert(data, TableName);
        }

        public override List<CornerDTO> getDTO(NpgsqlDataReader reader)
        {
            List<CornerDTO> result = new List<CornerDTO>();
            while (reader.Read())
            {
                CornerDTO cor = new CornerDTO(Convert.ToInt32(reader[ColBoardId]), Convert.ToInt32(reader[ColId]), (string)reader[ColName], (string)reader[ColDesc], Convert.ToInt32(reader[ColProgress]), false);
                result.Add(cor);
            }
            return result;
        }
        public List<CornerDTO> GetCorner(int BoardId)
        {
            return DBF.getDTOs(TableName, this,$"WHERE {ColBoardId}={BoardId}");
        }

        public int getMaxID()
        {
            return DBF.getMaxId(TableName, ColId);
        }


        // update funs ------------------------

        public void updateName(int id, string name)
        {
            DBF.Update(TableName, ColName,name,ColId , id);
        }

        public void updateProgress(int id ,int progress)
        {
            DBF.Update(TableName, ColProgress, progress, ColId, id);
        }

        public void updateDescription(int id,string description)
        {
            DBF.Update(TableName,ColDesc,description,ColId, id);
        }
        

        //delete data-----------------------------
        public void deleteCorner(int id)
        {
            DBF.delete(TableName, ColId, id);
        }

    }
}
