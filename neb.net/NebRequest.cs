using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Nebulas
{
    public class NebRequest
    {

        const bool DEBUGLOG = false;

        private static readonly HttpClient _httpClient = new HttpClient();

        public string Host { get; set; } = "http://localhost:8685";
        public uint Timeout { get; set; } = 0;
        public string APIVersion { get; set; } = "v1";

        public NebRequest(string host)
        {
            this.Host = host;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _httpClient.BaseAddress = new Uri(host);
            _httpClient.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
        }

        public NebRequest(string host, uint timeout, string apiVersion)
        {
            this.Host = host;
            this.Timeout = timeout;
            this.APIVersion = apiVersion;
        }

        public void SetHost(string host)
        {
            this.Host = host;
        }


        public string createAbsoluteUrl(string api)
        {
            return this.Host + createRelativeUrl(api);
        }
        public string createRelativeUrl(string api)
        {
            return "/" + this.APIVersion + api;
        }

        public string Request(HttpMethod method, string api, string payload)
        {
            if (DEBUGLOG)
            {
                //log("[debug] HttpRequest: " + method + " " + this.createUrl(api) + " " + JSON.stringify(payload));
            }

            var request = new HttpRequestMessage(method, this.createAbsoluteUrl(api))
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

        public void RequestAsync(HttpMethod method, string api, string payload, Func<string, string> callback)
        {
            RequestAsync(method, api, payload).ContinueWith((readTask) =>
            {
                callback?.Invoke(readTask.Result);
            });
        }
        public Task<string> RequestAsync(HttpMethod method, string api, string payload)
        {
            //Task<string> ret = null;

            var absUrl = this.createAbsoluteUrl(api);
            var relUrl = this.createRelativeUrl(api);

            /*
            var request = new HttpRequestMessage(method, relUrl)
            {
                Content = new StringContent(payload, Encoding.UTF8, "application/json"),
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = _httpClient.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;

                ret = responseContent.ReadAsStringAsync();
            }
            */

            var ret = absUrl
                .WithHeader("Accept", "application/json")
                .PostStringAsync(payload).ReceiveString();

            return ret;
        }
    }
}
