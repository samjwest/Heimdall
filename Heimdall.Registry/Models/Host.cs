using System;
using System.Collections.Generic;
using System.Text;

namespace Heimdall.Gateway.Registry.Models
{
    public class Host
    {
        public Host() { }

        public Host(string hostname, int? port)
        {

        }

        public string Hostname { get; set; }

        public int? Port { get; set; }

        public override string ToString()
        {
            return Hostname + ((Port == null) ? "//" : ":" + Port.ToString());
        }
    }
}
