using System.Xml.Linq;

namespace Quickblox.Sdk.Modules.ChatXmppModule.ExtraParameters
{
    public class CallExtraParameter : IExtraParameter
    {
        public string SessionId { get; private set; }
        public string Sdp { get; private set; }
        public string Platform { get; private set; }
        public int SenderId { get; private set; }
        public int ReceiverId { get; private set; }

        public CallExtraParameter(string sessionId, string sdp, string platform, int senderId, int receiverId)
        {
            SessionId = sessionId;
            Sdp = sdp;
            Platform = platform;
            SenderId = senderId;
            ReceiverId = receiverId;
        }

        public string Build()
        {
            var srcTree = new XDocument(
                new XElement("extraParams",
                    new XElement("moduleIdentifier", "WebRTCVideoChat"),
                    new XElement("signalType", SignalType.call),
                    new XElement("sessionID", SessionId),
                    new XElement("callType", "1"),
                    new XElement("sdp", Sdp),
                    new XElement("platform", Platform),
                    new XElement("callerID", SenderId),
                    new XElement("opponentsIDs",
                       new XElement("opponentID", ReceiverId))
                )
            );

            return srcTree.ToString();
        }
    }
}
