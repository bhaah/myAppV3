using myFirstAppSol.DatabaseLayer;
using MySql.Data.MySqlClient;
using System.Data.SQLite;

namespace WebApplication2.DatabaseLayer
{
    public class UserController : dbController<UserDTO>
    {
        private const string tableName = "Users";
        private const string ColEmail = "Email";
        private const string ColUserName = "UserName";
        private const string ColPassword = "Password";



        //C:\Users\bhaah\OneDrive\שולחן העבודה\WebApplication2\DatabaseLayer\MyAppDatabase.db
        public UserController()
        {
           
        }
        private void convertValToPar(SQLiteCommand command, string valuestring, object par)
        {
            SQLiteParameter param = new SQLiteParameter(valuestring, par);
            command.Parameters.Add(param);
        }
        //----- get and add Users

        public override List<UserDTO> getDTO(MySqlDataReader reader)
        {
            List<UserDTO> toRet = new List<UserDTO>();
            while (reader.Read())
            {
                UserDTO toAdd = new UserDTO((string)reader[ColEmail], (string)reader[ColPassword], (string)reader[ColUserName], false);
                Console.WriteLine($"we got this user :{toAdd.Email},{toAdd.UserName}");
                toRet.Add(toAdd);
            }
            return toRet;
        }
        public List<UserDTO> getAllUsers()
        {
            return DBF.getDTOs(tableName, this, null);
        }


        public UserDTO getUser(string Email)
        {
            UserDTO user =null;
            List<UserDTO> userList = DBF.getDTOs(tableName, this, $"WHERE {ColEmail} LIKE '%{Email}%'" );
            if(userList.Count > 0)
            {
                return userList[0];
            }
            return null;
        }

        public void addUser(UserDTO user)
        {
            string email = user.Email;
            string userName = user.UserName;
            string password = user.Password;
            Dictionary<string,object> data = new Dictionary<string,object>();
            data.Add(ColEmail, email);
            data.Add (ColUserName, userName);
            data.Add(ColPassword, password);
            DBF.Insert(data,tableName);
        }


        // updates 
       




    }
}
