using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Nebulas
{
    public class HttpRequest
    {

        const bool DEBUGLOG = false;

        private static readonly HttpClient _httpClient = new HttpClient();

        public string Host { get; set; } = "http://localhost:8685";
        public uint Timeout { get; set; } = 0;
        public string APIVersion { get; set; } = "v1";

        public HttpRequest(string host, uint timeout, string apiVersion)
        {

        }

        public void SetHost(string host)
        {
            this.Host = host;
        }


        public string createUrl(string api)
        {
            return this.Host + "/" + this.APIVersion + api;
        }

        public string Request(HttpMethod method, string api, string payload)
        {
            if (DEBUGLOG)
            {
                //log("[debug] HttpRequest: " + method + " " + this.createUrl(api) + " " + JSON.stringify(payload));
            }

            var request = new HttpRequestMessage(method, this.createUrl(api))
            {
                Content = new StringContent(payload)
            };

            var response = _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead).Result;
            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;

                // by calling .Result you are synchronously reading the result
                return responseContent.ReadAsStringAsync().Result;
            }
            else
            {
                return response.StatusCode.ToString();
            }            
        }

        public HttpStatusCode AsyncRequestAsync(HttpMethod method, string api, string payload, Func<string, string> callback)
        {
            var request = new HttpRequestMessage(method, this.createUrl(api))
            {
                Content = new StringContent(payload)
            };

            var response = _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead).Result;
            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;

                // by calling .Result you are synchronously reading the result
                responseContent.ReadAsStringAsync().ContinueWith((readTask) =>
                {
                    callback?.Invoke(readTask.Result);
                });
            }

            return response.StatusCode;
        }
    }
}
