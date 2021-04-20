using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    [Serializable]
    public class Chat
    {
        public int ChatId { get; private set; }
        public int CreatorId { get; private set; }
        public string Creator { get; private set; }
        public List<Message> Messages { get; private set; }

        public Chat(int chatId, int creatorId, string creator) 
        {
            ChatId = chatId;
            CreatorId = creatorId;
            Creator = creator;

            Messages = new List<Message>();
        }
    }
}
