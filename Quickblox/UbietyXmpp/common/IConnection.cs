using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XMPP.common
{
    public interface IConnection : IDisposable
    {
        bool IsConnected { get; }
        string Hostname { get; }
        bool IsSSLEnabled { get;}

        void Connect();
        void Disconnect();

        void Send(tags.Tag tag);
        void Send(string message);
        void EnableSSL();
        void EnableCompression(string algorithm);
    }
}
