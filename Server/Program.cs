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

        static void CreateDialog()
        {
            string[] dictionary = new string[]{ "1" , "2" , "3" };

            int firstId = 1;
            int secondId = 3;

            Data.Chat chat = new Data.Chat()
            {
                UserId = firstId,
            };
            db.Chats.Add(chat);
            db.SaveChanges();
            int chatId = db.Chats.First().ChatId;
            Data.ChatMember member = new Data.ChatMember() { ChatId = chatId, UserId = secondId, };
            db.ChatMembers.Add(member);
            Random rand = new Random();
            for (int i = 0; i < 1000; i++) 
            {
                Data.Message chatMessage = new Data.Message()
                {
                    UserId = firstId,
                    ChatId = chatId,
                    SendedDateTime = DateTime.Now,
                    MessageText = dictionary[rand.Next(0, dictionary.Length)],
                };
                db.Messages.Add(chatMessage);

                Data.Message chatMessageSecond = new Data.Message()
                {
                    UserId = secondId,
                    ChatId = chatId,
                    SendedDateTime = DateTime.Now,
                    MessageText = dictionary[rand.Next(0, dictionary.Length)],
                };
                db.Messages.Add(chatMessageSecond);
            }
            db.SaveChanges();
            Console.WriteLine("Work done, my lord.");
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
                        request.Message.Add(client);
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
