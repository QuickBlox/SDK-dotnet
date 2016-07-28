using System;
using System.Xml.Linq;
using System.Linq;
using System.Collections.ObjectModel;
using Quickblox.Sdk.Modules.ChatXmppModule.ExtraParameters;
using Xmpp.Im;
using System.Collections.Generic;

namespace Quickblox.Sdk.Modules.ChatXmppModule
{
	public class VideoChatMessage {
        public string SessionId { get; set; }
        public int Sender { get; set; }
		public int Receiver {get; set;}
        public string Caller { get; set; }

        public string Platform { get; set; }
        public string UserInfo { get; set; }
        public bool VideoCall { get; set; }
        public SignalType Signal { get; set; }
		public string Sdp {get; set;}
        public Collection<string> OpponentsIds { get; set; }
		public Collection<IceCandidate> IceCandidates {get; set;}
	}

    /// <summary>
    /// The signaling in the QuickBox WebRTC module is implemented over the XMPP protocol using QuickBlox Chat Module. 
    /// </summary>
    public class WebSyncClient
    {
		const string webRTCVideoChat = "WebRTCVideoChat";
		public event EventHandler<VideoChatMessage> VideoChatMessage;
        QuickbloxClient client;

        public WebSyncClient(QuickbloxClient client)
        {
            this.client = client;
            this.client.ChatXmppClient.MessageReceived += OnMessageReceived;
        }

        /// <summary>
        /// Calls the specified session identifier.
        /// <message to="..."  type="headline" id="...">
        /// <extraParams xmlns = "jabber:client" >
        ///  < moduleIdentifier > WebRTCVideoChat </ moduleIdentifier >
        /// < signalType > call </ signalType >
        /// < sessionID >...</ sessionID >
        /// < callType >...</ callType >
        /// < sdp >...</ sdp >
        /// < platform >...</ platform >
        /// < callerID >...</ callerID >
        /// < opponentsIDs >
        ///  < opponentID >...</ opponentID >
        ///  < opponentID >...</ opponentID >
        /// </ opponentsIDs >
        /// </ extraParams >
        /// </ message >
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="sdp">The SDP.</param>
        /// <param name="platform">The platform.</param>
        /// <param name="caller">The caller.</param>
        /// <param name="receiver">The receiver.</param>
        /// <returns></returns>
		public CallExtraParameter Call(string sessionId, string sdp, string caller, string receiver, List<string> opponentsIds, string platform, bool isVideoCall = true, MessageType type = MessageType.Headline, string userInfo = null)
        {
            var jid = ChatXmppClient.BuildJid(receiver, client.ApplicationId, client.ChatEndpoint);
            var extraParameter = new CallExtraParameter(sessionId, sdp, caller, opponentsIds, platform, isVideoCall, userInfo);
			this.client.ChatXmppClient.SendMessage(jid, "Call", extraParameter.Build(), null, null, type);
            return extraParameter;
        }

        /// <summary>
        /// Accepts the specified session identifier.
        /// <message to="..."  type="headline" id="...">
        /// <extraParams xmlns = "jabber:client" >
        ///  < moduleIdentifier > WebRTCVideoChat </ moduleIdentifier >
        /// < signalType > accept </ signalType >
        /// < sessionID >...</ sessionID >
        /// < callType >...</ callType >
        /// < sdp >...</ sdp >
        /// < platform >...</ platform >
        /// </ extraParams >
        /// </ message >
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="sdp">The SDP.</param>
        /// <param name="platform">The platform.</param>
        /// <param name="caller">The caller.</param>
        /// <returns></returns>
        public AcceptExtraParameter Accept(string sessionId, string sdp, string caller, string receiver, List<string> opponentsIds, string platform, bool isVideoCall = true, MessageType type = MessageType.Headline, string userInfo = null)
        {
            var jid = ChatXmppClient.BuildJid(receiver, client.ApplicationId, client.ChatEndpoint);
            var extraParameter = new AcceptExtraParameter(sessionId, sdp, caller, opponentsIds, platform, isVideoCall, userInfo);
            this.client.ChatXmppClient.SendMessage(jid, "Accept", extraParameter.Build(), null, null, type);
            return extraParameter;
        }

        /// <summary>
        /// Rejects the specified session identifier.
        /// <message to="..."  type="headline" id="...">
        /// <extraParams xmlns = "jabber:client" >
        ///  < moduleIdentifier > WebRTCVideoChat </ moduleIdentifier >
        /// < signalType > reject </ signalType >
        /// < sessionID >...</ sessionID >
        /// </ extraParams >
        /// </ message >
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="caller">The caller.</param>
        /// <returns></returns>
        public RejectExtraParameter Reject(string sessionId, string caller, string receiver, List<string> opponentsIds, string platform, bool isVideoCall = true, MessageType type = MessageType.Headline, string userInfo = null)
        {
            var jid = ChatXmppClient.BuildJid(receiver, client.ApplicationId, client.ChatEndpoint);
            var extraParameter = new RejectExtraParameter(sessionId, caller, opponentsIds, platform, isVideoCall, userInfo);
            this.client.ChatXmppClient.SendMessage(jid, "Reject", extraParameter.Build(), null, null, type);
            return extraParameter;
        }

