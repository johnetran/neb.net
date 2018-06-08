using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Transactions;

namespace Nebulas
{
    public class NewAccountOptions
    {
        public string passphrase { get; set; }
    }
    public class UnlockAccountOptions
    {
        public string address { get; set; }
        public string passphrase { get; set; }
        public int duration { get; set; }
    }
    public class LockAccountOptions
    {
        public string address { get; set; }
    }

    public class RawTransactionOptions
    {
        public string data { get; set; }
    }

    public class SignHashOptions
    {
        public string address { get; set; }
        public string hash { get; set; }
        public string alg { get; set; }
    }
    public class TransactionWithPassphraseOptions
    {
        public Transaction tx { get; set; }
        public string passphrase { get; set; }
    }
    public class StartPprofOptions
    {
        public string listen { get; set; }
    }

    public class GetAccountStateOptions
    {
        public string address { get; set; }
        public int height { get; set; }
    }

    public class SubscribeOptions
    {
        public string topics { get; set; }
    }

    public class GetBlockByHashOptions
    {
        public string hash { get; set; }
        public bool full_fill_transaction { get; set; }
    }
    public class GetBlockByHeightOptions
    {
        public int height{ get; set; }
        public bool full_fill_transaction { get; set; }
    }
    public class GetTransactionReceiptOptions
    {
        public string hash { get; set; }
    }
    public class GetTransactionByContractOptions
    {
        public string address { get; set; }
    }
    public class GetEventsByHashOptions
    {
        public string hash { get; set; }
    }
    public class GetDynastyOptions
    {
        public string hash { get; set; }
    }
}
