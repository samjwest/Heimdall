using System;
using System.Collections.Generic;
using System.Text;

namespace Heimdall.Gateway.Registry.Models
{
    public class Route
    {
        public Route(string value)
        {
            Url = value;
        }
        
        public string Url { get; set; }
    }
}
