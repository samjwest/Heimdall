using System;
using System.Net.Http;
using System.Text;
using Heimdall.Gateway.Registry.Models;
using Heimdall.Gateway.ServiceDiscovery.Extensions;

namespace Heimdall.Gateway.ServiceDiscovery.Clients
{
    public class ClientFactory : IClientFactory
    {
        private readonly ClientPool _clientPool = new ClientPool();
        private static readonly object singleUse = new object(); 

        public ClientFactory() { }

        /// <summary>
        /// Get an instance of an HttpClient to service a request.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public HttpClient Create(ServiceEndpoint endpoint) 
        {
            return Create(endpoint, null);
        }

        /// <summary>
        /// Get an instance of an HttpClient to service a request. Optionally, take a preconfigured HttpMessageHandler
        /// which will be disposed of after the request is processed.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public HttpClient Create(ServiceEndpoint endpoint, HttpMessageHandler handler)
        {
            if (endpoint == null || endpoint.IsValid() == false)
                return null;

            HttpClient createdClient;

            lock(singleUse)
            {
                createdClient = _clientPool.Fetch(endpoint.BaseAddress);
                if (createdClient == null)
                {
                    createdClient = BuildClient(endpoint.BaseAddress);
                    _clientPool.Add(endpoint.BaseAddress, createdClient);
                }
            }
            return createdClient;
        }

        /// <summary>
        /// Responsible for the configuration of a new HttpClient when one is not found in the existing pool of clients
        /// </summary>
        /// <param name="baseAddress"></param>
        /// <returns></returns>
        private HttpClient BuildClient(Uri baseAddress)
        {
            // TODO: Include future security headers or any other default configuration
            var client = new HttpClient().WithBase(baseAddress);

            return client;
        }
    }
}
