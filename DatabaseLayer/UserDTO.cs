namespace WebApplication2.DatabaseLayer
{
    public class UserDTO
    {
        private string _email;
        private string _password;
        private string _userName;


        //for database
        private bool _isPersisted;
        private UserController uc = new UserController();

        //geters and seters 
        public string Email
        {
            get { return _email; }
            set
            {
                if(_isPersisted)
                {
                    _email = value;
                   // uc.updateEmail(value);
                }
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                if (_isPersisted)
                {
                    _password = value;
                    //
                }
            }
        }
        public string UserName
        {
            get { return _userName; }
            set { }
        }


        public UserDTO(string email, string password,string userName,bool toPersist)
        {
            _email= email;
            _password= password;
            _userName= userName;
            if(toPersist & !_isPersisted)
            {
                persist();
            }
            else
            {
                _isPersisted = true;
            }
        }


        public void persist() 
        {
            uc.addUser(this);
            _isPersisted= true;
        }

    }
}
