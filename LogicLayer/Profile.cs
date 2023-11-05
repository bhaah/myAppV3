using myFirstAppSol.DatabaseLayer;

namespace myFirstAppSol.LogicLayer
{
    public class Profile
    {
        private string _email;
        private int _coins;
        private List<string> _ownedAvatars;
        private string _currAvatar;
        private ProfileDTO pdto;
        private List<DateTime> _addDay;

        //public fields ----------------
        public string Email
        {
            get { return _email; }
        }
        public int Coins
        {
            get { return _coins; }
            
        }
        public string[] OwnedAvatars
        {
            get { return _ownedAvatars.ToArray(); }
            
        }
        public string CurrentAvatar
        {
            get { return _currAvatar; }
            set { 
                pdto.CurrentAvatar = value;
                _currAvatar = value;
            }
        }

        // constructor's ==========================

        public Profile(string email)
        {
            Console.WriteLine("hi from profile const with persist :" + email);
                _email= email;
                _coins = 40;
                _ownedAvatars = new List<string>();
                _addDay= new List<DateTime>();
                pdto= new ProfileDTO(email,_coins,_ownedAvatars,null,_addDay,true);
        }


        public Profile(ProfileDTO profileDto)
        {
            pdto= profileDto;
            _email= profileDto.Email;
            _currAvatar= profileDto.CurrentAvatar;
            _coins= profileDto.Coins;
            _ownedAvatars = profileDto.OwnedAvatars;
            _addDay= profileDto.dateTimes;
        }
        //==============================

        /// <summary>
        /// purchasing avatar 
        /// </summary>
        /// <param name="avatar"></param>
        /// <returns    ></returns>
        public bool purchase(string avatar)
        {
            if (avatar == null || !Avatars.AvatarSales.ContainsKey(avatar)) return false;
            if((_ownedAvatars == null || _ownedAvatars.Count==0) || !_ownedAvatars.Contains(avatar))
            {
                int sale= Avatars.AvatarSales[avatar];
                if(_coins>=sale)
                {
                    pdto.OwnedAvatars.Add(avatar);
                    pdto.Coins = _coins-sale;
                    _coins-=sale;
                    return true;
                }
                else return false;
            }
            return false;
        }


        /// <summary>
        /// add the coins, if count == true its check how many times they added in the same day (maximum 3)
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="count"></param>
        public void addCoins(int amount,bool count)
        {
            if(count && checkAddOption())
            {
                pdto.Coins = _coins+amount;
                _coins+=amount;
                
            }
            else
            {
                if(!count)
                {
                    pdto.Coins = _coins + amount;
                    _coins += amount;
                }
            }
        }

        /// <summary>
        /// set avatar from owned
        /// </summary>
        /// <param name="avatar"></param>
        public bool setAvatar(string avatar)
        {
            if (_ownedAvatars.Contains(avatar))
            {
                CurrentAvatar= avatar;
                return true;
            }
            return false;
        }


        public void deleteProfile()
        {
            pdto.delete();
        }

        // private fun's ================
        private bool checkAddOption()
        {
            if(_addDay != null && _addDay.Count > 2 && checkLastThree())
            {
                return true;
            }
            return false;
        }

        private bool checkLastThree()
        {
            bool done = false;
            DateTime toAdd= DateTime.Now;
            DateTime toRemove= DateTime.Now;
            foreach(DateTime date in _addDay)
            {
                if(date.Day != toAdd.Day)
                {
                    toRemove= date;
                    done= true;
                }
            }
            if(done)
            {
                _addDay.Remove(toRemove);
                _addDay.Add(toAdd);

            }
            return done;
        }
    }
}
