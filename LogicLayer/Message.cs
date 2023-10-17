using myFirstAppSol.DatabaseLayer;

namespace myFirstAppSol.LogicLayer
{
    public class Message
    {
        private int _id;
        private string _content;
        
        private bool _random;
        private string _email;
        private DateTime _time;
        private EMessageDTO emdto;

        public int Id { get { return _id; } }
        public string Email
        {
            get { return _email; }
        }
        public string Content
        {
            get { return _content; }
        }
        public DateTime Time
        {
            get { return _time; }
        }


        public Message(string content, DateTime time,string email)
        {
            _content= content;
            _time= time;
            _email= email;
            EMessageController emc = new EMessageController();
            _id=emc.getMaxId()+1;
            emdto = new EMessageDTO(_id, _content,email,time,true);

        }
        public Message(string content)
        {
            _content = content;
            _random = true;
        }

        public Message(EMessageDTO emdto)
        {

            _id = emdto.Id;
            _content = emdto.Content;
            _email= emdto.Email;
            _time = emdto.Time;
            _random = false;
            this.emdto = emdto;
        }


    }
}
