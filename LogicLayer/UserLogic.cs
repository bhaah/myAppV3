using WebApplication2.Singletons;
using WebApplication2.DatabaseLayer;

namespace WebApplication2.LogicLayer
{
    public class UserLogic
    {
        private List<User> _users;
        public List<User> Users { get { return _users; } }

        public UserLogic()
        {
            _users = new List<User>();
            UserController uc = new UserController();
            List<UserDTO> userDTOs = uc.getAllUsers();
            foreach(UserDTO userDTO in userDTOs)
            {
                User toAdd = new User(userDTO);
                _users.Add(toAdd);
            }
        }

        public User Register(string username,string email, string password)
        {

            User newUser = new User(username,email,password);
            _users.Add(newUser);
            BoardsOfUser.addUser(newUser.Email);
            return newUser;
        }

        public User login(string email,string password) 
        {
            foreach (User user in _users)
            {
                if (user.Email == email && user.login(password))
                {
                    return user;
                }
            }
            throw new ArgumentException("password or email is wrong!!");
           
        }

        public User GetUser(string username)
        {
            foreach(User user in _users)
            {
                if(user.UserName==username) return user;
            }
            throw new ArgumentException("there is no user like this");
        }

        public bool checkUser(string email,string password)
        {
            foreach(User user in _users)
            {
                if ((user.Email==email) && (user.Password==password)) return true;
            }
            return false;
        }
    }
}
