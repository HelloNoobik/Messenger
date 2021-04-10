using System;
using System.Linq;
using SharedLibrary;

namespace Server
{
    partial class Program
    {
        [Route("Auth")]
        static public Pocket Auth(Pocket pocket)
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
        static public Pocket Register(Pocket pocket)
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
        static public Pocket RestoreAccess(Pocket pocket)
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

                string subject = "Восстановление доступа";
                string text = $"Код необходимый для восстановления доступа к учетной записи: {code}";

                Console.WriteLine("Отправка письма");
                email.Send(user.Email, text, subject);
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
