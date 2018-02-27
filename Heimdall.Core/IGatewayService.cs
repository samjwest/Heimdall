using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Heimdall.Gateway.Core
{
    public interface IGatewayService
    {
        Uri Uri { get; }
        string IdentifiedRoute { get; set; }
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }
}
