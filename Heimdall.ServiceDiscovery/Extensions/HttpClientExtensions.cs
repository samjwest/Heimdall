using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Heimdall.Gateway.ServiceDiscovery.Extensions
{
    public static class HttpClientExtension
    {
        public static HttpClient WithBase(this HttpClient client, Uri baseAddress)
        {
            client.BaseAddress = baseAddress;
            return client;
        }

        public static HttpClient WithSecurity(this HttpClient client, string securityToken)
        {
            client.DefaultRequestHeaders.Add("Heimdall.Service.Token", securityToken);
            return client;
        }

        //public static HttpClient ForJson(this HttpClient client)
        //{
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    return client;
        //}
    }
}
