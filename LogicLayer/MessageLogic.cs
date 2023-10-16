using myFirstAppSol.DatabaseLayer;

namespace myFirstAppSol.LogicLayer
{
    public class MessageLogic
    {
        private EMessageController emc = new EMessageController();
        private Dictionary<string, List<Message>> Emessges; // email : id of message

        public MessageLogic() {
        Emessges= new Dictionary<string, List<Message>>();
        }

        //the random messages =========================================

        //public Message getRandomMessage()
        //{

        //}
        public void setRandomMessage(string content) 
        {
            
        }




        // the messages for email ====================================
        public void setEmailMessage(string content,string email,DateTime time)
        {
            Message em = new Message(content,time,email);
            if(Emessges.ContainsKey(email))
            {
                Emessges[email].Add(em);
            }
            else
            {
                Emessges.Add(email,new List<Message> { em});
            }
        }

        public Message[] getMessageOnDay(DateTime time)
        {
            List<Message> msgs = new List<Message>();
            List<EMessageDTO> eMessageDTOs= emc.getInDate(time);
            foreach(EMessageDTO emdto in eMessageDTOs)
            {
                Message toAdd = new Message(emdto);
                msgs.Add(toAdd);
            }
            return msgs.ToArray();
        }

        public Message[] getMessagesForEmail(string email)
        {
            List<Message> msgs = new List<Message>();
            if(Emessges.ContainsKey(email) )
            {
                msgs = Emessges[email];
            }
            return msgs.ToArray();
        }
    }
}
