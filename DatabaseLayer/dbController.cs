using MySql.Data.MySqlClient;
using Npgsql;

namespace myFirstAppSol.DatabaseLayer
{
    public abstract class dbController<T>
    {


        public abstract List<T> getDTO(NpgsqlDataReader reader);
    }
}
