using System.Collections.Generic;
using System.Xml.Linq;
using System.Collections.ObjectModel;

namespace Quickblox.Sdk.WebRTC
{
    public class IceCandidatesExtraParameter : IExtraParameter
    {
        public string SessionId { private get; set; }

        public Collection<IceCandidate> IceCandidates { get; set; }

		public IceCandidatesExtraParameter(string sessionId, Collection<IceCandidate> iceCandidates)
        {
            SessionId = sessionId;
            IceCandidates = iceCandidates;
        }

        public string Build()
        {
            var extraParams = new XElement("extraParams",
                    new XElement("moduleIdentifier", "WebRTCVideoChat"),
                    new XElement("signalType", SignalType.iceCandidates),
                    new XElement("sessionID", SessionId));

            XElement iceCandidates = new XElement("iceCandidates");
                        foreach (var item in IceCandidates)
            {
                iceCandidates.Add(new XElement("iceCandidate",
                        new XElement("sdpMLineIndex", item.SdpMLineIndex),
                        new XElement("sdpMid", item.SdpMid),
                        new XElement("candidate", item.Candidate)));
            }
            extraParams.Add(iceCandidates);

            XDocument srcTree = new XDocument(extraParams);    
            return srcTree.ToString();
        }
    }
}
