using MySql.Data.MySqlClient;
using System.Data.SQLite;
using WebApplication2.DatabaseLayer;

namespace myFirstAppSol.DatabaseLayer
{
    public class EMessageController : dbController<EMessageDTO>
    {
        private const string tableName = "EMessages";
        private const string colId = "id";
        private const string colContent = "content";
        private const string colTime = "timeToSend";
        private const string colEmail = "email";


        //cons ========
        public EMessageController()
        {

        }
        //===========INSERT AND GETTERS==================================================
        public void Insert(EMessageDTO emdto)
        {
            int id = emdto.Id;
            string content = emdto.Content;
            string email = emdto.Email;
            string time= emdto.Time.ToString(DBF.timeFormat);
            Dictionary<string,object> data = new Dictionary<string,object>();
            data.Add(colId, id);
            data.Add(colContent, content);
            data.Add(colEmail, email);
            data.Add(colTime, time);
            DBF.Insert(data, tableName);

        }

        public override List<EMessageDTO> getDTO(MySqlDataReader reader)
        {
            List<EMessageDTO> list = new List<EMessageDTO>();
            while(reader.Read())
            {
                EMessageDTO toAdd = new EMessageDTO(Convert.ToInt32(reader[colId]), (string)reader[colContent], (string)reader[colEmail], DateTime.Parse((string)reader[colTime]),false);
                list.Add(toAdd);
            }
            return list;
        }


        public List<EMessageDTO> getForEmail(string email)
        {


            List<EMessageDTO> list = DBF.getDTOs(tableName, this, $"WHERE {colEmail} LIKE '%{email}%'");
            
            return list;
        }

        public List<EMessageDTO> getInDate(DateTime date)
        {
            string time = date.ToString(DBF.timeFormat);
            time = time.Substring(0, 10);
            List<EMessageDTO> list = DBF.getDTOs(tableName, this, $"WHERE {colTime} LIKE '%{time}%'");
            return list;
        }
        public int getMaxId()
        {
            return DBF.getMaxId(tableName, colId);
        }


        public void deleteMessage(int id)
        {
            DBF.delete(tableName, colId, id);
        }
    }
}
