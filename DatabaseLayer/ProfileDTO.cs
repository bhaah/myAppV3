namespace myFirstAppSol.DatabaseLayer
{
    public class ProfileDTO
    {
        private string _email;
        private int _coins;
        private List<string> _ownedAvatars;
        private string _currAvatar;

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



    }
}
