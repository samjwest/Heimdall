using Microsoft.AspNetCore.Mvc;

namespace Heimdall.Gateway.Domain.ResponseHandlers
{
    public interface IResponseHandler
    {
        IActionResult BuildResult(int statusCode, object content);
    }
}
