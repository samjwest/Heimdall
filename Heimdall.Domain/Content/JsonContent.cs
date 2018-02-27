using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Heimdall.Gateway.Domain.Content
{
    /// <summary>
    /// Source copied from: https://github.com/TahirNaushad/Fiver.Api.HttpClient
    /// This work was originally completed by Tahir Naushad
    /// and his writeup can be found: https://www.codeproject.com/Articles/1204494/Consuming-ASP-NET-Core-Web-API-using-HttpClient
    /// </summary>
    public class JsonContent : StringContent
    {
        public JsonContent(object value)
            : base (JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json")
        {
        }

        public JsonContent(object value, string mediaType)
            : base(JsonConvert.SerializeObject(value), Encoding.UTF8, mediaType)
        {
        }
    }
}
