using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.Mail;
using SharedLibrary;

namespace Server
{
    class Program
    {
        static Socket socket;
        static IPEndPoint endPoint;
        static ResponseDispatcher dispatcher;
        static Data.MessTestEntities db;
        static ServerUser user;
        static void Main(string[] args)
        {
            db = new Data.MessTestEntities();
            dispatcher = new ResponseDispatcher();
            dispatcher.Add(Auth);
            dispatcher.Add(Register);
            dispatcher.Add(RestoreAccess);
            Console.WriteLine("Запущен обработчик команд");
            

            endPoint = new IPEndPoint(IPAddress.Any, 1000);
            socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endPoint);
            socket.Listen(128);
            Console.WriteLine("Запущен сервер");
            Console.WriteLine("Ожидание подключений");

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
                }
            }
        }

        [Route("Auth")]
        static Pocket Auth(Pocket pocket)
        {
            string login = pocket.Message[0] as string;
            string pass = pocket.Message[1] as string;

            Pocket response = new Pocket();
            Data.User User = db.Users.Where(c => c.Login == login && c.Password == pass).SingleOrDefault();
            if (User != null)
            {
                user.User = User;
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
            string email = pocket.Message[2] as string;

            Pocket response = new Pocket();
            if (db.Users.Where(c => c.Login == login && c.Password == pass).Count() == 0)
            {
                Data.User User = new Data.User()
                {
                    Login = login,
                    Password = pass,
                    Email = email,
                };
                db.Users.Add(User);
                db.SaveChanges();

                user.User = User;

                response.Message.Add(true);
            }
            else response.Message.Add(false);
            return response;
        }

        [Route("RestoreAccess")]
        static Pocket RestoreAccess(Pocket pocket)
        {
            Console.WriteLine("Восстановление доступа");
            string login = (string)pocket.Message[0];
            Channel channel = (Channel)pocket.Message[1];
            Console.WriteLine("Запрос от " + login);

            Pocket response = new Pocket();
            Data.User user = db.Users.Where(c => c.Login == login).SingleOrDefault();
            if (user != null)
            {
                Console.WriteLine("Пользоавтель найдет");
                Random rand = new Random();
                string code = rand.Next(10000, 99999).ToString();
                Console.WriteLine("Сформирован код восстановления " + code);
                response.Message.Add(true);

                MailAddress from = new MailAddress("messapp@mail.ru");
                MailAddress to = new MailAddress(user.Email);
                MailMessage message = new MailMessage(from, to);
                message.Subject = "Восстановление доступа";
                message.Body = $"Код необходимый для восстановления доступа к учетной записи: {code}";

                SmtpClient smtp = new SmtpClient("smtp.mail.ru", 25);
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("messapp@mail.ru", "aZ19g08f7dMvanB12Ek6");
                Console.WriteLine("Отправка письма");
                smtp.Send(message);
                Console.WriteLine("Письмо отправлено");

                channel.Send(response);
                Pocket request = channel.Recieve();
                Console.WriteLine("Получен код " + request.Message[0]);
                if (code == (string)request.Message[0]) 
                {
                    channel.Send(response);
                    request = channel.Recieve();
                    Console.WriteLine("Получен пароль " + (string)request.Message[0]);
                    user.Password = (string)request.Message[0];
                    db.SaveChanges();
                    Console.WriteLine("Пароль изменен");
                    return response;
                }
            }
            response.Message.Add(false);
            return response;
        }
    }
}
