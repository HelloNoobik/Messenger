using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace Update
{
    class Program
    {
        static void Main(string[] args)
        {
            args = Environment.GetCommandLineArgs();
            string path = args[0];
            string oldName = args.Length > 1 ? args[1] + ".exe" : "Client.exe";
            string newName = "newClient.exe";

            if (File.Exists(Path.Combine(Environment.CurrentDirectory, oldName)) && File.Exists(Path.Combine(Environment.CurrentDirectory, newName)))
            {
                while (Process.GetProcessesByName(oldName).Length > 0)
                {
                    foreach (Process process in Process.GetProcessesByName(oldName)) { process.Kill(); }
                    Thread.Sleep(1000);
                }

                File.Delete(Path.Combine(Environment.CurrentDirectory, oldName));
                File.Move(Path.Combine(Environment.CurrentDirectory, newName), Path.Combine(Environment.CurrentDirectory, oldName));
            }
        }
    }
}
