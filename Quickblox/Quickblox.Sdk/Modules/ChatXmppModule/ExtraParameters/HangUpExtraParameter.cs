using System.Xml.Linq;

namespace Quickblox.Sdk.Modules.ChatXmppModule.ExtraParameters
{
    public class HangUpExtraParameter : IExtraParameter
    {
        public string SessionId {  get; private set; }

        public HangUpExtraParameter(string sessionId)
        {
            SessionId = sessionId;
        }

        public XElement Build()
        {
            XDocument srcTree = new XDocument(
                new XElement("extraParams",
                    new XElement("moduleIdentifier", "WebRTCVideoChat"),
                    new XElement("signalType", SignalType.hangUp),
                    new XElement("sessionID", SessionId)
                )
            );

            return srcTree.Root;
        }
    }
}
