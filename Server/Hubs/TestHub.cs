using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace RemoteOrganControl.Server.Hubs
{
    public class TestHub : Hub
    {
        private static int counter;
        public async Task SendMessage(string user, string message)
        {
            Console.WriteLine($"Received a message: {message}");
            counter++;
            Console.Write(counter);
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}