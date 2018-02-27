using Newtonsoft.Json;
using System.Net.Http;

namespace Heimdall.Gateway.Domain.Response
{
    public static class HttpResponseExtensions
    {
        public static T AsType<T>(this HttpContent content)
        {
            var data = content.ReadAsStringAsync().Result;
            return string.IsNullOrEmpty(data) ?
                            default(T) :
                            JsonConvert.DeserializeObject<T>(data);
        }

        public static string AsJson(this HttpContent content)
        {
            var data = content.ReadAsStringAsync().Result;
            return JsonConvert.SerializeObject(data);
        }

        public static string AsString(this HttpContent content)
        {
            return content.ReadAsStringAsync().Result;
        }
    }
}
