using Consul;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Heimdall.Gateway.Core;
using Heimdall.Gateway.Core.Extensions;
using Heimdall.Gateway.Registry.Models;

namespace Heimdall.Gateway.Registry.Clients
{
    /// <summary>
    /// This is an IServiceRegistry implementation of the Consul OSS service registry
    /// Consul provides service discovery, health checks, key/value storage, and a scalable 
    /// architecture with support for multiple data centers. See https://www.consul.io for more info.
    /// </summary>
    public class ConsulRegistryClient : IServiceRegistry
    {
        protected ConsulClient _consulClient;
        public ConsulRegistryClient(ConsulClient consulClient)
        {
            Name = "Consul Registry";
            _consulClient = consulClient;
        }

        public string Name { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<List<ServiceEndpoint>> FindByName(string name)
        {
            //List<Uri> serverUrls = new List<Uri>();
            //return new List<ServiceEndpoint>();

            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        public async Task<List<ServiceEndpoint>> FindByRoute(string route)
        {
            //List<Uri> serverUrls = new List<Uri>();

            //var services = _consulClient.Agent.Services().Result.Response;
            //foreach (var service in services)
            //{
            //    //service.Value.

            //    var isSchoolApi = service.Value.Tags.Any(t => t == "School") &&
            //                      service.Value.Tags.Any(t => t == "Students");
            //    if (isSchoolApi)
            //    {
            //        var serviceUri = new Uri($"{service.Value.Address}:{service.Value.Port}");
            //        serverUrls.Add(serviceUri);
            //    }
            //}

            //return new List<ServiceEndpoint>();

            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public async Task<List<ServiceEndpoint>> FindByTags(List<string> tags)
        {            
            List<ServiceEndpoint> endpointsList = new List<ServiceEndpoint>();
            ServiceEndpoint endpoint;

            if (tags.IsNullOrEmpty())
                return endpointsList;

            try
            {
                var services = await _consulClient.Agent.Services();
                foreach (var service in services.Response)
                {

                    if (service.Value.Tags.Intersect(tags, StringComparer.CurrentCultureIgnoreCase).Count() == tags.Count())
                    {
                        var address = (service.Value.Address.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase)) ? service.Value.Address : "http://" + service.Value.Address;
                        var host = new Uri(address);

                        endpoint = new ServiceEndpoint()
                        {
                            Host = new Host { Hostname = host.Host, Port = service.Value.Port },
                            Name = service.Value.Service
                        };
                        endpointsList.Add(endpoint);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new RegistryUnavailableException("Error occured retrieving services", ex);
            }

            return endpointsList;
        }
    }
}
