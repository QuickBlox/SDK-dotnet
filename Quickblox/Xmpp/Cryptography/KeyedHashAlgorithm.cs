using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{

    [ComVisible(true)]
    public abstract class KeyedHashAlgorithm : HashAlgorithm
    {

        protected byte[] KeyValue;

        protected KeyedHashAlgorithm() : base()
        {
            // create a random 64 bits key
        }

        public virtual byte[] Key
        {
            get
            {
                return (byte[])KeyValue.Clone();
            }
            set
            {
                // can't change the key during a hashing ops
                if (State != 0)
                {
                    throw new ArgumentException("Key can't be changed at this state.");
                }
                // zeroize current key material for security
                ZeroizeKey();
                // copy new key
                KeyValue = (byte[])value.Clone();
            }
        }

        protected override void Dispose(bool disposing)
        {
            // zeroize key material for security
            ZeroizeKey();
            // dispose managed resources
            // none so far
            // dispose unmanaged resources 
            // none so far
            // calling base class HashAlgorithm
            base.Dispose(disposing);
        }

        private void ZeroizeKey()
        {
            if (KeyValue != null)
                Array.Clear(KeyValue, 0, KeyValue.Length);
        }

        public static new KeyedHashAlgorithm Create()
        {
		    return new System.Security.Cryptography.HMACSHA1 ();
        }
    }

}