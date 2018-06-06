using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

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


        public string GetNebState()
        {
            return _sendRequest(HttpMethod.Get, "/nebstate", null);
        }

        public string LatestIrreversibleBlock()
        {
            return _sendRequest(HttpMethod.Get, "/lib", null);

        }

        public string GetAccountState(string options)
        {
            //options = utils.argumentsToObject(['address', 'height'], arguments);
            var param = options; // new { address: options.address, height: options.height };
            return _sendRequest(HttpMethod.Post, "/accountstate", param);
        }

        public string call(string options)
        {

            // options = utils.argumentsToObject(['from', 'to', 'value', 'nonce', 'gasPrice', 'gasLimit', 'contract'], arguments);
            var param = options;
            /*
            new {
            from: options.from,
            to: options.to,
            value: utils.toString(options.value),
            nonce: options.nonce,
            gasPrice: utils.toString(options.gasPrice),
            gasLimit: utils.toString(options.gasLimit),
            contract: options.contract
            };
            */
            return _sendRequest(HttpMethod.Post, "/call", param);
        }

        public string sendRawTransaction(string options)
        {
            //options = utils.argumentsToObject(['data'], arguments);
            var param = options; // new { "data": options.data };
            return _sendRequest(HttpMethod.Post, "/rawtransaction", param);
        }

        public string getBlockByHash(string options)
        {

            //options = utils.argumentsToObject(['hash', 'fullTransaction'], arguments);
            var param = options; //new { "hash": options.hash, "full_fill_transaction": options.fullTransaction};
            return this._sendRequest(HttpMethod.Post, "/getBlockByHash", param);
        }

        public string getBlockByHeight(string options)
        {
            //options = utils.argumentsToObject(['height', 'fullTransaction'], arguments);
            var param = options; //new  { "height": options.height, "full_fill_transaction": options.fullTransaction};
            return this._sendRequest(HttpMethod.Post, "/getBlockByHeight", param);
        }

        public string getTransactionReceipt(string options)
        {

            //options = utils.argumentsToObject(['hash'], arguments);
            var param = options; //new { "hash": options.hash};
            return _sendRequest(HttpMethod.Post, "/getTransactionReceipt", param);
        }

        public string getTransactionByContract(string options)
        {

            //options = utils.argumentsToObject(['address'], arguments);
            var param = options; // new { "address": options.address};
            return _sendRequest(HttpMethod.Post, "/getTransactionByContract", param);
        }

        public string subscribe(string options)
        {

            //options = utils.argumentsToObject(['topics', 'onDownloadProgress'], arguments);
            var param = options; // new { "topics": options.topics};

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
            return this._sendRequest(HttpMethod.Post, "/subscribe", param, null, axiosOptions);
            */
            return this._sendRequest(HttpMethod.Post, "/subscribe", param);
        }

        public string gasPrice()
        {
            return _sendRequest(HttpMethod.Get, "/getGasPrice", null);
        }

        public string estimateGas(string options)
        {

            //options = utils.argumentsToObject(['from', 'to', 'value', 'nonce', 'gasPrice', 'gasLimit', 'contract', 'binary'], arguments);
            var param = options;

            /* new {
                "from": options.from,
                "to": options.to,
                "value": utils.toString(options.value),
                "nonce": options.nonce,
                "gasPrice": utils.toString(options.gasPrice),
                "gasLimit": utils.toString(options.gasLimit),
                "contract": options.contract,
                "binary": options.binary
            };*/

            return _sendRequest(HttpMethod.Post, "/estimateGas", param);
        }

        public string getEventsByHash(string options)
        {

            //options = utils.argumentsToObject(['hash'], arguments);

            var param = options; // new { "hash": options.hash};
            return this._sendRequest(HttpMethod.Post, "/getEventsByHash", param);
        }

        public string getDynasty(string options)
        {
            var param = options; // new {"height": options.height};
            return this._sendRequest(HttpMethod.Post, "/dynasty", param);
        }

        private string _sendRequest(HttpMethod method, string api, string param)
        {
            return _request.Request(method, api, param);
        }
        private HttpStatusCode _sendRequest(HttpMethod method, string api, string param, Func<string, string> callback)
        {
            return _request.AsyncRequestAsync(method, api, param, callback);
        }
    }
}
