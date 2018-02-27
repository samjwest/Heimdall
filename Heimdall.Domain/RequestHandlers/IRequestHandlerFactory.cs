using System.Net.Http;

namespace Heimdall.Gateway.Domain.RequestHandlers
{
    public interface IRequestHandlerFactory
    {
        IRequestHandler BuildHandler(HttpMethod httpVerb);
    }
}
