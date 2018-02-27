using System;
using System.Collections.Generic;
using System.Text;
using Heimdall.Gateway.Registry.Models;

namespace Heimdall.Gateway.ServiceDiscovery
{
    public static class GatewayServiceExtensions
    {
        public static GatewayService WithRoute(this GatewayService service, string route)
        {
            service.IdentifiedRoute = route;
            return service;
        }

        public static GatewayService AddEndpoint(this GatewayService service, ServiceEndpoint serviceEndpoint)
        {
            //service.Uri = serviceEndpoint.ServiceUri;
            service.IdentifiedRoute = serviceEndpoint.Route.Url;
            return service;
        }

        public static GatewayService WithEndpoints(this GatewayService service, List<ServiceEndpoint> serviceEndpoints)
        {
            service.DiscoveredEndpoints = serviceEndpoints;
            return service;
        }
    }
}
