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
                    userBoards.Add(user.Email,new BoardLogic(user.Email));
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
            }
            userBoards.Add(email, new BoardLogic(email));
        }
        public static void setBoardLoic(BoardLogic bl,string email)
        {
            userBoards[email] = bl;
        }


    }
}
