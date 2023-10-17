using myFirstAppSol.DatabaseLayer;

namespace myFirstAppSol.LogicLayer
{
    public class MessageLogic
    {
        private EMessageController emc = new EMessageController();
        private RMessageController rmc= new RMessageController();
        private Dictionary<string, List<Message>> Emessges; // email : id of message

        public MessageLogic() {
        Emessges= new Dictionary<string, List<Message>>();
        }

        //the random messages =========================================

        public Message getRandomMessage()
        {
            List<RMessageDTO> msgs = new List<RMessageDTO>();
            msgs = rmc.getRMessages();
            int size = msgs.Count;
            RMessageDTO[] msgsArray = msgs.ToArray();
            Random random = new Random();
            int randomIndex = random.Next(0, size);
            return new Message(msgsArray[randomIndex].Content);
        }
        public void setRandomMessage(string content) 
        {
            int id = rmc.getMaxId()+1;
            RMessageDTO rm = new RMessageDTO(id,content,true);
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
            List<EMessageDTO> lst = emc.getForEmail(email);
            List<Message> msgs = new List<Message>();
            foreach (EMessageDTO emdto in lst)
            {
                Message m = new Message(emdto);
                msgs.Add(m);
                Console.WriteLine(m.Content);
            }
            
           
            return msgs.ToArray();
        }
    }
}
