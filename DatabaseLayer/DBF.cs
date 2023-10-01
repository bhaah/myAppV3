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
    }
}
