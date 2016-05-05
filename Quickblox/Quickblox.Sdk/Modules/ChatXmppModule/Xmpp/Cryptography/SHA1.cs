using System.Runtime.InteropServices;

namespace Xmpp.Cryptography
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
			return new SHA1CryptoServiceProvider ();
        }
    }
}
