using Cryptography.ECDSA;
using NebulasNetCore.Utils;
using Norgerman.Cryptography.Scrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NebulasNetCore
{

    public class KeyOptions
    {
        public byte[] salt { get; set; }
        public byte[] iv { get; set; }
        public string kdf { get; set; }
        public int dklen { get; set; }
        public int c { get; set; }
        public int n { get; set; }
        public int r { get; set; }
        public int p { get; set; }
        public string cipher { get; set; }
        public byte[] uuid { get; set; }
    }
    public class Key
    {
        public int version { get; set; }
        public byte[] id { get; set; }
        public string address { get; set; }
        public KeyCrypto crypto { get; set; }
    }
    public class CipherParams
    {
        public string iv { get; set; }
    }
    public class KDFParams
    {
        public int dklen { get; set; }
        public string salt { get; set; }
        public int c { get; set; }
        public string prf { get; set; }
        public int n { get; set; }
        public int r { get; set; }
        public int p { get; set; }
    }
    public class KeyCrypto
    {
        public string ciphertext { get; set; }
        public CipherParams cipherparams { get; set; }
        public string cipher { get; set; }
        public string kdf { get; set; }
        public KDFParams kdfparams { get; set; }
        public string mac { get; set; }
        public string machash { get; set; }
    }

    public partial class Account
    {
        const int ADDRESSLENGTH = 26;
        const int ADDRESSPREFIX = 25;
        const int NORMALTYPE = 87;
        const int CONTRACTTYPE = 88;

        const int KEYVERSION3 = 3;
        const int KEYCURRENTVERSION = 4;

        public string Path { get; set; }
        public byte[] PrivKey { get; set; }
        public byte[] PubKey { get; set; }
        public byte[] Address { get; set; }

        public Account()
        {
        }
        public Account(byte[] priv)
        {
            this.SetPrivateKey(priv);
        }
        public Account(byte[] priv, string path)
        {
            this.SetPrivateKey(priv);
            this.Path = path;
        }

        /**
         * Private Key setter.
         *
         * @param {Hash} priv - Account private key.
         *
         * @example account.setPrivateKey("ac3773e06ae74c0fa566b0e421d4e391333f31aef90b383f0c0e83e4873609d6");
         */
        public void SetPrivateKey(string priv)
        {
            var _priv = CryptoUtils.toBuffer(priv);
            SetPrivateKey(_priv);
        }
        public void SetPrivateKey(byte[] priv)
        {
            this.PrivKey = priv.Length == 32 ? priv : null;
            this.PubKey = null;
            this.Address = null;

        }

        /**
         * Private Key getter.
         *
         * @return {Buffer} Account private key.
         *
         * @example var privKey = account.getPrivateKey();
         * //<Buffer 5b ed 67 f9 9c b3 31 9e 0c 6f 6a 03 54 8b e3 c8 c5 2a 83 64 46 4f 88 6f> 24
         */
        public byte[] GetPrivateKey()
        {
            return this.PrivKey;
        }

        /**
         * Get Private Key in hex string format.
         *
         * @return {HexString} Account private key in String format.
         *
         * @example var privKey = account.getPrivateKeyString();
         * //"ac3773e06ae74c0fa566b0e421d4e391333f31aef90b383f0c0e83e4873609d6"
         */
        public string GetPrivateKeyString()
        {
            return CryptoUtils.bufferToHex(this.GetPrivateKey());
        }
        /**
         * Public Key getter.
         *
         * @return {Buffer} Account public key.
         *
         * @example var publicKey = account.getPublicKey();
         * //<Buffer c0 96 aa 4e 66 c7 4a 9a c7 18 31 f1 24 72 2a c1 3e b5 df 7f 97 1b 13 1d 46 a2 8a e6 81 c6 1d 96 f7 07 d0 aa e9 a7 67 436b 68 af a8 f0 96 65 17 24 29 ... >
         */
        public byte[] GetPublicKey()
        {
            if (this.PubKey == null)
            {
                this.PubKey = CryptoUtils.privateToPublic(this.PrivKey);
            }
            return this.PubKey;
        }

        /**
         * Get Public Key in hex string format.
         *
         * @return {HexString} Account public key in String format.
         *
         * @example var publicKey = account.getPublicKey();
         * //"f18ec04019dd131bbcfada4020b001d547244d768f144ef947577ce53a13ad690eb43e4b02a8daa3c168045cd122c0685f083e1656756ba7982721322ebe4da7"
         */
        public string getPublicKeyString()
        {
            return CryptoUtils.bufferToHex(this.GetPublicKey());
        }

        /**
         * Accaunt address getter.
         *
         * @return {Buffer} Account address.
         *
         * @example var publicKey = account.getAddress();
         * //<Buffer 7f 87 83 58 46 96 12 7d 1a c0 57 1a 42 87 c6 25 36 08 ff 32 61 36 51 7c>
         */
        public byte[] GetAddress()
        {
            if (this.Address == null)
            {
                byte[] pubKey = null;

                var _pubKey = this.GetPublicKey();
                if (_pubKey.Length != 64)
                {
                    // get uncompressed public key
                    var _uPubKey = Secp256K1Manager.GetPublicKey(this.PrivKey, false);
                    pubKey = new byte[_uPubKey.Length - 1];
                    Array.Copy(_uPubKey, 1, pubKey, 0, pubKey.Length);
                }
                else
                {
                    pubKey = _pubKey;
                }

                // The uncompressed form consists of a 0x04 (in analogy to the DER OCTET STRING tag) plus
                // the concatenation of the binary representation of the X coordinate plus the binary
                // representation of the y coordinate of the public point.

                var _cPubKey = new byte[pubKey.Length + 1];
                Array.Copy(pubKey, 0, _cPubKey, 1, pubKey.Length);
                _cPubKey[0] = 4;

                // Only take the lower 160bits of the hash
                var _scontent = CryptoUtils.sha3(_cPubKey);
                var _rContent = CryptoUtils.ripemd160(_scontent);
                // content = AddressPrefix + NormalType + content(local address only use normal type)

                var content = new byte[_rContent.Length + 2];
                content[0] = ADDRESSPREFIX;
                content[1] = NORMALTYPE;
                Array.Copy(_rContent, 0, content, 2, _rContent.Length);
                var _sChecksum = CryptoUtils.sha3(content);
                var checksum = new byte[4];
                Array.Copy(_sChecksum, 0, checksum, 0, 4);
                this.Address = new byte[content.Length + checksum.Length];
                Array.Copy(content, 0, this.Address, 0, content.Length);
                Array.Copy(checksum, 0, this.Address, content.Length, checksum.Length);
            }
            return this.Address;
        }

        /**
         * Get account address in hex string format.
         *
         * @return {HexString} Account address in String format.
         *
         * @example var publicKey = account.getAddressString();
         * //"802d529bf55d6693b3ac72c59b4a7d159da53cae5a7bf99c"
         */
        public string GetAddressString()
        {
            var addr = this.GetAddress();
            return Base58.Encode(addr);
        }

        /**
         * Generate key by passphrase and options.
         *
         * @param {Password} password - Provided password.
         * @param {KeyOptions} opts - Key options.
         *
         * @return {Key} Key Object.
         *
         * @example var key = account.toKey("passphrase");
         */
        public Key ToKey(string password, KeyOptions opts)
        {
            /*jshint maxcomplexity:16 */

            opts = opts ?? new KeyOptions();
            var salt = opts.salt ?? CryptoUtils.randomBytes(32);
            var iv = opts.iv ?? CryptoUtils.randomBytes(16);
            byte[] derivedKey;
            var kdf = opts.kdf ?? "scrypt";
            var kdfparams = new KDFParams {
                dklen = opts.dklen == 0 ? 32 : opts.dklen,
                salt = CryptoUtils.bufferToHex(salt),
                c = (kdf == "pbkdf2") ? (opts.c == 0 ? 262144 : opts.c) : 0,
                prf = (kdf == "pbkdf2") ? "hmac-sha256" : "",
                n = (kdf == "scrypt") ? (opts.n == 0 ? 4096 : opts.n) : 0,
                r = (kdf == "scrypt") ? (opts.r == 0 ? 8 : opts.r) : 0,
                p = (kdf == "scrypt") ? (opts.p == 0 ? 1 : opts.p) : 0,
            };
            if (kdf == "pbkdf2") {
                RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                crypto.GetBytes(salt);
                Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, kdfparams.c, HashAlgorithmName.SHA256);
                derivedKey = pbkdf2.GetBytes(kdfparams.dklen);
            } else if (kdf == "scrypt") {
                derivedKey = ScryptUtil.Scrypt(CryptoUtils.HexStringToByteArray(password), salt, kdfparams.n, kdfparams.r, kdfparams.p, kdfparams.dklen);
            } else {
                throw new Exception("Unsupported kdf");
            }

            // TODO: need to handle other algos
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = kdfparams.dklen;
            aes.IV = iv;
            aes.Key = new byte[16];
            Array.Copy(derivedKey, 0, aes.Key, 0, 16);
            aes.Mode = CipherMode.CTS;

            byte[] src = this.PrivKey;
            byte[] dest = new byte[0];

            using (ICryptoTransform encrypt = aes.CreateEncryptor())
            {
                dest = encrypt.TransformFinalBlock(src, 0, src.Length);
            }

            var ciphertext = new byte[src.Length + dest.Length];
            Array.Copy(src, 0, ciphertext, 0, src.Length);
            Array.Copy(dest, 0, ciphertext, src.Length, dest.Length);

            var _derivedKey = new byte[32];
            Array.Copy(derivedKey, 16, _derivedKey, 0, 32);

            var algoStr = opts.cipher ?? "aes-128-ctr";
            var algoBuf = CryptoUtils.HexStringToByteArray(algoStr);

            var mac = new byte[_derivedKey.Length + ciphertext.Length + iv.Length + algoBuf.Length];

            /*
            var cipher = CryptoUtils.crypto.createCipheriv(opts.cipher || 'aes-128-ctr', derivedKey.slice(0, 16), iv);
            if (!cipher) {
                throw new Error('Unsupported cipher');
            }
            var ciphertext = Buffer.concat([cipher.update(this.PrivKey), cipher.final()]);
            */

            // var mac = cryptoUtils.sha3(Buffer.concat([derivedKey.slice(16, 32), new Buffer(ciphertext, 'hex')]));   // KeyVersion3 deprecated

            /*
            var mac = CryptoUtils.sha3(Buffer.concat([derivedKey.slice(16, 32), new Buffer(ciphertext, 'hex'), iv, new Buffer(opts.cipher || 'aes-128-ctr')]));
            */

            return new Key {
                version = KEYCURRENTVERSION,
                id = Guid.NewGuid().ToByteArray(),
                address = this.GetAddressString(),
                crypto = new KeyCrypto {
                    ciphertext = CryptoUtils.bufferToHex(ciphertext),
                    cipherparams = new CipherParams {
                        iv = CryptoUtils.bufferToHex(iv)
                    },
                    cipher = opts.cipher ?? "aes-128-ctr",
                    kdf = kdf,
                    kdfparams = kdfparams,
                    mac = CryptoUtils.bufferToHex(mac),
                    machash = "sha3256"
                }
            };
        }

        /**
         * Generate key buy passphrase and options.
         * Return in JSON format.
         *
         * @param {Password} password - Provided password.
         * @param {KeyOptions} opts - Key options.
         *
         * @return {String} JSON stringify Key.
         *
         * @example var key = account.toKeyString("passphrase");
         */
        public string ToKeyString(string password, KeyOptions opts)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this.ToKey(password, opts));
        }

        /**
         * Restore account from key and passphrase.
         *
         * @param {Key} input - Key Object.
         * @param {Password} password - Provided password.
         * @param {Boolean} nonStrict - Strict сase sensitivity flag.
         *
         * @return {@link Account} - Instance of Account restored from key and passphrase.
         */
        public Account FromKey(Key input, string password, bool nonStrict)
        {
            /*jshint maxcomplexity:9 */

            var json = input;
            if (json.version != KEYVERSION3 && json.version != KEYCURRENTVERSION)
            {
                throw new Exception("Not supported wallet version");
            }
            byte[] derivedKey = null;
            KDFParams kdfparams = new KDFParams();
            if (json.crypto.kdf == "scrypt")
            {
                kdfparams = json.crypto.kdfparams;
                derivedKey = ScryptUtil.Scrypt(CryptoUtils.HexStringToByteArray(password), CryptoUtils.HexStringToByteArray(kdfparams.salt), kdfparams.n, kdfparams.r, kdfparams.p, kdfparams.dklen);
            }
            else if (json.crypto.kdf == "pbkdf2")
            {
                kdfparams = json.crypto.kdfparams;
                if (kdfparams.prf != "hmac-sha256")
                {
                    throw new Exception("Unsupported parameters to PBKDF2");
                }

                RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                crypto.GetBytes(CryptoUtils.HexStringToByteArray(kdfparams.salt));
                Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(CryptoUtils.HexStringToByteArray(password), CryptoUtils.HexStringToByteArray(kdfparams.salt), kdfparams.c, HashAlgorithmName.SHA256);
                derivedKey = pbkdf2.GetBytes(kdfparams.dklen);
            }
            else
            {
                throw new Exception("Unsupported key derivation scheme");
            }
            var ciphertext = CryptoUtils.HexStringToByteArray(json.crypto.ciphertext);
            byte[] mac;

            if (json.version == KEYCURRENTVERSION)
            {
                var _derviedKey = new byte[32];
                Array.Copy(derivedKey, 16, _derviedKey, 0, 32);
                var iv = CryptoUtils.HexStringToByteArray(json.crypto.cipherparams.iv);
                var cipher = CryptoUtils.HexStringToByteArray(json.crypto.cipher);
                var _mac = new byte[_derviedKey.Length + ciphertext.Length + iv.Length + cipher.Length];
                Array.Copy(_derviedKey, 0, _mac, 0, _derviedKey.Length);
                Array.Copy(ciphertext, 0, _mac, _derviedKey.Length, ciphertext.Length);
                Array.Copy(iv, 0, _mac, _derviedKey.Length + ciphertext.Length, iv.Length);
                Array.Copy(cipher, 0, _mac, _derviedKey.Length + ciphertext.Length + iv.Length, cipher.Length);
                mac = CryptoUtils.sha3(_mac);
            }
            else
            {
                // KeyVersion3
                var _derviedKey = new byte[32];
                Array.Copy(derivedKey, 16, _derviedKey, 0, 32);
                var _mac = new byte[_derviedKey.Length + ciphertext.Length];
                mac = CryptoUtils.sha3(_mac);
            }

            if (CryptoUtils.bufferToHex(mac) != json.crypto.mac)
            {
                throw new Exception("Key derivation failed - possibly wrong passphrase");
            }

            // TODO: need to handle other algos
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = kdfparams.dklen;
            aes.IV = CryptoUtils.HexStringToByteArray(json.crypto.cipherparams.iv);
            aes.Key = new byte[16];
            Array.Copy(derivedKey, 0, aes.Key, 0, 16);
            aes.Mode = CipherMode.CTS;

            byte[] src = ciphertext;
            byte[] dest = new byte[0];

            using (ICryptoTransform decrypt = aes.CreateDecryptor())
            {
                dest = decrypt.TransformFinalBlock(src, 0, src.Length);
            }

            var _seed = new byte[src.Length + dest.Length];
            var seed = new byte[32];
            CryptoUtils.zeros(seed);
            Array.Copy(_seed, 0, seed, 32 - _seed.Length, _seed.Length);
            this.SetPrivateKey(seed);
            return this;
        }
    }
}
