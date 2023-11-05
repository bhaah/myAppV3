using WebApplication2.Singletons;
using WebApplication2.DatabaseLayer;
using myFirstAppSol.LogicLayer;

namespace WebApplication2.LogicLayer
{
    public class UserLogic
    {
        private List<User> _users;
        public List<User> Users { get { return _users; } }

        private Dictionary< string,string> _usersById;
        public Dictionary<string,string> UserById {
            get { return _usersById; }
            set { _usersById = value; }
        }

        public UserLogic()
        {
            Console.WriteLine("20");
            _users = new List<User>();
            Console.WriteLine("23");
            UserController uc = new UserController();
            Console.WriteLine("25");
            List<UserDTO> userDTOs = uc.getAllUsers();
            Console.WriteLine("27");
            _usersById = new Dictionary<string, string>();
            foreach(UserDTO userDTO in userDTOs)
            {
                Console.WriteLine(userDTO.Email);
                User toAdd = new User(userDTO);
                _users.Add(toAdd);
                _usersById.Add(toAdd.Email, null);

            }
        }

        public User Register(string username,string email, string password)
        {
            Console.WriteLine("we just enterd the register fun in UserLogic");
            User newUser = new User(username,email,password);
            _users.Add(newUser);
            BoardsOfUser.addUser(newUser.Email);
            _usersById.Add(email, null);
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
                if(user.UserName==username || user.Email == username) return user;
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


        // profile featurs ========================================================

        public void addCoins(string email,int amount,bool toCount)
        {
            User rec = GetUser(email);
            rec.Profile.addCoins(amount, toCount);
            
        }

        public bool purchase(string email,string avatar)
        {
            User req= GetUser(email);
            return req.Profile.purchase(avatar);
        }

        public bool setAvatar(string email,string avatar)
        {
            User rec = GetUser(email);
            return rec.Profile.setAvatar(avatar);
        }

        

    }
}
