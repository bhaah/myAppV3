using myFirstAppSol.DatabaseLayer;
using myFirstAppSol.LogicLayer;
using WebApplication2.DatabaseLayer;
using WebApplication2.LogicLayer.BoardFolder;

namespace WebApplication2.LogicLayer
{
    public class User
    {
        private string _userName;
        private string _email;
        private string _password;
        private Profile _profile;
        private UserDTO userDTO;


        //geters
        public string UserName { get { return _userName; }}
        public string Email { get { return _email; }}
       
        public string Password { get { return _password; }}
        public Profile Profile { 
            get { return _profile; }
            set { _profile = value; }
        }

        //constructors 
        public User(string userName, string email, string password)
        {
            _userName = userName;
            _email = email+"@bhaa.com";
            _password = password;
            userDTO = new UserDTO(_email, password,userName,true);
            _profile = new Profile(_email);
        }

        public User(UserDTO userDTO)
        {
            _userName= userDTO.UserName;
            _email= userDTO.Email;
            _password= userDTO.Password;
            this.userDTO= userDTO;
            ProfileController profileController= new ProfileController();
            ProfileDTO pdto = profileController.getEmailProfile(_email);
            _profile = new Profile(pdto);
        }


        public bool login(string password)
        {
            return _password.Equals(password);
        }
        
        



    }
}
