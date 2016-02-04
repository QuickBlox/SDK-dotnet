using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Quickblox.Sdk.Modules.ChatXmppModule.ExtraParameters
{
    public class AcceptExtraParameter : IExtraParameter
    {
        public string SessionId { private get; set; }
        public string Sdp { private get; set; }
        public string Platform { private get; set; }

        public AcceptExtraParameter(string sessionId, string sdp, string platform)
        {
            SessionId = sessionId;
            Sdp = sdp;
            Platform = platform;
        }

        public string Build()
        {
            XDocument srcTree = new XDocument(
                new XElement("extraParams",
                    new XElement("moduleIdentifier", "WebRTCVideoChat"),
                    new XElement("signalType", SignalType.accept),
                    new XElement("sessionID", SessionId),
                    new XElement("callType", "1"),
                    new XElement("sdp", Sdp),
                    new XElement("platform", Platform)
                )
            );

            return srcTree.ToString();
        }
    }
}
