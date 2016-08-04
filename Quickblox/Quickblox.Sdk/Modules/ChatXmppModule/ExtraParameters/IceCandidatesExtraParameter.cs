using System.Collections.Generic;
using System.Xml.Linq;
using System.Collections.ObjectModel;

namespace Quickblox.Sdk.Modules.ChatXmppModule.ExtraParameters
{
    public class IceCandidatesExtraParameter : IExtraParameter
    {
        public string SessionId { get; private set; }
        public string Platform { get; private set; }
        public string UserInfo { get; private set; }
        public string CallerId { get; private set; }
        public List<string> ReceiversIds { get; private set; }
        public bool VideoCall { get; private set; }
        public Collection<IceCandidate> IceCandidates { get; private set; }

        public IceCandidatesExtraParameter(string sessionId, Collection<IceCandidate> iceCandidates, string callerId, List<string> receiversIds, string platform, bool isVideoCall = true, string userInfo = null)
        {
            IceCandidates = iceCandidates;
            SessionId = sessionId;
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
                    new XElement(XName.Get("signalType", "jabber:client"), SignalType.iceCandidates),
                    new XElement(XName.Get("sessionID", "jabber:client"), SessionId),
                    new XElement(XName.Get("callType", "jabber:client"), VideoCall ? "1" : "2"),
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

            XElement iceCandidates = new XElement(XName.Get("iceCandidates", "jabber:client"));
                        foreach (var item in IceCandidates)
            {
                iceCandidates.Add(new XElement(XName.Get("iceCandidate", "jabber:client"),
                        new XElement(XName.Get("sdpMLineIndex", "jabber:client"), item.SdpMLineIndex),
                        new XElement(XName.Get("sdpMid", "jabber:client"), item.SdpMid),
                        new XElement(XName.Get("candidate", "jabber:client"), item.Candidate)));
            }
            extraParams.Add(iceCandidates);

            XDocument srcTree = new XDocument(extraParams);    
            return srcTree.Root;
        }
    }
}
