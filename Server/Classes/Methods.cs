using System;
using System.Linq;
using System.IO;
using SharedLibrary;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Server
{
    partial class Program
    {
        [Route("Hello")]
        static public Pocket Hello(Pocket pocket) 
        {
            Pocket response = new Pocket();
            string signature = pocket.Signature + $"/{(pocket.Message.Last() as Channel).IP.Split(':')[0]}";
            Data.Session session = db.Sessions.SingleOrDefault(c => c.Signature == signature);
            if (session != null)
            {
                response.Message.Add(true);
                response.Message.Add(session.User.Login);
                (pocket.Message.Last() as ServerUser).User = session.User;
            }
            else response.Message.Add(false);
            return response;
        }
        [Route("Auth")]
        static public Pocket Auth(Pocket pocket)
        {
            string login = pocket.Message[0] as string;
            string pass = pocket.Message[1] as string;

            Pocket response = new Pocket();
            string signature = pocket.Signature + $"/{(pocket.Message.Last() as Channel).IP.Split(':')[0]}";
            Data.User User = db.Users.Where(c => c.Login == login && c.Password == pass).SingleOrDefault();
            if (User != null)
            {
                (pocket.Message.Last() as ServerUser).User = User;

                Data.Session session = db.Sessions.SingleOrDefault(c => c.Signature == signature);
                if (session != null)
                {
                    session.UserId = User.Id;
                    session.Signature = signature;
                }
                else
                {
                    session = new Data.Session()
                    {
                        UserId = User.Id,
                        Signature = signature,
                    };
                    db.Sessions.Add(session);
                }
                db.SaveChanges();

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
            string email = (pocket.Message[2] as string).Trim().ToLowerInvariant();

            Pocket response = new Pocket();
            if (!string.IsNullOrEmpty(email?.Trim()))
            {
                const string pattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                if (!Regex.Match(email, pattern).Success)
                {
                    response.Message.Add(false);
                    return response;
                }
            }

            string signature = pocket.Signature + $"/{(pocket.Message.Last() as Channel).IP.Split(':')[0]}";
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

                Data.Session session = db.Sessions.SingleOrDefault(c => c.Signature == signature);
                if (session != null)
                {
                    session.UserId = User.Id;
                    session.Signature = signature;
                }
                else
                {
                    session = new Data.Session()
                    {
                        UserId = User.Id,
                        Signature = signature,
                    };
                    db.Sessions.Add(session);
                }
                db.SaveChanges();

                (pocket.Message.Last() as ServerUser).User = User;

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

        [Route("GetChats")]
        static public Pocket GetChats(Pocket pocket) 
        {
            List<Chat> chats = new List<Chat>();
            Data.User user = db.Users.Find(1);

            IQueryable<Data.Chat> chatOwner =  db.Chats.Where(c => c.UserId == user.Id);
            IQueryable<Data.Chat> chatMember = db.ChatMembers.Where(c => c.UserId == user.Id).Select(с => с.Chat);

            Pocket response = new Pocket();
            if (chatOwner.Count() == 0 && chatMember.Count() == 0) 
            {
                response.Message.Add(false);
                return response;
            }

            Console.WriteLine("Работаю");
            foreach (Data.Chat chat in chatOwner) 
            {
                Chat Chat = new Chat(chat.ChatId, chat.UserId, chat.User.Login);
                foreach (Data.Message message in chat.Messages.Take(50).ToArray()) 
                {
                    Message Message = new Message(message.MessageId, message.UserId, message.User.Login, message.MessageText, message.SendedDateTime);
                    Chat.Messages.Add(Message);
                }
                chats.Add(Chat);
            }

            foreach (Data.Chat chat in chatMember) 
            {
                Chat Chat = new Chat(chat.ChatId, chat.UserId, chat.User.Login);
                foreach (Data.Message message in chat.Messages.Take(50).ToArray())
                {
                    Message Message = new Message(message.MessageId, message.UserId, message.User.Login, message.MessageText, message.SendedDateTime);
                    Chat.Messages.Add(Message);
                }
                chats.Add(Chat);
            }
            Console.WriteLine("Готово");
            response.Message.Add(chats);
            return response;
        }

        [Route("Update")]
        static public Pocket Update(Pocket pocket) 
        {
            byte[] file = File.ReadAllBytes("C:\\Users\\User\\source\\repos\\Messenger\\Client\\bin\\Debug\\Client.exe");
            Pocket response = new Pocket();
            response.Message.Add(file);
            return response;
        }
    }
}
