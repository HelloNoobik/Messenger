using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using SharedLibrary;

namespace Client.Classes
{
    public class SocketClient
    {
        IPEndPoint ipEnd = new IPEndPoint(IPAddress.Loopback, 1000);
        Channel channel;
        public SocketClient() 
        {
            Socket socket = new Socket(ipEnd.AddressFamily,SocketType.Stream,ProtocolType.Tcp);
            socket.Connect(ipEnd);
            channel = new Channel(socket);
        }

        public bool AuthRequest(string login, string pass)
        {
            Pocket pocket = new Pocket("Auth");
            pocket.Message.Add(login);
            pocket.Message.Add(pass);

            channel.Send(pocket);
            Pocket response = channel.Recieve();

            bool result = (bool)response.Message[0];

            return result;
        }

        public bool RegisterRequest(string login, string password, string email)
        {
            Pocket pocket = new Pocket("Register");
            pocket.Message.Add(login);
            pocket.Message.Add(password);
            pocket.Message.Add(email);

            channel.Send(pocket);
            Pocket response = channel.Recieve();

            bool result = (bool)response.Message[0];
            return result;
        }

        public bool RestoreRequest(string login)
        {
            Pocket pocket = new Pocket("RestoreAccess");
            pocket.Message.Add(login);
            channel.Send(pocket);

            Pocket response = channel.Recieve();

            bool result = (bool)response.Message[0];
            return result;
        }

        public bool RestoreRequest(int code)
        {
            Pocket pocket = new Pocket("RestoreAccess");
            pocket.Message.Add(code);
            channel.Send(pocket);

            Pocket response = channel.Recieve();
            bool result = (bool)response.Message[0];
            return result;
        }

        public bool ChangePassword(string password)
        {
            Pocket pocket = new Pocket("RestoreAccess");
            pocket.Message.Add(password);
            channel.Send(pocket);

            Pocket response = channel.Recieve();
            bool result = (bool)response.Message[0];
            return result;

        }

        public List<Chat> GetChats()
        {
            Pocket pocket = new Pocket("GetChats");
            channel.Send(pocket);
            Pocket response = channel.Recieve();
            List<Chat> chats = (List<Chat>)response.Message[0];
            return chats;
        }
    }
}
