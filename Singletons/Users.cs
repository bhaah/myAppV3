using WebApplication2.LogicLayer;

namespace WebApplication2.Singletons
{
    public class Users
    {
        private static UserLogic _ul;
        public static UserLogic UserLogic
        {
            get
            {
                if (_ul == null)
                {
                    _ul = new UserLogic();
                }
                return _ul;
            }
        }
    }
}
