using System.Collections.Generic;
using System.Xml.Linq;

namespace Quickblox.Sdk.Modules.ChatXmppModule.ExtraParameters
{
    public class AcceptExtraParameter : IExtraParameter
    {
        public string SessionId { get; private set; }
        public string Sdp { get; private set; }
        public string Platform { get; private set; }
        public string UserInfo { get; private set; }
        public string CallerId { get; private set; }
        public List<string> ReceiversIds { get; private set; }
        public bool VideoCall { get; private set; }

        public AcceptExtraParameter(string sessionId, string sdp, string callerId, List<string> receiversIds, string platform, bool isVideoCall = true, string userInfo = null)
        {
            SessionId = sessionId;
            Sdp = sdp;
            Platform = platform;
            CallerId = callerId;
            ReceiversIds = receiversIds;
            UserInfo = userInfo;
            VideoCall = isVideoCall;
        }

        public XElement Build()
        {
            var extraParams = new XElement(XName.Get("extraParams", "jabber:client"),
                    new XElement("moduleIdentifier", "WebRTCVideoChat"),
                    new XElement("signalType", SignalType.accept),
                    new XElement("sessionID", SessionId),
                    new XElement("callType", VideoCall ? "1" : "2"),
                    new XElement("sdp", Sdp),
                    new XElement("callerID", CallerId),
                    new XElement("platform", Platform),
                    new XElement("userInfo", UserInfo)
                    );

            XElement opponentsIDs = new XElement("opponentsIDs");
            foreach (var id in ReceiversIds)
            {
                opponentsIDs.Add(new XElement("opponentID", id));
            }

            extraParams.Add(opponentsIDs);

            XDocument srcTree = new XDocument(extraParams);
            return srcTree.Root;
        }
    }
}
