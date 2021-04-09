using System;
using System.Net;
using System.Net.Sockets;

namespace SharedLibrary
{
    public class Channel
    {
        NetworkStream networkStream;

        public Channel(Socket socket) 
        {
            networkStream = new NetworkStream(socket, true);
        }

        public void Send(Pocket pocket) 
        {
            byte[] body = pocket.ToByteArray();
            byte[] head = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(body.Length));

            networkStream.Write(head, 0, head.Length);
            networkStream.Write(body, 0, body.Length);
        }

        public Pocket Recieve() 
        {
            byte[] head = ReadBytes(4);
            int length = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(head,0));
            byte[] body = ReadBytes(length);
            return Pocket.FromByteArray(body);
        }

        private byte[] ReadBytes(int countOfBytes) 
        {
            byte[] buffer = new byte[countOfBytes];
            int bytesRecieved = 0;
            while (bytesRecieved != countOfBytes) 
            {
                int bytesRead = networkStream.Read(buffer, 0, countOfBytes - bytesRecieved);
                bytesRecieved += bytesRead;
            }
            return buffer;
        }
    }
}
