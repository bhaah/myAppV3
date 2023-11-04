using MySql.Data.MySqlClient;
using Npgsql;
using System.Data.SQLite;
using WebApplication2.DatabaseLayer;

namespace myFirstAppSol.DatabaseLayer
{
    public class RMessageController : dbController<RMessageDTO>
    {
        private const string tableName = "RMessages";
        private const string colId = "RMid";
        private const string colContent = "RMcontent";

   

        public RMessageController()
        {
         
        }


        public override List<RMessageDTO> getDTO(NpgsqlDataReader reader)
        {
            List<RMessageDTO> rMessageDTOs= new List<RMessageDTO>();
            while (reader.Read())
            {
                RMessageDTO rm = new RMessageDTO(Convert.ToInt32(reader[colId]), (string)reader[colContent], false);
                rMessageDTOs.Add(rm);
            }
            return rMessageDTOs;
        }
        public void Insert(RMessageDTO rmdto)
        {
            int id = rmdto.Id;
            string content = rmdto.Content;
            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add(colId, id);
            dic.Add(colContent, content);
            DBF.Insert(dic, tableName);
        }

        public List<RMessageDTO> getRMessages()
        {
            return DBF.getDTOs(tableName, this, null);
        }

        public int getMaxId()
        {
            return DBF.getMaxId(tableName, colId);
        }

    }
}
