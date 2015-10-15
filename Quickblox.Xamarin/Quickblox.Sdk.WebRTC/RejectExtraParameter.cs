using System.Xml.Linq;

namespace Quickblox.Sdk.WebRTC
{
    public class RejectExtraParameter : IExtraParameter
    {
        public string SessionId { private get; set; }

        public RejectExtraParameter(string sessionId)
        {
            SessionId = sessionId;
        }

        public string Build()
        {
            XDocument srcTree = new XDocument(
                new XElement("extraParams",
                    new XElement("moduleIdentifier", "WebRTCVideoChat"),
                    new XElement("signalType", SignalType.reject),
                    new XElement("sessionID", SessionId)
                )
            );

            return srcTree.ToString();
        }
    }
}
