using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Heimdall.Gateway.Core
{
    public interface ILocateService
    {
        Task<IGatewayService> Find(string route);
        Task<IGatewayService> Find(string domain, string route);
    }
}
