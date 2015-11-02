using System.Xml.Linq;

namespace Quickblox.Sdk.WebRTC
{
    public class CallExtraParameter : IExtraParameter
    {
        public string SessionId { private get; set; }
        public string Sdp { private get; set; }
        public string Platform { private get; set; }
        public int SenderId { private get; set; }
        public int ReceiverId { private get; set; }

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
