using System;
using SharedLibrary;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Client
{
    class Program
    {
        static RequestDispatcher dispatcher;
        static Socket socket;
        static IPEndPoint endPoint;
        static Channel channel;
        static void Main(string[] args)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "Update.exe"))) File.Delete("Update.exe");

            Console.Title = "Client.exe";

            Pocket pocket = new Pocket("Auth");
            Console.WriteLine(pocket.Signature);

            dispatcher = new RequestDispatcher();
            dispatcher.Add(Hello);
            dispatcher.Add(Auth);
            dispatcher.Add(Register);
            dispatcher.Add(RestoreAccess);
            dispatcher.Add(Update);
            endPoint = new IPEndPoint(IPAddress.Loopback, 1000);
            socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endPoint);

            channel = new Channel(socket);

            while (true)
            {
                try
                {
                    string input = Console.ReadLine();
                    dispatcher.Execute(input);
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
        }

        [Route("Hello")]
        static void Hello() 
        {
            Console.WriteLine("Запрос Приветствия");
            Pocket pocket = new Pocket("Hello");
            channel.Send(pocket);
            Pocket response = channel.Recieve();

            if ((bool)response.Message[0] == true)
            {
                Console.WriteLine($"Привет, {(string)response.Message[1]}");
            }
            else Console.WriteLine("Требуется авторизация");
        }

        [Route("Auth")]
        static void Auth()
        {
            Console.WriteLine("Запрос Авторизации");
            Pocket pocket = new Pocket("Auth");
            Console.Write("Логин: ");
            pocket.Message.Add(Console.ReadLine());
            Console.Write("Пароль: ");
            pocket.Message.Add(Console.ReadLine());
            channel.Send(pocket);
            Pocket response = channel.Recieve();
            Console.WriteLine((bool)response.Message[0]);
        }

        [Route("Register")]
        static void Register()
        {
            Console.WriteLine("Запрос Регистрации");
            Pocket pocket = new Pocket("Register");
            Console.Write("Логин: ");
            pocket.Message.Add(Console.ReadLine());
            Console.Write("Пароль: ");
            pocket.Message.Add(Console.ReadLine());
            Console.Write("Почта: ");
            pocket.Message.Add(Console.ReadLine());
            channel.Send(pocket);
            Pocket response = channel.Recieve();
            Console.WriteLine((bool)response.Message[0]);
        }

        [Route("RestoreAccess")]
        static void RestoreAccess() 
        {
            Console.WriteLine("Запрос восстановления пароля");
            Pocket pocket = new Pocket("RestoreAccess");
            Console.Write("Логин: ");
            pocket.Message.Add(Console.ReadLine());
            channel.Send(pocket);
            Pocket response = channel.Recieve();
            Console.WriteLine((bool)response.Message[0]);
            if ((bool)response.Message[0] == true) 
            {
                Console.WriteLine("Письмо отправлено вам на почту");
                Console.Write("Введите код из письма: ");
                pocket.Message[0] = Console.ReadLine();
                channel.Send(pocket);
                response = channel.Recieve();
                Console.WriteLine((bool)response.Message[0]);

                if ((bool)response.Message[0] == true) 
                {
                    Console.Write("Введите новый пароль: ");
                    pocket.Message[0] = Console.ReadLine();
                    channel.Send(pocket);
                    response = channel.Recieve();
                    Console.WriteLine((bool)response.Message[0]);

                }
            }
        }
        
        [Route("Update")]
        static void Update() 
        {
            //string path = Environment.GetCommandLineArgs()[0];
            //string filename = path.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries).Last();
            //Console.WriteLine("Запрос обновления");



            //Pocket pocket = new Pocket("Update");
            //channel.Send(pocket);
            //Pocket response = channel.Recieve();
            //File.WriteAllBytes("new_Client.exe", response.Message[0] as byte[]);
            //File.WriteAllLines("update.bat", bat);
            //Process.Start("update.bat");
        }
    }
}
