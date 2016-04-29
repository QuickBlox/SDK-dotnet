using System;
using System.Xml.Linq;
using System.Linq;
using System.Collections.ObjectModel;
using Quickblox.Sdk.Modules.ChatXmppModule.ExtraParameters;
using Quickblox.Sdk.Modules.ChatXmppModule.Models;

namespace Quickblox.Sdk.Modules.ChatXmppModule
{
	public class VideoChatMessage {
		public string Guid { get; set; }
		public int Caller { get; set; }
		public int Receiver {get; set;}
		public SignalType Signal { get; set; }
		public string Sdp {get; set;}
		public Collection<IceCandidate> IceCandidates {get; set;}
	}

    /// <summary>
    /// The signaling in the QuickBox WebRTC module is implemented over the XMPP protocol using QuickBlox Chat Module. 
    /// </summary>
    public class WebSyncClient
    {
		const string webRTCVideoChat = "WebRTCVideoChat";
		public event EventHandler<VideoChatMessage> VideoChatMessage;
        IQuickbloxClient client;

        public WebSyncClient(IQuickbloxClient client)
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
		public CallExtraParameter Call(string sessionId, string sdp, string platform, int caller, int receiver, MessageType type = MessageType.Headline)
        {
            var extraParameter = new CallExtraParameter(sessionId, sdp, platform, caller, receiver);
			this.client.ChatXmppClient.SendMessage(receiver, "Call", extraParameter.Build(), null, null, type);
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
        public AcceptExtraParameter Accept(string sessionId, string sdp, string platform, int receiver)
        {
            var jid = ChatXmppClient.BuildJid(receiver, client.ApplicationId, client.ChatEndpoint);
            var extraParameter = new AcceptExtraParameter(sessionId, sdp, platform);
			this.client.ChatXmppClient.SendMessage(receiver, "Accept", extraParameter.Build(), null, null, MessageType.Headline);
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
        public RejectExtraParameter Reject(string sessionId, int receiver)
        {
            var jid = ChatXmppClient.BuildJid(receiver, client.ApplicationId, client.ChatEndpoint);
            var extraParameter = new RejectExtraParameter(sessionId);
			this.client.ChatXmppClient.SendMessage(receiver, "Reject", extraParameter.Build(), null, null, MessageType.Headline);
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
        public HangUpExtraParameter HangUp(string sessionId, int receiver)
        {
            var jid = ChatXmppClient.BuildJid(receiver, client.ApplicationId, client.ChatEndpoint);
            var extraParameter = new HangUpExtraParameter(sessionId);
			this.client.ChatXmppClient.SendMessage(receiver, "HangUp", extraParameter.Build(), null, null, MessageType.Headline);
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
        public IceCandidatesExtraParameter IceCandidates(string sessionId, int receiver, Collection<IceCandidate> iceCandidates)
        {
            var jid = ChatXmppClient.BuildJid(receiver, client.ApplicationId, client.ChatEndpoint);
            var extraParameter = new IceCandidatesExtraParameter(sessionId, iceCandidates);
			this.client.ChatXmppClient.SendMessage(receiver, "IceCandidates", extraParameter.Build(), null, null, MessageType.Headline);
            return extraParameter;
        }

        private void OnMessageReceived(object sender, MessageEventArgs e)
        {
            if (e.MessageType == MessageType.Headline)
            {
                //var extraParameters = e.Message.ExtraParameter;
                //var elements = XElement.Parse(extraParameters);

                var elements = e.Message.ExtraParameters;

                var element = elements.Elements (XName.Get("moduleIdentifier", "jabber:client")).FirstOrDefault();
				if (element == null || element.Value != webRTCVideoChat) 
					return;

				var videoChatMessage = new VideoChatMessage ();

				videoChatMessage.Caller = ChatXmppClient.GetQbUserIdFromJid(e.Message.From);
				videoChatMessage.Receiver = ChatXmppClient.GetQbUserIdFromJid(e.Message.To);

				var session = elements.Elements (XName.Get("sessionID", "jabber:client")).FirstOrDefault ();
				videoChatMessage.Guid = session != null ? session.Value : null;
				var sdp = elements.Elements (XName.Get("sdp", "jabber:client")).FirstOrDefault ();
				videoChatMessage.Sdp = sdp != null ? sdp.Value : null;

				var signalType =  elements.Elements (XName.Get("signalType", "jabber:client")).FirstOrDefault ();
				videoChatMessage.Signal = (SignalType)Enum.Parse(typeof(SignalType), signalType.Value);
					
				var iceCandidates = elements.Elements (XName.Get("iceCandidates", "jabber:client")).FirstOrDefault ();
				if (iceCandidates != null) {
					var iceCandidatesList = new Collection<IceCandidate> ();
					foreach (var iceCandidateElement in iceCandidates.Elements()) {
						var candidateElement = iceCandidateElement.Elements(XName.Get("candidate", "jabber:client")).FirstOrDefault ();
						var sdpMLineIndexElement = iceCandidateElement.Elements(XName.Get("sdpMLineIndex", "jabber:client")).FirstOrDefault ();
						//var sdpMidElement = iceCandidateElement.Elements(XName.Get("sdpMid", "jabber:client")).FirstOrDefault ();
						iceCandidatesList.Add (new IceCandidate () {
							Candidate = candidateElement.Value,
							//SdpMid = sdpMidElement.Value,
							SdpMLineIndex = sdpMLineIndexElement.Value
						});
					} 

					videoChatMessage.IceCandidates = iceCandidatesList;
				}

                //Parse message and raise event
                var handler = VideoChatMessage;
                if (handler != null)
                {
					handler.Invoke(this, videoChatMessage);
                }
            }
        }        
    }
}
