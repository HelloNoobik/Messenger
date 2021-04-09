using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public class RouteAttribute : Attribute
    {
        public string Path { get; }

        public RouteAttribute(string path) 
        {
            Path = path;
        }
    }
}
