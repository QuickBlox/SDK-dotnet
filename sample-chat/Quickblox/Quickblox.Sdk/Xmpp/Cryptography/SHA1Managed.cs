using System.Runtime.InteropServices;

namespace Xmpp.Cryptography
{
    [ComVisible(true)]
    public class SHA1Managed : SHA1
    {

        private SHA1Internal sha;

        public SHA1Managed()
        {
            this.sha = new SHA1Internal();
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