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
            set { _coins = value; }
        }
        public List<string> OwnedAvatars
        {
            get { return _ownedAvatars; }
            set { _ownedAvatars = value; }
        }
        public string CurrentAvatar
        {
            get { return _currAvatar; }
            set { _currAvatar = value; }
        }

        public List<DateTime> dateTimes {
            get { return _addDays; }
            set { _addDays = value; }
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



        private void persist()
        {
            
        }

    }
}
