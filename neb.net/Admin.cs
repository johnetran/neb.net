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


        public Task<string> NodeInfo()
        {
            return this._sendRequestAsync(HttpMethod.Get, "/nodeinfo", null);
        }


        public Task<string> Accounts()
        {
            return this._sendRequestAsync(HttpMethod.Get, "/accounts", null);
        }


        public Task<string> NewAccount(NewAccountOptions options)
        {
            var param = JsonConvert.SerializeObject(options);
            return this._sendRequestAsync(HttpMethod.Post, "/account/new", param);
        }

        public Task<string> UnlockAccount(UnlockAccountOptions options)
        {
            var param = JsonConvert.SerializeObject(options);
            return this._sendRequestAsync(HttpMethod.Post, "/account/unlock", param);
        }

        public Task<string> LockAccount(LockAccountOptions options)
        {
            // options = utils.argumentsToObject(['address'], arguments);
            var param = JsonConvert.SerializeObject(options);
            return this._sendRequestAsync(HttpMethod.Post, "/account/lock", param);
        }


        public Task<string> SendTransaction(Transaction options)
        {
            var param = JsonConvert.SerializeObject(options);
            return this._sendRequestAsync(HttpMethod.Post, "/transaction", param);
        }


        public Task<string> SignHash(SignHashOptions options)
        {
            var param = JsonConvert.SerializeObject(options);
            return this._sendRequestAsync(HttpMethod.Post, "/sign/hash", param);
        }


        public Task<string> SignTransactionWithPassphrase(TransactionWithPassphraseOptions options)
        {
            var param = JsonConvert.SerializeObject(options);
            return this._sendRequestAsync(HttpMethod.Post, "/sign", param);
        }


        public Task<string> SendTransactionWithPassphrase(TransactionWithPassphraseOptions options)
        {
            var param = JsonConvert.SerializeObject(options);
            return this._sendRequestAsync(HttpMethod.Post, "/transactionWithPassphrase", param);
        }


        public Task<string> StartPprof(StartPprofOptions options)
        {
            var param = JsonConvert.SerializeObject(options);
            return this._sendRequestAsync(HttpMethod.Post, "/pprof", param);
        }

        public Task<string> GetConfig()
        {
            return this._sendRequestAsync(HttpMethod.Get, "/getConfig", null);
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
