using MySql.Data.MySqlClient;

namespace myFirstAppSol.DatabaseLayer
{
    public abstract class dbController<T>
    {


        public abstract List<T> getDTO(MySqlDataReader reader);
    }
}
