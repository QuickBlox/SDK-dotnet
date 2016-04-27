using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{

    [ComVisible(true)]
    public abstract class MD5 : HashAlgorithm
    {

        // Why is it protected when others abstract hash classes are public ?
        protected MD5()
        {
            HashSizeValue = 128;
        }

        public static new MD5 Create()
        {
			return new System.Security.Cryptography.MD5CryptoServiceProvider ();
        }
    }
}