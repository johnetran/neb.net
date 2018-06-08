using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Web;
using Google.Protobuf;
using Nebulas.Utils;

namespace Nebulas
{
    public class Contract
    {
        public string sourceType { get; set; }
        public string function { get; set; }
        public string args { get; set; }
        public string source { get; set; }
        public byte[] binary { get; set; }
    }
    public class Payload
    {
        public string SourceType { get; set; }
        public string Function { get; set; }
        public string Args { get; set; }
        public string Source { get; set; }
        public byte[] Data { get; set; }
    }
    public class ParsedContract
    {
        public string type { get; set; }
        public byte[] payload { get; set; }
    }

    public class Transaction
    {
        public const int SECP256K1 = 1;
        public const string TXPAYLOADBINARYTYPE = "binary";
        public const string TXPAYLOADDEPLOYTYPE = "deploy";
        public const string TXPAYLOADCALLTYPE = "call";

        public uint chainID { get; set; }
        public Account from { get; set; }
        public Account to { get; set; }
        public BigInteger value { get; set; }
        public ulong nonce { get; set; }
        public BigInteger gasPrice { get; set; }
        public BigInteger gasLimit { get; set; }
        public Contract contract { get; set; }
        public byte[] binary { get; set; }
        public ParsedContract data { get; set; }

        public long timestamp { get; set; }
        public string SignErrorMessage { get; set; }

        public byte[] hash { get; set; }
        public uint alg { get; set; }
        public byte[] sign { get; set; }

        private static Int64 GetTime()
        {
            Int64 retval = 0;
            var st = new DateTime(1970, 1, 1);
            TimeSpan t = (DateTime.Now.ToUniversalTime() - st);
            retval = (Int64)(t.TotalMilliseconds + 0.5);
            return retval;
        }

        public ParsedContract ParseContract(Contract obj)
        {
            /*jshint maxcomplexity:7 */

            string payloadType;
            Payload payload = null;
            if (obj != null && !string.IsNullOrEmpty(obj.source) && obj.source.Length > 0)
            {
                payloadType = TXPAYLOADDEPLOYTYPE;
                payload = new Payload
                {
                    SourceType = obj.sourceType,
                    Source = obj.source,
                    Args = obj.args
                };
            }
            else if (obj != null && !string.IsNullOrEmpty(obj.function) && obj.function.Length > 0)
            {
                payloadType = TXPAYLOADCALLTYPE;
                payload = new Payload
                {
                    Function = obj.function,
                    Args = obj.args
                };
            }
            else
            {
                payloadType = TXPAYLOADBINARYTYPE;
                if (obj != null)
                {
                    payload = new Payload
                    {
                        Data = CryptoUtils.toBuffer(obj.binary)
                    };
                }
            }
            var payloadData = payload == null ? null : CryptoUtils.toBuffer(HttpUtility.HtmlEncode(payload));

            return new ParsedContract { type = payloadType, payload = payloadData };
        }

        public Transaction()
        {

        }
        public Transaction(uint chainID, string from, string to, BigInteger value, ulong nonce,
            Contract contract, BigInteger gasPrice, BigInteger gasLimit)
        {
            /*
            Corepb.Transaction tx = new Corepb.Transaction();
            var b = tx.ToByteArray();
            
            var test = Corepb.Transaction.Parser.ParseFrom(b);
            JsonParser.Default.Parse<Corepb.Transaction>("");
            */

            this.chainID = chainID;
            this.from = Account.fromAddress(from);
            this.to = Account.fromAddress(to);
            this.value = value;
            this.nonce = nonce;
            this.timestamp = (long)Math.Floor((double)(GetTime() / 1000));
            this.contract = contract;
            this.gasPrice = gasPrice;
            this.gasLimit = gasLimit;

            this.data = ParseContract(this.contract);
            if (this.gasPrice <= 0)
            {
                this.gasPrice = 1000000;
            }

            if (this.gasLimit <= 0)
            {
                this.gasLimit = 20000;
            }
            this.SignErrorMessage = "You should sign transaction before this operation.";
        }

        /**
         * Convert transaction to hash by SHA3-256 algorithm.
         *
         * @return {Hash} hash of Transaction.
         *
         * @example
         * var acc = Account.NewAccount();
         *
         * var tx = new Transaction({
         *    chainID: 1,
         *    from: acc,
         *    to: "n1SAeQRVn33bamxN4ehWUT7JGdxipwn8b17",
         *    value: 10,
         *    nonce: 12,
         *    gasPrice: 1000000,
         *    gasLimit: 2000000
         * });
         * var txHash = tx.hashTransaction();
         * //Uint8Array(32) [211, 213, 102, 103, 23, 231, 246, 141, 20, 202, 210, 25, 92, 142, 162, 242, 232, 95, 44, 239, 45, 57, 241, 61, 34, 2, 213, 160, 17, 207, 75, 40]
         */
        public byte[] HashTransaction()
        {
            var data = new Corepb.Data
            {
                Payload = ByteString.CopyFrom(this.data.payload),
                Type = this.data.type
            };

            var dataBuffer = data.ToByteArray();

            var hash = CryptoUtils.sha3(
                this.from.GetAddress(),
                this.to.GetAddress(),
                CryptoUtils.padToBigEndian(this.value.ToString("X"), 128),
                CryptoUtils.padToBigEndian(this.nonce.ToString("X"), 64),
                CryptoUtils.padToBigEndian(this.timestamp.ToString("X"), 64),
                dataBuffer,
                CryptoUtils.padToBigEndian(this.chainID.ToString("X"), 32),
                CryptoUtils.padToBigEndian(this.gasPrice.ToString("X"), 128),
                CryptoUtils.padToBigEndian(this.gasLimit.ToString("X"), 128)
                );
            return hash;
        }

