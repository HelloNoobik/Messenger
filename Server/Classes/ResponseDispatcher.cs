using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary;
using Server;

namespace Server.Classes
{
    public class ResponseDispatcher
    {
        Dictionary<string, Func<Pocket, Pocket>> requests;

        public ResponseDispatcher()
        {
            requests = new Dictionary<string, Func<Pocket, Pocket>>();
            Load();
        }
        public void Add(Func<Pocket, Pocket> method)
        {
            string path = GetPath(method.Method);
            if (path != "") requests.Add(path, method);
        }
        public void Remove(string path)
        {
            Func<Pocket, Pocket> method;
            requests.TryGetValue(path, out method);
            if (method != null) requests.Remove(path);
        }
        public void Load() 
        {
            Add(Program.Hello);
            Add(Program.Auth);
            Add(Program.Register);
            Add(Program.RestoreAccess);
            Add(Program.Update);
        }
        public Pocket Execute(string path, Pocket pocket)
        {
            Func<Pocket, Pocket> method;
            requests.TryGetValue(path, out method);
            if (method != null) return method(pocket);
            else return null;
        }
        private string GetPath(MethodInfo method)
        {
            RouteAttribute route = method.GetCustomAttribute<RouteAttribute>();
            if (route != null) return route.Path;
            return "";
        }
    }
}
