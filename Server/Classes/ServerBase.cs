using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary;
using System.Net.Sockets;
using System.Net;

namespace Server.Classes
{
    public abstract class ServerBase
    {
        protected Socket socket;
        protected Data.MessEntities db;
        protected IPEndPoint endPoint;
        protected bool isActive;
        protected ServerBase() 
        {
            endPoint = new IPEndPoint(IPAddress.Loopback, 1000);
            db = new Data.MessEntities();
            socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            isActive = true;
        }
    }
}
