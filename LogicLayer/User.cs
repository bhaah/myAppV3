using WebApplication2.DatabaseLayer;
using WebApplication2.LogicLayer.BoardFolder;

namespace WebApplication2.LogicLayer
{
    public class User
    {
        private string _userName;
        private string _email;
        private string _password;
        private UserDTO userDTO;


        //geters
        public string UserName { get { return _userName; }}
        public string Email { get { return _email; }}
       
        public string Password { get { return _password; }}
       

        //constructors 
        public User(string userName, string email, string password)
        {
            _userName = userName;
            _email = email+"@bhaa.com";
            _password = password;
            userDTO = new UserDTO(_email, password,userName,true);
        }

        public User(UserDTO userDTO)
        {
            _userName= userDTO.UserName;
            _email= userDTO.Email;
            _password= userDTO.Password;
            this.userDTO= userDTO;
        }


        public bool login(string password)
        {
            return _password.Equals(password);
        }
        
        



    }
}
