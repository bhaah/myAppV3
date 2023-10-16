using myFirstAppSol.LogicLayer;
using System.Configuration;
using WebApplication2;

namespace myFirstAppSol
{
    public class MessageSender : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        //        MessageLogic ml = new MessageLogic();
        //        Message[] messagesToday = ml.getMessageOnDay(DateTime.Now);
        //        foreach (Message message in messagesToday)
        //        {
        //            if(message.Time.Hour == DateTime.Now.Hour && message.Time.Minute == DateTime.Now.Minute) { SendMessage(message); }
        //        }
        //        Console.WriteLine(DateTime.Now);
        //    }
        }

        private void SendMessage(Message m)
        {
        //    ChatHub ch = new ChatHub();
        //    ch.SendMessage(m.Email,new Response(m));
        }
    }


}
