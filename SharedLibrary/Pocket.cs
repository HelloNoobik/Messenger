using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Net;

namespace SharedLibrary
{
    [Serializable]
    public class Pocket
    {
        public string Action { get; set; }
        public string Signature { get; set; }
        public List<object> Message { get; set; }
        public Pocket() 
        {
            Message = new List<object>();
        }
        public Pocket(string action) : this()
        {
            Action = action;
            Signature = $"{Environment.MachineName}/{Environment.UserName}/{Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(c => c.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First().ToString()}";
        }
        public byte[] ToByteArray() 
        {
            using (MemoryStream memory = new MemoryStream()) 
            {
                BinaryFormatter binary = new BinaryFormatter();
                binary.Serialize(memory, this);
                return memory.ToArray();
            }
        }
        static public Pocket FromByteArray(byte[] arr) 
        {
            using (MemoryStream memory = new MemoryStream()) 
            {
                BinaryFormatter binary = new BinaryFormatter();
                memory.Write(arr,0,arr.Length);
                memory.Seek(0, SeekOrigin.Begin);
                return binary.Deserialize(memory) as Pocket;
            }
        }
    }
}
