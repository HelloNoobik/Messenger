using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using SharedLibrary;

namespace Server
{
    class Program
    {
        static Socket socket;
        static IPEndPoint endPoint;
        static ResponseDispatcher dispatcher;
        static Data.MessTestEntities db;
        static void Main(string[] args)
        {
            db = new Data.MessTestEntities();
            dispatcher = new ResponseDispatcher();
            dispatcher.Add(Auth);
            dispatcher.Add(Register);

            endPoint = new IPEndPoint(IPAddress.Any, 1000);
            socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endPoint);
            socket.Listen(128);

            Channel channel = new Channel(socket.Accept());

            while (true)
            {
                try
                {
                    Pocket request = channel.Recieve();
                    Pocket response = dispatcher.Execute(request.Action,request);
                    if (response != null) channel.Send(response);
                }
                catch (Exception ex) 
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        [Route("Auth")]
        static Pocket Auth(Pocket pocket) 
        {
            string login = pocket.Message[0] as string;
            string pass = pocket.Message[1] as string;

            Pocket response = new Pocket();
            if (db.Users.Where(c => c.Login == login && c.Password == pass).Count() == 1)
            {
                response.Message.Add(true);
            }
            else response.Message.Add(false);
            return response;
        }

        [Route("Register")]
        static Pocket Register(Pocket pocket) 
        {
            string login = pocket.Message[0] as string;
            string pass = pocket.Message[1] as string;

            Pocket response = new Pocket();
            if (db.Users.Where(c => c.Login == login && c.Password == pass).Count() == 0)
            {
                Data.User user = new Data.User()
                {
                    Login = login,
                    Password = pass,
                };
                db.Users.Add(user);
                db.SaveChanges();

                response.Message.Add(true);
            }
            else response.Message.Add(false);
            return response;
        }
    }
}
