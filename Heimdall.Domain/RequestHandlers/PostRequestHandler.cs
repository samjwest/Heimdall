using Heimdall.Gateway.Domain.Requests;
using Heimdall.Gateway.Core;

namespace Heimdall.Gateway.Domain.RequestHandlers
{
    public class PostRequestHandler : RequestHandlerBase
    {
        public PostRequestHandler(ILocateService locator, IRequestBuilder requestBuilder) : base(locator, requestBuilder)
        {

        }
    }
}