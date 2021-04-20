using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using SharedLibrary;

namespace DebugClient
{
    public class RequestDispatcher
    {
        Dictionary<string, Action> requests;

        public RequestDispatcher()
        {
            requests = new Dictionary<string, Action>();
        }

        public void Add(Action method)
        {
            string path = GetPath(method.Method);
            if (path != "") requests.Add(path, method);
        }

        public void Remove(string path)
        {
            Action method;
            requests.TryGetValue(path, out method);
            if (method != null) requests.Remove(path);
        }

        public void Execute(string path)
        {
            Action method;
            requests.TryGetValue(path, out method);
            if (method != null) method();
        }

        private string GetPath(MethodInfo method)
        {
            RouteAttribute route = method.GetCustomAttribute<RouteAttribute>();
            if (route != null) return route.Path;
            return "";
        }
    }
}
