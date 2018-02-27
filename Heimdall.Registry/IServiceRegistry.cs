using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Heimdall.Gateway.Registry.Models;

namespace Heimdall.Gateway.Registry
{
    public interface IServiceRegistry
    {
        string Name { get; }
        Task<List<ServiceEndpoint>> FindByName(string name);
        Task<List<ServiceEndpoint>> FindByRoute(string route);
        Task<List<ServiceEndpoint>> FindByTags(List<string> tags);
    }
}
