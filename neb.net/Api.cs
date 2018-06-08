using Nebulas;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Nebulas
{
    public class API
    {
        private HttpRequest _request;
        private string _path;

        public API(Neb neb)
        {
            SetRequest(neb.Request);
        }

        public void SetRequest(HttpRequest request)
        {
            _request = request;
            _path = "/user";
        }


        public Task<string> GetNebState()
        {
            return this._sendRequestAsync(HttpMethod.Get, "/nebstate", null);
        }

        public Task<string> LatestIrreversibleBlock()
        {
            return this._sendRequestAsync(HttpMethod.Get, "/lib", null);

        }

        public Task<string> GetAccountState(GetAccountStateOptions options)
        {
            var param = JsonConvert.SerializeObject(options);
            return this._sendRequestAsync(HttpMethod.Post, "/accountstate", param);
        }

        // binary field of TransactionOptions not used
        public Task<string> Call(Transaction options)
        {
            var param = JsonConvert.SerializeObject(options);
            return this._sendRequestAsync(HttpMethod.Post, "/call", param);
        }

        // TODO
        public Task<string> SendRawTransaction(string options)
        {
            //options = utils.argumentsToObject(['data'], arguments);
            
            var param = options; // new { "data": options.data };
            return this._sendRequestAsync(HttpMethod.Post, "/rawtransaction", param);
        }

        public Task<string> GetBlockByHash(GetBlockByHashOptions options)
        {
            var param = JsonConvert.SerializeObject(options);
            return this._sendRequestAsync(HttpMethod.Post, "/getBlockByHash", param);
        }

        public Task<string> GetBlockByHeight(GetBlockByHeightOptions options)
        {
            var param = JsonConvert.SerializeObject(options);
            return this._sendRequestAsync(HttpMethod.Post, "/getBlockByHeight", param);
        }

        public Task<string> GetTransactionReceipt(GetTransactionReceiptOptions options)
        {
            var param = JsonConvert.SerializeObject(options);
            return this._sendRequestAsync(HttpMethod.Post, "/getTransactionReceipt", param);
        }

        public Task<string> GetTransactionByContract(GetTransactionByContractOptions options)
        {
            var param = JsonConvert.SerializeObject(options);
            return this._sendRequestAsync(HttpMethod.Post, "/getTransactionByContract", param);
        }

        public Task<string> Subscribe(SubscribeOptions options)
        {

            //options = utils.argumentsToObject(['topics', 'onDownloadProgress'], arguments);
            //var param = options; // new { "topics": options.topics};

            /*
            var axiosOptions;
            if (typeof options.onDownloadProgress === 'function') {
                axiosOptions = {
                onDownloadProgress: function(e)
                {
                    if (typeof e.target._readLength === 'undefined')
                    {
                        e.target._readLength = 0;
                    }
                    var chunk = e.target.responseText.substr(e.target._readLength);
                    // TODO check and split multi events
                    if (chunk && chunk.trim().length > 0)
                    {
                        e.target._readLength += chunk.length;
                        options.onDownloadProgress(chunk);
                    }
                }
                };
            }
            return this._sendRequestAsync(HttpMethod.Post, "/subscribe", param, null, axiosOptions);
            */

            var param = JsonConvert.SerializeObject(options);
            return this._sendRequestAsync(HttpMethod.Post, "/subscribe", param);
        }

        public Task<string> GasPrice()
        {
            return this._sendRequestAsync(HttpMethod.Get, "/getGasPrice", null);
        }

        public Task<string> EstimateGas(Transaction options)
        {
            var param = JsonConvert.SerializeObject(options);
            return this._sendRequestAsync(HttpMethod.Post, "/estimateGas", param);
        }

        public Task<string> GetEventsByHash(GetEventsByHashOptions options)
        {
            var param = JsonConvert.SerializeObject(options);
            return this._sendRequestAsync(HttpMethod.Post, "/getEventsByHash", param);
        }

        public Task<string> GetDynasty(GetDynastyOptions options)
        {
            var param = JsonConvert.SerializeObject(options);
            return this._sendRequestAsync(HttpMethod.Post, "/dynasty", param);
        }

        private string _sendRequest(HttpMethod method, string api, string param)
        {
            var action = this._path + api;
            return _request.Request(method, action, param);
        }
        private void _sendRequestAsync(HttpMethod method, string api, string param, Func<string, string> callback)
        {
            var action = this._path + api;
            this._sendRequestAsync(method, action, param, callback);
        }
        private Task<string> _sendRequestAsync(HttpMethod method, string api, string param)
        {
            var action = this._path + api;
            return this._sendRequestAsync(method, api, param);
        }

    }
}
