using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
    [ComVisible(true)]
    public abstract class SHA1 : HashAlgorithm
    {

        protected SHA1()
        {
            HashSizeValue = 160;
        }

        public static new SHA1 Create()
        {
			return new System.Security.Cryptography.SHA1CryptoServiceProvider ();
        }
    }
}
