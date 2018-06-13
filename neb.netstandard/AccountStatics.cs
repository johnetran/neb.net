using Cryptography.ECDSA;
using Nebulas.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Nebulas
{
    public partial class Account
    {

        // static methods

        /**
         * Account factory method.
         * Create random account.
         * @static
         *
         * @return {Account} Instance of Account constructor.
         *
         * @example var account = Account.NewAccount();
         */
        public static Account NewAccount()
        {
            var bytes = new byte[] { 240, 170, 211, 237, 97, 118, 219, 33, 224, 233, 43, 207, 234, 38, 114, 198, 195, 38, 191, 187, 33, 59, 188, 253, 182, 125, 105, 211, 147, 56, 166, 25 };
            return new Account(bytes);
            //return new Account(CryptoUtils.randomBytes(32));
        }

        /**
         * Address validation method.
         *
         * @static
         * @param {String/Hash} addr - Account address.
         * @param {Number} type - NormalType / ContractType
         *
         * @return {Boolean} Is address has correct format.
         *
         * @example
         * if ( Account.isValidAddress("n1QZMXSZtW7BUerroSms4axNfyBGyFGkrh5") ) {
         *     // some code
         * };
         */
        public static bool isValidAddress(string addr, int type)
        {
            try
            {
                var addrBuf = Base58.Decode(addr);
                return isValidAddress(addrBuf, type);
            }
            catch (Exception e)
            {
                //console.log("invalid address.");
                // if address can't be base58 decode, return false.
                return false;
            }

        }

        public static bool isValidAddress(byte[] addr, int type)
        {
            // address not equal to 26
            if (addr.Length != ADDRESSLENGTH)
            {
                return false;
            }

            // check if address start with AddressPrefix
            var buff = addr;
            if (buff[0] != ADDRESSPREFIX)
            {
                return false;
            }

            // check if address type is NormalType or ContractType
            var t = buff[1];
            if (type == NORMALTYPE || type == CONTRACTTYPE)
            {
                if (t != type)
                {
                    return false;
                }
            }
            else if (t != NORMALTYPE && t != CONTRACTTYPE)
            {
                return false;
            }
            var content = new byte[22];
            Array.Copy(addr, 0, content, 0, 22);

            var checksum = new byte[4];
            Array.Copy(addr, addr.Length - 4, checksum, 0, 4);

            var shaContent = CryptoUtils.sha3(content);
            var shaChecksum = new byte[4];
            Array.Copy(shaContent, shaChecksum, 4);

            return CompareByteArrays(shaChecksum, checksum) == 0;
        }

        public static int CompareByteArrays(byte[] array1, byte[] array2)
        {
            return array1.Where((x, i) => x != array2[i]).Count();
        }

        /**
         * Restore account from address.
         * Receive addr or Account instance.
         * If addr is Account instance return new Account instance with same PrivateKey.
         *
         * @static
         * @param {(Hash|Object)} - Client address or Account instance.
         *
         * @return {Account} Instance of Account restored from address.
         *
         * @example var account = Account.fromAddress("n1QZMXSZtW7BUerroSms4axNfyBGyFGkrh5");
         */
        public static Account fromAddress(Account addr)
        {
            var acc = new Account();
            acc.SetPrivateKey(addr.GetPrivateKey());
            return acc;
        }
        public static Account fromAddress(string addr)
        {

            var acc = new Account();
            if (isValidAddress(addr, 0))
            {
                acc.Address = Base58.Decode(addr);
                return acc;
            }

            var buf = CryptoUtils.toBuffer(addr);
            if (isValidAddress(buf, 0))
            {
                acc.Address = buf;
                return acc;
            }

            return null;
        }


        public static int getNormalType()
        {
            return NORMALTYPE;
        }

        public static int getContractType()
        {
            return CONTRACTTYPE;
        }
    }
}