        /**
         * Sign transaction with the specified algorithm.
         *
         * @example
         * var acc = Account.NewAccount();
         *
         * var tx = new Transaction({
         *    chainID: 1,
         *    from: acc,
         *    to: "n1SAeQRVn33bamxN4ehWUT7JGdxipwn8b17",
         *    value: 10,
         *    nonce: 12,
         *    gasPrice: 1000000,
         *    gasLimit: 2000000
         * });
         * tx.signTransaction();
         */
        public void SignTransaction()
        {
            if (this.from.GetPrivateKey() != null)
            {
                this.hash = this.HashTransaction();
                this.alg = SECP256K1;
                this.sign = CryptoUtils.sign(this.hash, this.from.GetPrivateKey());
            }
            else
            {
                throw new Exception("transaction from address's private key is invalid");
            }
        }

        /**
         * Conver transaction data to plain JavaScript object.
         *
         * @return {Object} Plain JavaScript object with Transaction fields.
         * @example
         * var acc = Account.NewAccount();
         * var tx = new Transaction({
         *    chainID: 1,
         *    from: acc,
         *    to: "n1SAeQRVn33bamxN4ehWUT7JGdxipwn8b17",
         *    value: 10,
         *    nonce: 12,
         *    gasPrice: 1000000,
         *    gasLimit: 2000000
         * });
         * txData = tx.toPlainObject();
         * // {chainID: 1001, from: "n1USdDKeZXQYubA44W2ZVUdW1cjiJuqswxp", to: "n1SAeQRVn33bamxN4ehWUT7JGdxipwn8b17", value: 1000000000000000000, nonce: 1, …}
         */
        public Transaction ToPlainObject()
        {
            return new Transaction
            {
                chainID = this.chainID,
                from = this.from,
                to = this.to,
                value = this.value,
                nonce = this.nonce,
                gasPrice = this.gasPrice,
                gasLimit = this.gasLimit,
                contract = this.contract
            };
        }
        /**
         * Convert transaction to JSON string.
         * </br><b>Note:</b> Transaction should be [sign]{@link Transaction#signTransaction} before converting.
         *
         * @return {String} JSON stringify of transaction data.
         * @example
         * var acc = Account.NewAccount();
         *
         * var tx = new Transaction({
         *    chainID: 1,
         *    from: acc,
         *    to: "n1SAeQRVn33bamxN4ehWUT7JGdxipwn8b17",
         *    value: 10,
         *    nonce: 12,
         *    gasPrice: 1000000,
         *    gasLimit: 2000000
         * });
         * tx.signTransaction();
         * var txHash = tx.toString();
         * // "{"chainID":1001,"from":"n1QZMXSZtW7BUerroSms4axNfyBGyFGkrh5","to":"n1SAeQRVn33bamxN4ehWUT7JGdxipwn8b17","value":"1000000000000000000","nonce":1,"timestamp":1521905294,"data":{"payloadType":"binary","payload":null},"gasPrice":"1000000","gasLimit":"20000","hash":"f52668b853dd476fd309f21b22ade6bb468262f55402965c3460175b10cb2f20","alg":1,"sign":"cf30d5f61e67bbeb73bb9724ba5ba3744dcbc995521c62f9b5f43efabd9b82f10aaadf19a9cdb05f039d8bf074849ef4b508905bcdea76ae57e464e79c958fa900"}"
         */
        public override string ToString()
        {
            if (this.sign == null)
            {
                throw new Exception(this.SignErrorMessage);
            }
            var payload = this.data.payload;
            var tx = new Transaction
            {
                chainID = this.chainID,
                from = this.from,
                to = this.to,
                value = this.value,
                nonce = this.nonce,
                timestamp = this.timestamp,
                data = new ParsedContract { type = this.data.type, payload = payload },
                gasPrice = this.gasPrice,
                gasLimit = this.gasLimit,
                hash = this.hash,
                alg = this.alg,
                sign = this.sign

            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(tx);
        }

        /**
         * Convert transaction to Protobuf format.
         * </br><b>Note:</b> Transaction should be [sign]{@link Transaction#signTransaction} before converting.
         *
         * @return {Buffer} Transaction data in Protobuf format
         *
         * @example
         * var acc = Account.NewAccount();
         *
         * var tx = new Transaction({
         *    chainID: 1,
         *    from: acc,
         *    to: "n1SAeQRVn33bamxN4ehWUT7JGdxipwn8b17",
         *    value: 10,
         *    nonce: 12,
         *    gasPrice: 1000000,
         *    gasLimit: 2000000
         * });
         * tx.signTransaction();
         * var txHash = tx.toProto();
         * // Uint8Array(127)
         */
        public byte[] ToProto()
        {
            if (this.sign == null)
            {
                throw new Exception(this.SignErrorMessage);
            }
            var data = new Corepb.Data
            {
                Payload = ByteString.CopyFrom(this.data.payload),
                Type = this.data.type
            };

            var txData = new Corepb.Transaction
            {
                Hash = ByteString.CopyFrom(this.hash),
                From = ByteString.CopyFrom(this.from.Address),
                To = ByteString.CopyFrom(this.to.Address),
                Value = ByteString.CopyFrom(CryptoUtils.padToBigEndian(this.value.ToString(), 128)),
                Nonce = this.nonce,
                Timestamp = this.timestamp,
                Data = data,
                ChainId = this.chainID,
                GasPrice = ByteString.CopyFrom(CryptoUtils.padToBigEndian(this.gasPrice.ToString(), 128)),
                GasLimit = ByteString.CopyFrom(CryptoUtils.padToBigEndian(this.gasLimit.ToString(), 128)),
                Alg = this.alg,
                Sign = ByteString.CopyFrom(this.sign)
            };

            var txBuffer = txData.ToByteArray();
            return txBuffer;
        }
        /**
         * Convert transaction to Protobuf hash string.
         * </br><b>Note:</b> Transaction should be [sign]{@link Transaction#signTransaction} before converting.
         *
         * @return {Base64} Transaction string.
         *
         * @example
         * var acc = Account.NewAccount();
         *
         * var tx = new Transaction({
         *    chainID: 1,
         *    from: acc,
         *    to: "n1SAeQRVn33bamxN4ehWUT7JGdxipwn8b17",
         *    value: 10,
         *    nonce: 12,
         *    gasPrice: 1000000,
         *    gasLimit: 2000000
         * });
         * tx.signTransaction();
         * var txHash = tx.toProtoString();
         * // "EhjZTY/gKLhWVVMZ+xoY9GiHOHJcxhc4uxkaGNlNj+AouFZVUxn7Ghj0aIc4clzGFzi7GSIQAAAAAAAAAAAN4Lazp2QAACgBMPCz6tUFOggKBmJpbmFyeUDpB0oQAAAAAAAAAAAAAAAAAA9CQFIQAAAAAAAAAAAAAAAAAABOIA=="
         */
        public string toProtoString()
        {
            var txBuffer = this.ToProto();
            return Convert.ToBase64String(txBuffer, 0, txBuffer.Length);
        }

        /**
         * Restore Transaction from Protobuf format.
         * @property {Buffer|String} data - Buffer or stringify Buffer.
         *
         * @return {Transaction} Restored transaction.
         *
         * @example
         * var acc = Account.NewAccount();
         *
         * var tx = new Transaction({
         *    chainID: 1,
         *    from: acc,
         *    to: "n1SAeQRVn33bamxN4ehWUT7JGdxipwn8b17",
         *    value: 10,
         *    nonce: 12,
         *    gasPrice: 1000000,
         *    gasLimit: 2000000
         * });
         * var tx = tx.fromProto("EhjZTY/gKLhWVVMZ+xoY9GiHOHJcxhc4uxkaGNlNj+AouFZVUxn7Ghj0aIc4clzGFzi7GSIQAAAAAAAAAAAN4Lazp2QAACgBMPCz6tUFOggKBmJpbmFyeUDpB0oQAAAAAAAAAAAAAAAAAA9CQFIQAAAAAAAAAAAAAAAAAABOIA==");
         */
        public Transaction FromProto(string data)
        {
            var txBuffer = Convert.FromBase64String(data);
            return FromProto(txBuffer);
        }

        public Transaction FromProto(byte[] data)
        {

            var txBuffer = data;

            var txProto = Corepb.Transaction.Parser.ParseFrom(txBuffer);

            this.hash = CryptoUtils.toBuffer(txProto.Hash.ToByteArray());
            this.from = Account.fromAddress(txProto.From.ToString());
            this.to = Account.fromAddress(txProto.To.ToString());

            this.value = BigInteger.Parse(txProto.Value.ToString());
            // long number is object, should convert to int
            this.nonce = ulong.Parse(txProto.Nonce.ToString());
            this.timestamp = long.Parse(txProto.Timestamp.ToString());
            this.data = new ParsedContract
            {
                payload = txProto.Data.Payload.ToByteArray(),
                type = txProto.Data.Type
            };
            if (this.data.payload.Length == 0)
            {
                this.data.payload = null;
            }
            this.chainID = txProto.ChainId;
            this.gasPrice = BigInteger.Parse(txProto.GasPrice.ToString());
            this.gasLimit = BigInteger.Parse(txProto.GasLimit.ToString());
            this.alg = uint.Parse(txProto.Alg.ToString());
            this.sign = txProto.Sign.ToByteArray();

            return this;
        }
    }
}
