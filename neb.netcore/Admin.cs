using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Nebulas
{
    public class Admin
    {
        private HttpRequest _request;
        private string _path;

        public Admin(Neb neb)
        {
            SetRequest(neb.Request);
        }


        public void SetRequest(HttpRequest request)
        {
            _request = request;
            _path = "/admin";
        }


        public string nodeInfo()
        {
            return _sendRequest(HttpMethod.Get, "/nodeinfo", null);
        }


        public string accounts()
        {
            return _sendRequest(HttpMethod.Get, "/accounts", null);
        }


        public string newAccount(string options)
        {
            // options = utils.argumentsToObject(['passphrase'], arguments);
            var param = options; /* new { "passphrase": options.passphrase };*/
            return _sendRequest(HttpMethod.Post, "/account/new", param);
        }

        public string unlockAccount(string options)
        {
            // options = utils.argumentsToObject(['address', 'passphrase', 'duration'], arguments);
            var param = options;
            /* new {
                "address": options.address,
                "passphrase": options.passphrase,
                "duration": options.duration
            };*/

            return _sendRequest(HttpMethod.Post, "/account/unlock", param);
        }

        public string lockAccount(string options)
        {
            // options = utils.argumentsToObject(['address'], arguments);
            var param = options; /* new { "address": options.address }; */
            return _sendRequest(HttpMethod.Post, "/account/lock", param);
        }


        public string sendTransaction(string options)
        {
            // options = utils.argumentsToObject(['from', 'to', 'value', 'nonce', 'gasPrice', 'gasLimit', 'contract', 'binary'], arguments);
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
            }; */
            return _sendRequest(HttpMethod.Post, "/transaction", param);
        }


        public string signHash(string options)
        {
            // options = utils.argumentsToObject(['address', 'hash', 'alg'], arguments);
            var param = options;
            /* new {
                "address": options.address,
                "hash": options.hash,
                "alg": options.alg
            };*/
            return _sendRequest(HttpMethod.Post, "/sign/hash", param);
        }


        public string signTransactionWithPassphrase(string options)
        {
            // options = utils.argumentsToObject(['from', 'to', 'value', 'nonce', 'gasPrice', 'gasLimit', 'contract', 'binary', 'passphrase'], arguments);

            /*
            var tx = {
                    "from": options.from,
                    "to": options.to,
                    "value": utils.toString(options.value),
                    "nonce": options.nonce,
                    "gasPrice": utils.toString(options.gasPrice),
                    "gasLimit": utils.toString(options.gasLimit),
                    "contract": options.contract,
                    "binary": options.binary
                };*/
            var param = options; /* new {
        "transaction": tx,
        "passphrase": options.passphrase
    };*/
            return _sendRequest(HttpMethod.Post, "/sign", param);
        }


        public string sendTransactionWithPassphrase(string options)
        {
            // options = utils.argumentsToObject(['from', 'to', 'value', 'nonce', 'gasPrice', 'gasLimit', 'contract', 'binary', 'passphrase'], arguments);
            /*
            var tx = {
            "from": options.from,
            "to": options.to,
            "value": utils.toString(options.value),
            "nonce": options.nonce,
            "gasPrice": utils.toString(options.gasPrice),
            "gasLimit": utils.toString(options.gasLimit),
            "contract": options.contract,
            "binary": options.binary
            };*/

            var param = options; /* new {
                "transaction": tx,
                "passphrase": options.passphrase
            };*/
            return _sendRequest(HttpMethod.Post, "/transactionWithPassphrase", param);
        }


        public string startPprof(string options)
        {
            // options = utils.argumentsToObject(['listen'], arguments);
            var param = options; /* new { "listen": options.listen };*/
            return _sendRequest(HttpMethod.Post, "/pprof", param);
        }

        public string getConfig()
        {
            return _sendRequest(HttpMethod.Get, "/getConfig", null);
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
