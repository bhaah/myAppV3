using System.Data.SQLite;
using System.IO;

namespace WebApplication2.DatabaseLayer
{
    internal class DBF
    {
        //public static const string Path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "DatabaseLayer\\MyAppDatabase.db"));
       // public static const string Conn = $"Data Source={this.Path}; Version=3; ";

        internal static string con
        {
            get
            {
                return $"Data Source={path}; Version=3; ";
            }
        }
        internal static string path
        {
            get
            {
                return Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "DatabaseLayer\\MyAppDatabase.db"));
            }
        }
        internal static void convertVP(SQLiteCommand command, string valuestring, object par)
        {
            SQLiteParameter param = new SQLiteParameter(valuestring, par);
            command.Parameters.Add(param);
        }
        internal static string timeFormat = "yyyy-MM-dd HH:mm:ss";
       

        internal static void prepare(SQLiteCommand command,SQLiteConnection con)
        {
            Console.WriteLine("line 21");
            con.Open();
            Console.WriteLine("line 23");
            command.Prepare();
        }
        internal static void printEx(SQLiteCommand command,Exception ex) 
        {
            Console.WriteLine(command.CommandText);
            Console.WriteLine(ex.Message);
        }

        internal static void end(SQLiteCommand command,SQLiteConnection connection) 
        {
            command.Dispose();
            connection.Close();
        }

        internal static void cleanData(string tableName)
        {
            using (SQLiteConnection connection = new SQLiteConnection(con))
            {
                
                SQLiteCommand command = new SQLiteCommand(connection);
                // connect the command to the DB
                int res = -1;
                try
                {

                    command.CommandText = $"DELETE FROM {tableName}";
                    connection.Open();
                    command.Prepare();


                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(command.CommandText);
                    Console.WriteLine(ex.ToString());
                    //log error
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
        }

        // THES FUNS ARE THE SAME FOR ALL CONTROLLERS ========================================
        internal static void Update(string TableName,string colNameToChange,object valueToChange,string colPK,object valuePK)
        {
            using (SQLiteConnection connection = new SQLiteConnection(con))
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                try
                {
                    command.CommandText = $"UPDATE {TableName} SET {colNameToChange} = @ValueToChange WHERE {colPK}= @pkValue";
                    convertVP(command, @"ValueToChange", valueToChange);
                    convertVP(command, @"pkValue", valuePK);
                    prepare(command, connection);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    printEx(command, ex);
                }
                finally { end(command, connection); }
            }
        }

        internal static void delete(string TableName,string colId,int id)
        {
            using(SQLiteConnection connection = new SQLiteConnection(con))
            {
                SQLiteCommand command = new SQLiteCommand(connection);
                try
                {
                    command.CommandText = $"DELETE FROM {TableName} WHERE {colId}={id}";
                    prepare(command, connection);
                    command.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    printEx(command, ex);
                }
                finally { end(command, connection); }
            }
        }
        
        public static int getMaxId(string TableName,string ColId)
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
    }
}
