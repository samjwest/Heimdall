using System;
using System.Net.Http;
using System.Text;
using Heimdall.Gateway.Registry.Models;

namespace Heimdall.Gateway.ServiceDiscovery.Clients
{
    public interface IClientFactory
    {
        HttpClient Create(ServiceEndpoint endpoint); //Uri baseAddress);
    }
}
