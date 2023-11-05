using Npgsql;
using WebApplication2.DatabaseLayer;

namespace myFirstAppSol.DatabaseLayer
{
    public class ProfileController : dbController<ProfileDTO>
    {

        private const string tableName = "Profiles";
        private const string ColEmail = "Email";
        private const string ColCoins = "Coins";
        private const string ColOwnedAvatars = "OwnedAvatars";
        private const string ColCurrAvatar="CurrAvatar";
        private const string ColAddedDates = "AddedDates";


        public ProfileController()
        {

        }


        public override List<ProfileDTO> getDTO(NpgsqlDataReader reader)
        {
            List<ProfileDTO> result = new List<ProfileDTO>();
            while(reader.Read())
            {
                List<string> ownedAvatars = ((string[])reader[ColOwnedAvatars]).ToList();
                List<string> addedDays = ((string[])reader[ColAddedDates]).ToList();
                List<DateTime> parmDates = new List<DateTime>();
                foreach(string date in addedDays)
                {
                    parmDates.Add(DateTime.Parse(date));
                }
                ProfileDTO toAdd = new ProfileDTO((string)reader[ColEmail], Convert.ToInt32(reader[ColCoins]), ownedAvatars, (string)reader[ColCurrAvatar], parmDates, false);
                result.Add(toAdd);
            }
            return result;
        }



        public void Insert(ProfileDTO pdto)
        {
            Dictionary<string, object> map = new Dictionary<string, object>
            {
                {ColEmail, pdto.Email},
                {ColCoins,pdto.Coins},
                {ColCurrAvatar,pdto.CurrentAvatar },
                {ColOwnedAvatars,pdto.OwnedAvatars.ToArray()},
                {ColAddedDates,pdto.dateTimes.Select(x=>x.ToString(DBF.timeFormat)).ToArray()},
            };
            NpgsqlConnection connection = null;
            NpgsqlCommand cmd = null;
            try
            {
                connection = new NpgsqlConnection("Host=dpg-ckp7srnkc2qc73dooufg-a.oregon-postgres.render.com;Port=5432;Database=myappdatabaseonrender;User Id=myappdatabaseonrender_user;Password=ilbicIliuWUhAEoIj9Ab8yS5DaFtFiOH;");
                connection.Open();
                
                string sqlQuery = $"INSERT INTO {tableName} ({ColEmail}, {ColCoins}, {ColCurrAvatar}, {ColOwnedAvatars},{ColAddedDates}) VALUES ('{pdto.Email}','{pdto.Coins}','{pdto.CurrentAvatar}', @ownedAvatars, @addedDates);";                           
                cmd = new NpgsqlCommand(sqlQuery, connection);
                cmd.Parameters.AddWithValue("@ownedAvatars", map[ColOwnedAvatars]);
                cmd.Parameters.AddWithValue("@addedDates", map[ColAddedDates]);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                DBF.printEx(cmd, ex);
            }
            finally
            {
                if (cmd != null) { cmd.Dispose(); }
                if (connection != null) { connection.Close(); }

            }
        }


        public ProfileDTO getEmailProfile(string email)
        {
            List<ProfileDTO> list =DBF.getDTOs(tableName, this, $"WHERE {ColEmail}='{email}'");
            foreach(ProfileDTO p in list) { return p; }
            
            return null;  
            
        }



        // delete query ===============

        public void delete(string email)
        {
            DBF.delete(tableName,ColEmail, email);
        }


        // update querys ==============

        public void updateCoins(string email,int coins)
        {
            DBF.Update(tableName,ColCoins,coins,ColEmail,email);
        }

        public void updateOwnedAvatars(string email,List<string> avatars)
        {
            DBF.Update(tableName,ColOwnedAvatars,avatars.ToArray(),ColEmail,email);
        }

        public void updateCurrAvatar(string email,string avatar)
        {
            DBF.Update(tableName,ColCurrAvatar,avatar,ColEmail,email);

        }
        public void updateAddedDates(string email,List<DateTime> value)
        {
            string[] toUpdate = value.Select(x => x.ToString(DBF.timeFormat)).ToArray();
            DBF.Update(tableName,ColAddedDates,toUpdate,ColEmail,email);
        }

    }
}
