using myFirstAppSol.LogicLayer;

namespace myFirstAppSol.DatabaseLayer
{
    public class ProfileDTO
    {
        private string _email;
        private int _coins;
        private List<string> _ownedAvatars;
        private string _currAvatar;
        private List<DateTime> _addDays;

        private bool isPersisted;
        private ProfileController pCon=new ProfileController();


        public string Email
        {
            get { return _email; } 
        }
        public int Coins
        {
            get { return _coins; }
            set { 
                Console.WriteLine("we are in he set coins and the value is :"+value);
                if(!isPersisted)
                {
                    persist();
                }
                Console.WriteLine("bedore updaing");
                pCon.updateCoins(Email, value);
                _coins = value;
            }
        }
        public List<string> OwnedAvatars
        {
            get { return _ownedAvatars; }
            set {
                if (!isPersisted)
                {
                    persist();
                }

                pCon.updateOwnedAvatars(Email, value);
                _ownedAvatars = value; }
        }
        public string CurrentAvatar
        {
            get { return _currAvatar; }
            set {
                if (!isPersisted)
                {
                    persist();
                }
                pCon.updateCurrAvatar(Email, value);
                _currAvatar = value; }
        }

        public List<DateTime> dateTimes {
            get { return _addDays; }
            set {
                if (!isPersisted)
                {
                    persist();
                }
                pCon.updateAddedDates(Email, value);
                _addDays = value; 
            }
        }


        public ProfileDTO(string email,int coins,List<string> ownedAvatars,string currAvatar,List<DateTime> addedDays,bool toPersist)
        {
            _email = email;
            _coins = coins;
            _ownedAvatars = ownedAvatars;
            _currAvatar = currAvatar;
            _addDays = addedDays;
            if (toPersist) persist();
            else isPersisted= true;
        }

        public void delete()
        {
            pCon.delete(Email);
        }

        private void persist()
        {
            pCon.Insert(this);
            isPersisted= true;
        }

    }
}
