using Microsoft.AspNetCore.Mvc;

namespace Heimdall.Gateway.Domain.ResponseHandlers
{
    public class NoContentResponseHandler : IResponseHandler
    {
        public IActionResult BuildResult(int statusCode, object content = null)
        {
            return (new StatusCodeResult(statusCode));
        }
    }
}
