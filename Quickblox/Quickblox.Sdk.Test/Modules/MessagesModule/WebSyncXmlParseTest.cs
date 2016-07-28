using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Quickblox.Sdk.Modules.ChatXmppModule;
using Quickblox.Sdk.Modules.ChatXmppModule.ExtraParameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Quickblox.Sdk.Test.Modules.MessagesModule
{
    [TestClass]
    public class WebSyncXmlParseTest
    {
        private string CallXml = "<message to=\"15302139-92@chat.quickblox.com\" id=\"57988425fe038e4dfb005bc2\" type=\"headline\" from=\"15378008-92@chat.quickblox.com/1220770403-quickblox-237573\">" +
                                "  <extraParams xmlns=\"jabber:client\">" +
                                "    <moduleIdentifier xmlns=\"\">WebRTCVideoChat</moduleIdentifier>"+
                                "    <signalType xmlns=\"\">call</signalType>"+
                                "    <sessionID xmlns=\"\">d82b2214-d8bb-4319-bc94-43fef3922eef</sessionID>"+
                                "    <callType xmlns=\"\">1</callType>" +
                                "    <sdp xmlns=\"\">v=0" +
                                "    o=- 6050036565418973184 724069890 IN IP4 127.0.0.1"+
                                "    s=IceLink"+
                                "    t = 0 0"+
                                "    m=audio 1 RTP/SAVPF 96 0 8"+
                                "    c=IN IP4 0.0.0.0"+
                                "    a=rtcp:1 IN IP4 0.0.0.0"+
                                "    a=ice-ufrag:b79ac849"+
                                "    a = ice - pwd:5693515ac8075a9c932ac89ee7a66fbb"+
                                "    a = sendrecv"+
                                "    a=rtcp-mux"+
                                "    a = fingerprint:sha-256 FC:26:5C:B1:D8:03:85:42:01:A8:DC:0C:55:AB:A6:51:85:F1:F3:14:B1:CC:56:59:7E:75:EF:09:4F:E3:D3:25"+
                                "    a=setup:actpass"+
                                "    a = rtpmap:96 opus/48000/2"+
                                "    a=rtpmap:0 PCMU/8000"+
                                "    a=rtpmap:8 PCMA/8000"+
                                "    a=ssrc:3824359383 cname:e228551c"+
                                "    m = video 1 RTP/SAVPF 97 98 99"+
                                "    c=IN IP4 0.0.0.0"+
                                "    a=rtcp:1 IN IP4 0.0.0.0"+
                                "    a=ice-ufrag:b79ac849"+
                                "    a = ice - pwd:5693515ac8075a9c932ac89ee7a66fbb"+
                                "    a = sendrecv"+
                                "    a=rtcp-mux"+
                                "    a = fingerprint:sha-256 FC:26:5C:B1:D8:03:85:42:01:A8:DC:0C:55:AB:A6:51:85:F1:F3:14:B1:CC:56:59:7E:75:EF:09:4F:E3:D3:25"+
                                "    a=setup:actpass"+
                                "    a = rtpmap:97 VP8/90000"+
                                "    a=rtpmap:98 red/90000"+
                                "    a=rtpmap:99 ulpfec/90000"+
                                "    a=ssrc:754406204 cname:e228551c"+
                                "    a = rtcp - fb:97 nack"+
                                "    a = rtcp - fb:97 nack pli"+
                                "    </sdp>"+
                                "    <platform xmlns=\"\">android</platform>" +
                                "    <callerID xmlns=\"\">15378008</callerID>" +
                                "    <opponentsIDs xmlns=\"\">" +
                                "        <opponentID>15302139</opponentID>"+
                                "        <opponentID>15302140</opponentID>" +
                                "    </opponentsIDs>"+
                                "    </extraParams>"+
                                "    <body>Call</body>"+
                                "</message>";

        private string IceCandidateXml = "<message to=\"15302139-92@chat.quickblox.com\" id=\"57988425fe038e4dfb005bc2\" type=\"headline\" from=\"15378008-92@chat.quickblox.com/1220770403-quickblox-237573\">" +
                                        "<extraParams xmlns=\"jabber:client\">" +
                                        "<moduleIdentifier>WebRTCVideoChat</moduleIdentifier>"+
                                    "    <signalType>call</signalType>"+
                                    "    <sessionID>d82b2214-d8bb-4319-bc94-43fef3922eef</sessionID>"+
                                    "    <callType>1</callType>"+
                                    "  <callerID>15378008</callerID>"+
                                            "    <opponentsIDs>"+
                                            "        <opponentID>15302139</opponentID>"+
                                            "        <opponentID>15302140</opponentID>" +
                                            "    </opponentsIDs>"+
                                            "<iceCandidates>"+
                                                "<iceCandidate>"+
                                                   " <sdpMLineIndex>01231</sdpMLineIndex>"+
                                                    "<sdpMid>video</sdpMid>" +
            	                                    "<candidate>1243234</candidate>"+
                                                "</iceCandidate>"+
                                                "<iceCandidate>"+
            	                                    "<sdpMLineIndex>23414</sdpMLineIndex>"+
            	                                    "<sdpMid>video</sdpMid>"+
            	                                    "<candidate>234234</candidate>"+
                                                " </iceCandidate>"+
                                            " </iceCandidates>"+
                                        "</extraParams>"+
                                    "</message>";



        private string webRTCVideoChat = "WebRTCVideoChat";

        [TestMethod]
        public void TestBuildAcceptXml()
        {
            var ac = new AcceptExtraParameter(Guid.NewGuid().ToString(), "asdasdasdasd", "CallerId", new List<string>() { "11111", "22222" }, "ios", true, "full userName");
            var build = ac.Build();
            var stringValue = build.Value;
        }

        [TestMethod]
        public void ParseCallXml ()
        {
            //var main = XElement.Parse(IceCandidateXml);
            var main = XElement.Parse(CallXml);
            var elements = main.Elements(XName.Get("extraParams", "jabber:client")).FirstOrDefault();
            var element = elements.Elements(XName.Get("moduleIdentifier", "jabber:client")).FirstOrDefault();
            if (element == null || element.Value != webRTCVideoChat)
                return;

            var videoChatMessage = new VideoChatMessage();

            //videoChatMessage.Caller = ChatXmppClient.GetQbUserIdFromJid(e.Message.From);
            //videoChatMessage.Receiver = ChatXmppClient.GetQbUserIdFromJid(e.Message.To);

            var session = elements.Elements(XName.Get("sessionID", "jabber:client")).FirstOrDefault();
            videoChatMessage.SessionId = session != null ? session.Value : null;
            var sdp = elements.Elements(XName.Get("sdp", "jabber:client")).FirstOrDefault();
            videoChatMessage.Sdp = sdp != null ? sdp.Value : null;

            var callerID = elements.Elements(XName.Get("callerID", "jabber:client")).FirstOrDefault();
            videoChatMessage.Caller = callerID != null ? callerID.Value : null;


            var signalType = elements.Elements(XName.Get("signalType", "jabber:client")).FirstOrDefault();
            videoChatMessage.Signal = (SignalType)Enum.Parse(typeof(SignalType), signalType.Value);

            var userInfo = elements.Elements(XName.Get("userInfo", "jabber:client")).FirstOrDefault();
            videoChatMessage.UserInfo = userInfo != null ? userInfo.Value : "";

            var platform = elements.Elements(XName.Get("platform", "jabber:client")).FirstOrDefault();
            videoChatMessage.Platform = platform != null ? platform.Value : "";

            var videoCall = elements.Elements(XName.Get("callType", "jabber:client")).FirstOrDefault();
            videoChatMessage.VideoCall = Int32.Parse(videoCall.Value) == 1 ? true : false;

            var opponentsIds = elements.Elements(XName.Get("opponentsIDs", "jabber:client")).FirstOrDefault();
            if (opponentsIds != null)
            {
                var listIds = new Collection<string>();
                foreach (var idsElement in opponentsIds.Elements())
                {
                    listIds.Add(idsElement.Value);
                }

                videoChatMessage.OpponentsIds = listIds;
            }

            var iceCandidates = elements.Elements(XName.Get("iceCandidates", "jabber:client")).FirstOrDefault();
            if (iceCandidates != null)
            {
                var iceCandidatesList = new Collection<IceCandidate>();
                foreach (var iceCandidateElement in iceCandidates.Elements())
                {
                    var candidateElement = iceCandidateElement.Elements(
                        XName.Get("candidate", "jabber:client")).FirstOrDefault();
                    var sdpMLineIndexElement = iceCandidateElement.Elements(XName.Get("sdpMLineIndex", "jabber:client")).FirstOrDefault();
                    var sdpMidElement = iceCandidateElement.Elements(XName.Get("sdpMid", "jabber:client")).FirstOrDefault();
                    iceCandidatesList.Add(new IceCandidate()
                    {
                        Candidate = candidateElement.Value,
                        SdpMid = sdpMidElement.Value,
                        SdpMLineIndex = sdpMLineIndexElement.Value
                    });
                }

                videoChatMessage.IceCandidates = iceCandidatesList;
            }
        }
    }
}
