using System;
using System.Collections.Generic;
using System.Text;

namespace NebulasNetCore
{
    public class Transaction
    {
        public const int SECP256K1 = 1;
        public const string TXPAYLOADBINARYTYPE = "binary";
        public const string TXPAYLOADDEPLOYTYPE = "deploy";
        public const string TXPAYLOADCALLTYPE = "call";

        public Transaction()
        {
        }
    }
}
