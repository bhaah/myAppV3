using Microsoft.AspNetCore.SignalR;
using WebApplication2.Singletons;
using WebApplication2;
using WebApplication2.LogicLayer;
using System.Text.Json;

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
        

        public async Task SendMessage(string email,Response res)
        {
            Dictionary<string, string> dict = Users.UserLogic.UserById;
            string jres = JsonSerializer.Serialize(res);
            Console.WriteLine(jres);
            try
            {
                Console.WriteLine(email+" : " + dict[email]);
                await Clients.Client(dict[email]).SendAsync("ReceiveMessage", jres);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
