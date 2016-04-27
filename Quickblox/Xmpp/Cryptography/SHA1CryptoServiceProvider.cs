using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
    [ComVisible(true)]
    public sealed class SHA1CryptoServiceProvider : SHA1
    {

        private SHA1Internal sha;

        public SHA1CryptoServiceProvider()
        {
            this.sha = new SHA1Internal();
        }

        ~SHA1CryptoServiceProvider()
        {
            this.Dispose(false);
        }

        protected override void Dispose(bool disposing)
        {
            // nothing new to do (managed implementation)
            base.Dispose(disposing);
        }

        protected override void HashCore(byte[] rgb, int ibStart, int cbSize)
        {
            this.State = 1;
            this.sha.HashCore(rgb, ibStart, cbSize);
        }

        protected override byte[] HashFinal()
        {
            this.State = 0;
            return this.sha.HashFinal();
        }

        public override void Initialize()
        {
            this.sha.Initialize();
        }
    }
}