        /// <summary>
        /// Hangs up.
        /// <message to="..."  type="headline" id="...">
        /// <extraParams xmlns = "jabber:client" >
        ///  < moduleIdentifier > WebRTCVideoChat </ moduleIdentifier >
        /// < signalType > hangUp </ signalType >
        /// < sessionID >...</ sessionID >
        /// </ extraParams >
        /// </ message >
        /// </summary>
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="caller">The caller.</param>
        /// <returns></returns>
        public HangUpExtraParameter HangUp(string sessionId, string caller, string receiver, List<string> opponentsIds, string platform, bool isVideoCall = true, MessageType type = MessageType.Headline, string userInfo = null)
        {
            var jid = ChatXmppClient.BuildJid(receiver, client.ApplicationId, client.ChatEndpoint);
            var extraParameter = new HangUpExtraParameter(sessionId, caller, opponentsIds, platform, isVideoCall, userInfo);
            this.client.ChatXmppClient.SendMessage(jid, "HangUp", extraParameter.Build(), null, null, type);
            return extraParameter;
        }

        /// <summary>
        /// Ices the candidates.
        /// <message to="..."  type="headline" id="...">
        ///<extraParams xmlns = "jabber:client" >
        ///< moduleIdentifier > WebRTCVideoChat </ moduleIdentifier >
        /// < signalType > iceCandidates </ signalType >
        /// < sessionID >...</ sessionID >
        /// < iceCandidates >
        ///    < iceCandidate >
        ///        < sdpMLineIndex >...</ sdpMLineIndex >
        ///        < sdpMid >...</ sdpMid >
        ///        < candidate >...</ candidate >
        ///    </ iceCandidate >
        ///    < iceCandidate >
        ///        < sdpMLineIndex >...</ sdpMLineIndex >
        ///       < sdpMid >...</ sdpMid >
        ///        < candidate >...</ candidate >
        ///     </ iceCandidate >
        /// </ iceCandidates >
        /// </ extraParams >
        /// </ message >
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="iceCandidates">The ice candidates.</param>
        /// <param name="caller">The caller.</param>
        /// <returns></returns>
        public IceCandidatesExtraParameter IceCandidates(string sessionId, Collection<IceCandidate> iceCandidates, string caller, string receiver, List<string> opponentsIds, string platform, bool isVideoCall = true, MessageType type = MessageType.Headline, string userInfo = null)
        {
            var jid = ChatXmppClient.BuildJid(receiver, client.ApplicationId, client.ChatEndpoint);
            var extraParameter = new IceCandidatesExtraParameter(sessionId, iceCandidates, caller, opponentsIds, platform, isVideoCall, userInfo);
			this.client.ChatXmppClient.SendMessage(jid, "IceCandidates", extraParameter.Build(), null, null, type);
            return extraParameter;
        }

        /// <summary>
        /// Called when message received. Parse data and retun VideoChatMessage instance
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MessageEventArgs"/> instance containing the event data.</param>
        private void OnMessageReceived(object sender, MessageEventArgs e)
        {
            if (e.MessageType == MessageType.Headline)
            {
                //var extraParameters = e.Message.ExtraParameters;
                //var elements = XElement.Parse(extraParameters);

                var elements = e.Message.ExtraParameters;

                var element = elements.Elements (XName.Get("moduleIdentifier", "jabber:client")).FirstOrDefault();
				if (element == null || element.Value != webRTCVideoChat) 
					return;

				var videoChatMessage = new VideoChatMessage ();

				videoChatMessage.Sender = ChatXmppClient.GetQbUserIdFromJid(e.Message.From);
				videoChatMessage.Receiver = ChatXmppClient.GetQbUserIdFromJid(e.Message.To);

				var session = elements.Elements (XName.Get("sessionID", "jabber:client")).FirstOrDefault ();
				videoChatMessage.SessionId = session != null ? session.Value : null;
				var sdp = elements.Elements (XName.Get("sdp", "jabber:client")).FirstOrDefault ();
				videoChatMessage.Sdp = sdp != null ? sdp.Value : null;

                var callerID = elements.Elements(XName.Get("callerID", "jabber:client")).FirstOrDefault();
                videoChatMessage.Caller = callerID != null ? callerID.Value : null;

                var signalType =  elements.Elements (XName.Get("signalType", "jabber:client")).FirstOrDefault ();
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

                var iceCandidates = elements.Elements (XName.Get("iceCandidates", "jabber:client")).FirstOrDefault ();
				if (iceCandidates != null) {
					var iceCandidatesList = new Collection<IceCandidate> ();
					foreach (var iceCandidateElement in iceCandidates.Elements()) {
						var candidateElement = iceCandidateElement.Elements(
                            XName.Get("candidate", "jabber:client")).FirstOrDefault ();
						var sdpMLineIndexElement = iceCandidateElement.Elements(XName.Get("sdpMLineIndex", "jabber:client")).FirstOrDefault ();
						var sdpMidElement = iceCandidateElement.Elements(XName.Get("sdpMid", "jabber:client")).FirstOrDefault ();
						iceCandidatesList.Add (new IceCandidate () {
							Candidate = candidateElement.Value,
							SdpMid = sdpMidElement.Value,
							SdpMLineIndex = sdpMLineIndexElement.Value
						});
					} 

					videoChatMessage.IceCandidates = iceCandidatesList;
				}

                //Parse Message and raise event
                var handler = VideoChatMessage;
                if (handler != null)
                {
					handler.Invoke(this, videoChatMessage);
                }
            }
        }        
    }
}
