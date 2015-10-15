using System.Xml.Linq;

namespace Quickblox.Sdk.WebRTC
{
    public class HangUpExtraParameter : IExtraParameter
    {
        public string SessionId { private get; set; }

        public HangUpExtraParameter(string sessionId)
        {
            SessionId = sessionId;
        }

        public string Build()
        {
            XDocument srcTree = new XDocument(
                new XElement("extraParams",
                    new XElement("moduleIdentifier", "WebRTCVideoChat"),
                    new XElement("signalType", SignalType.hangUp),
                    new XElement("sessionID", SessionId)
                )
            );

            return srcTree.ToString();
        }
    }
}
