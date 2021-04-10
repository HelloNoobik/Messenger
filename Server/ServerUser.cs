using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary;
using System.Net.Sockets;

namespace Server
{
    public class ServerUser
    {
        private Channel channel;
        public Data.User User;

        public ServerUser(Socket socket) 
        {
            channel = new Channel(socket);
        }

        public Channel GetChannel() => channel;
        public void Send(Pocket pocket) => channel.Send(pocket);
        public Pocket Recieve() => channel.Recieve();
    }
}
