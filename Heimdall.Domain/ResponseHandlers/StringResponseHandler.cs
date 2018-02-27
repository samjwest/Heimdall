using Microsoft.AspNetCore.Mvc;

namespace Heimdall.Gateway.Domain.ResponseHandlers
{
    public class StringResponseHandler : IResponseHandler
    {
        public IActionResult BuildResult(int statusCode, object content)
        {
            var stringContent = new ContentResult();
            stringContent.Content = content as string;
            stringContent.StatusCode = statusCode;
            return (stringContent);
        }
    }
}
