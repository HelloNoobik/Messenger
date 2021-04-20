using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    [Serializable]
    public class Message
    {
        public int MessageId { get; private set; }
        public int SenderId { get; private set; }
        public string Sender { get; private set; }
        public string MessageText { get; private set; }
        public DateTime Sended { get; private set; }
        public Message(int messageId, int senderId, string sender, string messageText, DateTime sended)
        {
            MessageId = messageId;
            SenderId = senderId;
            Sender = sender;
            MessageText = messageText;
            Sended = sended;
        }
    }
}
