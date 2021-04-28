using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using SharedLibrary.Net;
using SharedLibrary;

namespace Server.Classes
{
    public class ResponseDispatcherAsync
    {
        private Dictionary<string, Func<RequestPocket,Task<ResponsePocket>>> methods;

        public ResponseDispatcherAsync() 
        {
            methods = new Dictionary<string, Func<RequestPocket, Task<ResponsePocket>>>();
        }

        public void Add(Func<RequestPocket, Task<ResponsePocket>> method)
        {
            string path = GetPath(method.Method);
            if (path == null) throw new Exception("Path empty.");

            methods.Add(path, method);
        }

        public ResponsePocket Execute(RequestPocket pocket) 
        {
            Func<RequestPocket, Task<ResponsePocket>> method;

            methods.TryGetValue(pocket.Request.Action, out method);
            if (method == null) throw new ArgumentNullException("Path not route to method.");

            return method(pocket).Result;
        }

        private string GetPath(MethodInfo method) 
        {
            RouteAttribute route = method.GetCustomAttribute<RouteAttribute>();
            if (route == null) throw new ArgumentNullException("Route attribute not exist.");
                
            return route.Path;
        }
    }
}
