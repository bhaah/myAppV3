using System.Data.SQLite;

namespace WebApplication2.DatabaseLayer
{
    public class UserController
    {
        private const string tableName = "Users";
        private const string ColEmail = "Email";
        private const string ColUserName = "UserName";
        private const string ColPassword = "Password";


        private string path;
        private string connectionString;

        //C:\Users\bhaah\OneDrive\שולחן העבודה\WebApplication2\DatabaseLayer\MyAppDatabase.db
        public UserController()
        {
            path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "DatabaseLayer\\MyAppDatabase.db"));
            connectionString = $"Data Source={path}; Version=3; ";
        }
        private void convertValToPar(SQLiteCommand command, string valuestring, object par)
        {
            SQLiteParameter param = new SQLiteParameter(valuestring, par);
            command.Parameters.Add(param);
        }
        //----- get and add Users


        public List<UserDTO> getAllUsers()
        {
            List<UserDTO> toRet = new List<UserDTO>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                int res = -1;
                SQLiteDataReader reader = null;
                try
                {
                    command.CommandText = $"SELECT * FROM {tableName}";
                    //convertValToPar(command, @"emailVal", Email);
                    connection.Open();
                    command.Prepare();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        UserDTO toAdd = new UserDTO((string)reader[ColEmail], (string)reader[ColPassword], (string)reader[ColUserName], false);
                        Console.WriteLine($"we got this user :{toAdd.Email},{toAdd.UserName}");
                        toRet.Add(toAdd);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(command.CommandText);
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (reader != null) { reader.Close(); }
                    command.Dispose();
                    connection.Close();
                }
                return toRet;
            }
        }


        public UserDTO getUser(string Email)
        {
            UserDTO toRet=null;
            using(SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                int res = -1;
                SQLiteDataReader reader = null;
                try
                {
                    command.CommandText = $"SELECT * FROM {tableName} WHERE {ColEmail}= @emailVal";
                    convertValToPar(command, @"emailVal", Email);
                    connection.Open();
                    command.Prepare();
                    reader = command.ExecuteReader();
                    while(reader.Read())
                    {
                        toRet = new UserDTO(Email, (string)reader[ColPassword], (string)reader[ColUserName],false);
                        Console.WriteLine($"we got this user :{toRet.Email},{toRet.UserName}");
                    }
                    
                }
                catch (Exception ex) 
                {
                    Console.WriteLine(command.CommandText);
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if(reader!= null) { reader.Close(); }
                    command.Dispose();
                    connection.Close();   
                }
                return toRet;
            }
        }

        public void addUser(UserDTO user)
        {
            string email = user.Email;
            string userName = user.UserName;
            string password = user.Password;

            using(SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                int res = -1;
                try
                {
                    command.CommandText = $"INSERT INTO {tableName} ({ColUserName}, {ColPassword}, {ColEmail}) VALUES (@unVal, @passVal, @emailVal)";
                    convertValToPar(command, @"unVal", userName);
                    convertValToPar(command, @"passVal", password);
                    convertValToPar(command, @"emailVal", email);
                    connection.Open();
                    command.Prepare();
                    res= command.ExecuteNonQuery();
                   
                }
                catch(Exception ex)
                {
                    Console.WriteLine(command.CommandText);
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine(path);
                }
                finally {
                    command.Dispose(); 
                    connection.Close();
                }

            }
        }


        // updates 





    }
}
