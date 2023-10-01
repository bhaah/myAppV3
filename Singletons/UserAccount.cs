using WebApplication2.LogicLayer;

namespace WebApplication2.Singletons
{
    public class UserAccount
    {
        private static User _user;

        public static User Account {
            get { 
                if(_user == null)
                {
                    throw new ArgumentException("there is no login!!");
                }
                return _user;
            } 
            set
            {
                _user= value;
            }
        }
    }
}
