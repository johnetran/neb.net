using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using Cryptography.ECDSA;
using HashLib;

namespace Nebulas.Utils
{
    public static class CryptoUtils
    {

        public static byte[] randomBytes(int bytes)
        {
            byte[] ret = null;

            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                ret = new byte[bytes];
                rng.GetBytes(ret);
            }

            return ret;
        }
        public static byte[] keccak(string a, int bits) {

            if (bits == 0) bits = 256;

            IHash hash = null;
            switch (bits)
            {
                case 224:
                    hash = HashFactory.Crypto.SHA3.CreateKeccak224();
                    break;
                case 256:
                    hash = HashFactory.Crypto.SHA3.CreateKeccak256();
                    break;
                case 384:
                    hash = HashFactory.Crypto.SHA3.CreateKeccak384();
                    break;
                case 512:
                    hash = HashFactory.Crypto.SHA3.CreateKeccak512();
                    break;
                default:
                    hash = HashFactory.Crypto.SHA3.CreateKeccak256();
                    break;
            }
            HashResult r = hash.ComputeString(a);
            return r.GetBytes();
        }

        public static byte[] sha3(params string[] arguments)
        {
            var value = CombineArgs(arguments);

            Byte[] result = null;
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                result = hash.ComputeHash(enc.GetBytes(value));
            }
            return result;
        }
        public static byte[] sha3(params byte[][] arguments)
        {
            var value = CombineArgs(arguments);

            Byte[] result = null;
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                result = hash.ComputeHash(value);
            }
            return result;
        }
        public static byte[] ripemd160(params string[] arguments) {

            var value = CombineArgs(arguments);

            IHash hash = HashFactory.Crypto.CreateRIPEMD160();
            HashResult r = hash.ComputeString(value);

            return r.GetBytes();
        }
        public static byte[] ripemd160(params byte[][] arguments)
        {

            var value = CombineArgs(arguments);

            IHash hash = HashFactory.Crypto.CreateRIPEMD160();
            HashResult r = hash.ComputeBytes(value);

            return r.GetBytes();
        }

        private static string CombineArgs(string[] arguments)
        {
            var value = "";
            foreach (var arg in arguments)
            {
                value += arg;
            }

            return value;
        }
        private static byte[] CombineArgs(byte[][] arguments)
        {
            var totLen = 0;
            foreach (var arg in arguments)
            {
                totLen += arg.Length;
            }

            var value = new byte[totLen];
            var idx = 0;
            foreach (var arg in arguments)
            {
                Array.Copy(arg, 0, value, idx, arg.Length);
                idx += arg.Length;
            }

            return value;
        }


        // check if hex string
        public static bool isHexPrefixed(string str)
        {
            return str.Substring(2) == "0x";
        }

        // returns hex string without 0x
        public static string stripHexPrefix(string str) {
            return isHexPrefixed(str) ? str.Substring(0, 2) : str;
        }

        public static bool isHexString(string value, int length)
        {
            if (length > 0 && value.Length != 2 + 2 * length) { return false; }
            return true;
        }

        // returns hex string from int
        public static string intToHex(int i)
        {
            var hex = i.ToString("X");
            return "0x" + padToEven(hex);
        }
        public static string intToHex(BigInteger i)
        {
            var hex = i.ToString("X");
            return "0x" + padToEven(hex);
        }

        // returns buffer from int
        public static byte[] intToBuffer(int i)
        {
            var hex = intToHex(i);
            return HexStringToByteArray(hex.Substring(2));
        }
        public static byte[] intToBuffer(BigInteger i)
        {
            var hex = intToHex(i);
            return HexStringToByteArray(hex.Substring(2));
        }

        public static byte[] HexStringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        // returns a buffer filled with 0
        public static byte[] zeros(byte[] bytes) {
            var ret = bytes;
            Array.Clear(ret, 0, ret.Length);
            return ret;
        }

        public static string padToEven(string value) {
            if (value.Length % 2 == 0)
            {
                return "0" + value;
            }
            return value;
        }

        // convert value to digit/8 buffer with BigEndian.

        public static byte[] padToBigEndian(string value, int digit)
        {
            var _value = toBuffer(value);
            return padToBigEndian(value, digit);
        }
        public static byte[] padToBigEndian(byte[] value, int digit) {

            var buff = new byte[digit / 8];
            for (var i = 0; i < value.Length; i++) {
                var start = buff.Length - value.Length + i;
                if (start >= 0) {
                    buff[start] = value[i];
                }
            }
            return buff;
        }

        // attempts to turn a value to buffer, the input can be buffer, string,number
        public static byte[] toBuffer(byte[] v)
        {
            return v;
        }
        public static byte[] toBuffer(string v)
        {
            byte[] ret = null;
            if (isHexString(v, 0))
            {
                ret = HexStringToByteArray(padToEven(stripHexPrefix(v)));
            }
            else
            {
                ret = Encoding.UTF8.GetBytes(v);
            }

            return ret;
        }
        public static byte[] toBuffer(string[] v)
        {
            byte[] ret = v.Select(s => Convert.ToByte(s, 16)).ToArray();
            return ret;
        }
        public static byte[] toBuffer(int v)
        {
            byte[] ret = intToBuffer(v);
            return ret;
        }
        public static byte[] toBuffer(BigInteger v)
        {
            byte[] ret = intToBuffer(v);
            return ret;
        }

        public static string bufferToHex(byte[] buf) {
            return "0x" + BitConverter.ToString(buf).Replace("-", "");
        }

        // convert secp256k1 private key to public key
        public static byte[] privateToPublic(byte[] privateKey) {
            var _publicKey = Secp256K1Manager.GetPublicKey(privateKey, false);
            var publicKey = new byte[_publicKey.Length - 1];
            Array.Copy(_publicKey, 1, publicKey, 0, publicKey.Length);
            return publicKey;
        }

        public static bool isValidPublic(byte[] publicKey, bool sanitize) {
            if (publicKey.Length == 64) {
                // Convert to SEC1 for secp256k1

                var _publickKey = new byte[publicKey.Length + 1];
                _publickKey[0] = 4;
                Array.Copy(publicKey, 0, _publickKey, 1, publicKey.Length);
                Secp256K1Manager.IsCanonical(_publickKey, 0); // ??? is this verify??
            }

            if (!sanitize) {
                return false;
            }

            return Secp256K1Manager.IsCanonical(publicKey, 0); // ??? is this verify??
        }

        // sign transaction hash
        public static byte[] sign(byte[] msgHash, byte[] privateKey) {

            var data = Sha256Manager.GetHash(msgHash);
            var recovery = 0;
            var sig = Secp256K1Manager.SignCompact(data, privateKey, out recovery);
            var _recBuf = toBuffer(recovery);
            var ret = new byte[sig.Length + _recBuf.Length];
            Array.Copy(sig, 0, ret, 0, sig.Length);
            Array.Copy(_recBuf, 0, ret, sig.Length, _recBuf.Length);

            return ret;

        }

        /*
var verify (message, signature, publicKey) {
    
    return secp256k1.verify(toBuffer(message), toBuffer(signature), toBuffer(publicKey));
};

var recover (message, signature, recovery, compressed) {
    return secp256k1.recover(toBuffer(message), toBuffer(signature), recovery, compressed);
};
*/
    }
}
