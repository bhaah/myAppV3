namespace myFirstAppSol.DatabaseLayer
{
    public class EMessageDTO
    {
        private int _id;
        private string _content;
        private string _email;
        private DateTime _time;

        private bool isPersised;
        private EMessageController mc = new EMessageController();

        public int Id
        {
            get { return _id; } 
        }
        public string Content
        {
            get { return _content; }
           
        }
        public string Email
        {
            get { return _email; }
        }
        public DateTime Time
        {
            get { return _time; }
        }


        public EMessageDTO(int id, string content, string email, DateTime time, bool toPersised)
        {
            _id = id;
            _content = content;
            _email = email;
            _time = time;
            if(toPersised)
            {
                persist()
            }
            else
            {
                isPersised= true;
            }
            
        }

        public void persist()
        {
            mc.Insert(this);
            isPersised= true;
        }
    }
}
