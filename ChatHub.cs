using Microsoft.AspNetCore.SignalR;
using WebApplication2.Singletons;
using WebApplication2;
using WebApplication2.LogicLayer;
using System.Text.Json;
using myFirstAppSol.LogicLayer;

namespace myFirstAppSol
{
    public sealed class ChatHub : Hub
    {
        private Dictionary<string, string> _usersConnctions=new Dictionary<string, string>();



        public async Task ConnectToServer(string email)
        {
            
            string id = Context.ConnectionId;
            Console.WriteLine(id);
            Dictionary<string ,string> dict = Users.UserLogic.UserById;
            if (dict != null)
            {
                if (dict.ContainsKey(email))
                {
                    dict[email] = id;
                }
                else
                {
                    dict.Add(email, id);
                }
            }
            else
            {
                dict= new Dictionary<string ,string>();
                dict.Add(email, id);
            }
            Users.UserLogic.UserById = dict;
        }
        
        public async Task listen(string email)
        {
            int start = 0;
            Random random = new Random();
            while(true)
            {
                MessageLogic ml = new MessageLogic();
                Message[] messagesToday = ml.getMessageOnDay(DateTime.Now);
                foreach (Message message in messagesToday)
                {
                    if (message.Time.Hour == DateTime.Now.Hour && message.Time.Minute == DateTime.Now.Minute) { SendMessage(email,new Response(message)); }
                }
                await Task.Delay(60000);
                if (start > 5 && random.Next(0, 100) < 50)
                {
                    Message toSent = ml.getRandomMessage();
                    if (toSent != null)
                    {
                        SendMessage(email, new Response(toSent));
                    }
                    start = 0;
                }
                else start++;
            }
        }
        public async Task SendMessage(string email,Response res)
        {
            Dictionary<string, string> dict = Users.UserLogic.UserById;
            string jres = JsonSerializer.Serialize(res);
            Console.WriteLine(jres);
            try
            {
                string id = dict[email];
                Console.WriteLine(email+" : " + dict[email]);
                await Clients.Client(id).SendAsync("ReceiveMessage", jres);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
