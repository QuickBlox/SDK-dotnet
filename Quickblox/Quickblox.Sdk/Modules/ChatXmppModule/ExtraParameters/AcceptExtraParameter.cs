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
                    new XElement(XName.Get("moduleIdentifier", "jabber:client"), "WebRTCVideoChat"),
                    new XElement(XName.Get("signalType", "jabber:client"), SignalType.accept),
                    new XElement(XName.Get("sessionID", "jabber:client"), SessionId),
                    new XElement(XName.Get("callType", "jabber:client"), VideoCall ? "1" : "2"),
                    new XElement(XName.Get("sdp", "jabber:client"), Sdp),
                    new XElement(XName.Get("callerID", "jabber:client"), CallerId),
                    new XElement(XName.Get("platform", "jabber:client"), Platform),
                    new XElement(XName.Get("userInfo", "jabber:client"), UserInfo)
                    );

            XElement opponentsIDs = new XElement(XName.Get("opponentsIDs", "jabber:client"));
            foreach (var id in ReceiversIds)
            {
                opponentsIDs.Add(new XElement(XName.Get("opponentID", "jabber:client"), id));
            }

            extraParams.Add(opponentsIDs);

            XDocument srcTree = new XDocument(extraParams);
            return srcTree.Root;
        }
    }
}
