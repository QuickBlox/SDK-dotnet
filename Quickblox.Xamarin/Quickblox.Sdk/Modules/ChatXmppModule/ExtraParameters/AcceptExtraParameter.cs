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
        //private XDocument element;

        public string SessionId { get; private set; }
        public string Sdp { get; private set; }
        public string Platform { get; private set; }

        public AcceptExtraParameter(string sessionId, string sdp, string platform)
        {
            SessionId = sessionId;
            Sdp = sdp;
            Platform = platform;
            //element = BuildAsXDocument();
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

        //public XDocument BuildAsXDocument()
        //{
        //    XDocument srcTree = new XDocument(
        //        new XElement("extraParams",
        //            new XElement("moduleIdentifier", "WebRTCVideoChat"),
        //            new XElement("signalType", SignalType.accept),
        //            new XElement("sessionID", SessionId),
        //            new XElement("callType", "1"),
        //            new XElement("sdp", Sdp),
        //            new XElement("platform", Platform)
        //        )
        //    );

        //    return srcTree;
        //}
    }
}
