using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Heimdall.Gateway.Domain.ResponseHandlers
{
    public class JSONResponseHandler : IResponseHandler
    {
        public IActionResult BuildResult(int statusCode, object content)
        {
            var obj = JsonConvert.DeserializeObject(content as string);
            var jsonResult = new ObjectResult(obj);
            jsonResult.StatusCode = statusCode;
            return (jsonResult);
        }
    }
}
