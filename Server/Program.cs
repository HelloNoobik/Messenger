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
using System.Security.Cryptography;

namespace Server
{
    partial class Program
    {
        static IPEndPoint endPoint;
        static Socket socket;
        static ResponseDispatcher dispatcher;
        static Data.MessEntities db;
        static Email email;
        static ServerUser user;
        static void Main(string[] args)
        {
            endPoint = new IPEndPoint(IPAddress.Any, 1000);
            socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            dispatcher = new ResponseDispatcher();
            db = new Data.MessEntities();
            email = new Email(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Email.txt");

            Console.WriteLine("Запущен обработчик команд");
            
            socket.Bind(endPoint);
            socket.Listen(128);
            AcceptLoopAsync();
            Console.WriteLine("Запущен сервер\nОжидание подключений");

            while (true) 
            { 
            
            }
        }

        static async void AcceptLoopAsync() 
        {
            await Task.Run(() => 
            {
                while (true) 
                {
                    try
                    {
                        Socket client = socket.Accept();
                        Console.WriteLine("Новое подключение");
                        RecieveSendLoopAsync(new ServerUser(client));
                    }
                    catch (Exception ex) 
                    {
                        if(ex.InnerException is SocketException) Console.WriteLine("Соединение потеряно");
                        else Console.WriteLine($"{ex}");
                    }
                }
            });
        }

        static async void RecieveSendLoopAsync(ServerUser client) 
        {
            await Task.Run(() => 
            {
                while (true)
                {
                    try
                    {
                        Pocket request = client.Recieve();
                        Console.WriteLine("Поступил запрос: " + request.Action);
                        request.Message.Add(client.GetChannel());
                        Pocket response = dispatcher.Execute(request.Action,request);
                        if (response != null)
                        {
                            client.Send(response);
                            Console.WriteLine("Ответ сформирован и отправлен");
                        }
                    }
                    catch (Exception ex) 
                    {
                        if (ex.InnerException is SocketException) Console.WriteLine("Соединение потеряно");
                        else Console.WriteLine($"{ex}");
                        break;
                    }
                }    
            });
        }
    }
}
