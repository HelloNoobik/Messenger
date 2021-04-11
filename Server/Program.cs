using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.Mail;
using SharedLibrary;
using Server.Classes;

namespace Server
{
    partial class Program
    {
        static IPEndPoint endPoint;
        static Socket socket;
        static ResponseDispatcher dispatcher;
        static Data.MessTestEntities db;
        static Email email;
        static ServerUser user;
        static void Main(string[] args)
        {
            endPoint = new IPEndPoint(IPAddress.Any, 1000);
            socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            dispatcher = new ResponseDispatcher();
            db = new Data.MessTestEntities();
            email = new Email(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Email.txt");

            Console.WriteLine("Запущен обработчик команд");
            
            socket.Bind(endPoint);
            socket.Listen(128);
            Console.WriteLine("Запущен сервер\nОжидание подключений");

            user = new ServerUser(socket.Accept());
            Console.WriteLine("Новое подключение");

            while (true)
            {
                try
                {
                    Pocket request = user.Recieve();
                    Console.WriteLine("Поступил запрос: " + request.Action);
                    request.Message.Add(user.GetChannel());
                    Pocket response = dispatcher.Execute(request.Action, request);
                    if (response != null)
                    { 
                        user.Send(response);
                        Console.WriteLine("Ответ сформирован и отправлен");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    user = new ServerUser(socket.Accept());
                    Console.WriteLine("Новое подключение");
                }
            }
        }
    }
}
