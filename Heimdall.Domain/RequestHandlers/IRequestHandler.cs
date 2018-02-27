using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Heimdall.Gateway.Domain.RequestHandlers
{
    public interface IRequestHandler
    {
        Task<HttpResponseMessage> Process(HttpContext httpContext);
        Task<HttpResponseMessage> Process(HttpContext httpContext, HttpContent httpContent);
    }
}
