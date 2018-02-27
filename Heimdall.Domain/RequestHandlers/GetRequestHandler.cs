using Heimdall.Gateway.Domain.Requests;
using Heimdall.Gateway.Core;

namespace Heimdall.Gateway.Domain.RequestHandlers
{
    public class GetRequestHandler : RequestHandlerBase
    {
        public GetRequestHandler(ILocateService locator, IRequestBuilder requestBuilder) : base(locator, requestBuilder)
        {

        }
    }
}
