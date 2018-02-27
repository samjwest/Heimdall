using Heimdall.Gateway.Domain.Requests;
using Heimdall.Gateway.Core;

namespace Heimdall.Gateway.Domain.RequestHandlers
{
    public class PutRequestHandler : RequestHandlerBase
    {
        public PutRequestHandler(ILocateService locator, IRequestBuilder requestBuilder) : base(locator, requestBuilder)
        {

        }
    }
}