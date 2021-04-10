using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace Server.Classes
{
    public class Email
    {
        const string SMTP_HOST = "smtp.mail.ru";
        const int SMTP_PORT = 25;
        const SmtpDeliveryMethod SMTP_DELIVERY_METHOD = SmtpDeliveryMethod.Network;

        private SmtpClient  smtp;
        private MailAddress from;
        private string password;

        public Email(string path) 
        {
            string[] file = File.ReadAllLines(path)[0].Split(';');
            
            smtp = new SmtpClient(SMTP_HOST, SMTP_PORT);
            smtp.DeliveryMethod = SMTP_DELIVERY_METHOD;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(file[0], file[1]);
  
            from = new MailAddress(file[0]);
        }

        public void Send(string email, string text, string subject = "") 
        {
            MailAddress to = new MailAddress(email);
            MailMessage message = new MailMessage(from,to);
            message.Subject = subject;
            message.Body = text;

            smtp.Send(message);
        }
    }
}
