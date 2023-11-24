using WebApplication2.LogicLayer.BoardFolder;
using WebApplication2.LogicLayer;

namespace WebApplication2.Singletons
{
    public class BoardsOfUser
    {
        private static Dictionary<string, BoardLogic> userBoards;
        public static BoardLogic getUserBoards(string email)
        {
            if(userBoards == null)
            {
                userBoards= new Dictionary<string, BoardLogic>();
                List<User> users = Users.UserLogic.Users;
                foreach(User user in users)
                {
                    Console.WriteLine("RAM:we must add this user * " + user.Email + " * to the RAM");
                    userBoards.Add(user.Email,new BoardLogic(user.Email));
                    Console.WriteLine("+ "+user.Email+" addd to RAM");
                }
            }
            if(!userBoards.ContainsKey(email))
            {
                throw new Exception("there is no user with email:" + email);
            }
            return userBoards[email];
        }
        public static void addUser(string email)
        {
            if(userBoards==null)
            {
                userBoards= new Dictionary<string, BoardLogic>();
                List<User> users = Users.UserLogic.Users;
                foreach (User user in users)
                {
                    userBoards.Add(user.Email, new BoardLogic(user.Email));
                }
            }
            else
            {
                userBoards.Add(email, new BoardLogic(email));
            }
            
        }
        public static void setBoardLoic(BoardLogic bl,string email)
        {
            userBoards[email] = bl;
        }


    }
}
