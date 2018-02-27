using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Heimdall.Gateway.ServiceDiscovery.Clients
{
    
    public class ClientPool : IClientPool
    {
        protected Dictionary<Uri, HttpClient> _availableClients;

        public ClientPool()
        {
            _availableClients = new Dictionary<Uri, HttpClient>();
        }

        public void Add(Uri uri, HttpClient client)
        {
            _availableClients.Add(uri, client);
        }

        public HttpClient Fetch(Uri uri)
        {
            HttpClient client = null;
            _availableClients.TryGetValue(uri, out client);
            return client;
        }
    }
}
