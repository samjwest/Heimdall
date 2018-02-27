using System;
using System.Collections.Generic;
using System.Text;

namespace Heimdall.Gateway.Registry.Models
{
    public class ServiceEndpoint
    {
        public ServiceEndpoint()
        {
            Host = new Host();
            Route = new Route("");
        }
 
        public Host Host { get; set; }

        public Route Route { get; set; }

        public string Prefix { get; set; }

        public string Scheme { get; set; }

        public string Name { get; set; }

        public Uri BaseAddress
        {
            get
            {
                return new Uri(GetBaseAddress());
            }
        }

        public Uri ServiceUri
        {
            get
            {
                var baseAddress = GetBaseAddress();
                return new Uri($"{baseAddress}{Route.Url}");
            }
        }

        public bool IsValid()
        {
            var result = EnsureBaseIsValid();
            return result;
        }

        private bool EnsureBaseIsValid()
        {
            if (string.IsNullOrEmpty(Scheme))
                Scheme = "Http";

            //throw new InvalidOperationException("Misconfigured service address");
            return true;
        }

        private bool HasPrefix()
        {
            return !string.IsNullOrEmpty(Prefix);
        }

        private string GetBaseAddress()
        {
            EnsureBaseIsValid();
            var domain = HasPrefix() ? Prefix + "/" : "";
            return $"{Scheme}://{Host}/{domain}";
        }
    }
}
