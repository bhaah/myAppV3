﻿using System.Data.SQLite;
using System.IO;
using myFirstAppSol.DatabaseLayer;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace WebApplication2.DatabaseLayer
{
    internal class DBF
    {
        //public static const string Path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "DatabaseLayer\\MyAppDatabase.db"));
        // public static const string Conn = $"Data Source={this.Path}; Version=3; ";

        public const string connectionString = "Server=sql12.freesqldatabase.com;Database=sql12655161;User=sql12655161;Password=VmjNVLnRdI;";
        

        public static void Insert(Dictionary<string, object> map , string tableName)
        {
            MySqlConnection connection = null;
            MySqlCommand cmd = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                string[] cols_values = getParms(map);
                string sqlQuery = $"INSERT INTO {tableName} {cols_values[0]} VALUES {cols_values[1]};";
                cmd = new MySqlCommand(sqlQuery, connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                printEx(cmd,ex);
            }
            finally
            {
                if (cmd != null) { cmd.Dispose(); }
                if (connection != null) { connection.Close(); }
                
            }
        } 
        
        private static string[] getParms(Dictionary<string, object> map)
        {
            string columns = "(";
            string values = "(";
            foreach (string key in map.Keys)
            {
                columns += $" {key},";
                values += $" '{map[key]}',";
            }
            columns = columns.Substring(0, columns.Length - 1) + ")";
            values = values.Substring(0, values.Length - 1) + ")";
            return new string[] { columns, values };
        }



        public static List<T> getDTOs<T>(string tableName,dbController<T> controller, string addetionQuery)
        {
            MySqlConnection connection = null;
            MySqlCommand cmd = null;
            MySqlDataReader reader= null;
            List<T> list = new List<T>();
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                string sqlQuery = $"SELECT * FROM {tableName}";
                if(addetionQuery!= null) { sqlQuery =sqlQuery + " " + addetionQuery; }
                cmd = new MySqlCommand(sqlQuery, connection);
                reader= cmd.ExecuteReader();
                list = controller.getDTO(reader);
            }
            catch(Exception ex)
            {
                printEx(cmd, ex);
            }
            finally { 
                if(reader != null) { reader.Close(); }
                if(cmd!= null) { cmd.Dispose(); }
                if(connection != null){connection.Close();}
            }
            return list;
        }

        
        internal static void Update(string TableName, string colNameToChange, object valueToChange, string colPK, object valuePK)
        {

            MySqlConnection connection = null;
            MySqlCommand cmd = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                string sqlQuery = $"UPDATE {TableName} SET {colNameToChange} = @ValueToChange WHERE {colPK}= @pkValue";
                cmd = new MySqlCommand(sqlQuery, connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                printEx(cmd, ex);
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (connection != null) connection.Close();
            }
        }


        internal static void delete(string TableName, string colId, int id)
        {
            MySqlConnection connection = null;
            MySqlCommand cmd = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                string query = $"DELETE FROM {TableName} WHERE {colId}={id}";
                cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                printEx(cmd, ex);
            }
            finally
            {
                if (cmd != null) cmd.Dispose();
                if (connection != null) connection.Close();
            }
        }
        public static int getMaxId(string TableName, string ColId)
        {
            int toRet = 0;
            MySqlConnection connection = null;
            MySqlCommand cmd = null;
            MySqlDataReader reader = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                string query = $"SELECT MAX({ColId}) AS maxId FROM {TableName}";
                cmd = new MySqlCommand(query, connection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (!Convert.IsDBNull(reader["maxId"]))
                    {
                        toRet = Convert.ToInt32(reader["maxId"]);
                    }
                }

            }
            catch (Exception ex) { DBF.printEx(cmd, ex); }
            finally
            {
                if (reader != null) reader.Close();
                if (cmd != null) cmd.Dispose();
                if (connection != null) connection.Close();
            }
            return toRet;
        }
        internal static void printEx(MySqlCommand command, Exception ex)
        {
            Console.WriteLine(command.CommandText);
            Console.WriteLine(ex.Message);
        }








        internal static void convertVP(MySqlCommand command, string valuestring, object par)
        {
            
            command.Parameters.AddWithValue(valuestring,par);
        }
        internal static string timeFormat = "yyyy-MM-dd HH:mm:ss";
       

        
        

        

      





        



               
    }
}
