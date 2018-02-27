using System;
using System.Net.Http;

namespace Heimdall.Gateway.ServiceDiscovery.Clients
{
    public interface IClientPool
    {
        void Add(Uri uri, HttpClient client);
        HttpClient Fetch(Uri uri);
    }
}