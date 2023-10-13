namespace myFirstAppSol.LogicLayer
{
    public class Message
    {
        private int _id;
        private string _content;
        
        private bool _random;
        private string _email;
        private DateTime _time;

        public Message(string content, DateTime time,string email)
        {
            _content= content;
            _time= time;
            _email= email;

        }
        public Message(string content)
        {
            _content = content;
            _random = true;
        }



    }
}
