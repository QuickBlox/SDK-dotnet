using System.Collections.Generic;
using System;
using Quickblox.Sdk.Modules.MessagesModule.Models;

namespace Quickblox.Sdk.WebRTC
{
    public class WebSyncClient
    {
        public event EventHandler<Message> HeadlineReceive;
        
        IQuickbloxClient client;
        int appId;

        public WebSyncClient(IQuickbloxClient client)
        {
            this.client = client;
            this.client.MessagesClient.OnMessageReceived += OnMessageReceived;
            appId = this.client.MessagesClient.AppId;
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
        public CallExtraParameter Call(string sessionId, string sdp, string platform, string caller, string receiver)
        {
            //var jid = BuildJid(caller);
            var jid = BuildSmallJid(caller);
            var extraParameter = new CallExtraParameter(sessionId, sdp, platform, caller, receiver);
            this.client.MessagesClient.SendMessage(jid.ToString(), null, extraParameter.Build(), null, MessageType.Headline);
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
        public AcceptExtraParameter Accept(string sessionId, string caller, string sdp, string platform)
        {
            // jid = BuildJid(caller);
            var jid = BuildSmallJid(caller);
            var extraParameter = new AcceptExtraParameter(sessionId, sdp, platform);
            this.client.MessagesClient.SendMessage(jid.ToString(), null, extraParameter.Build(), null, MessageType.Headline);
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
        public RejectExtraParameter Reject(string sessionId, string caller)
        {
            // jid = BuildJid(caller);
            var jid = BuildSmallJid(caller);
            var extraParameter = new RejectExtraParameter(sessionId);
            this.client.MessagesClient.SendMessage(jid.ToString(), null, extraParameter.Build(), null, MessageType.Headline);
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
        public HangUpExtraParameter HangUp(string sessionId, string caller)
        {
            // jid = BuildJid(caller);
            var jid = BuildSmallJid(caller);
            var extraParameter = new HangUpExtraParameter(sessionId);
            this.client.MessagesClient.SendMessage(jid.ToString(), null, extraParameter.Build(), null, MessageType.Headline);
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
        public IceCandidatesExtraParameter IceCandidates(string sessionId, string caller, List<IceCandidate> iceCandidates)
        {
            // jid = BuildJid(caller);
            var jid = BuildSmallJid(caller);
            var extraParameter = new IceCandidatesExtraParameter(sessionId, iceCandidates);
            this.client.MessagesClient.SendMessage(jid.ToString(), null, extraParameter.Build(), null, MessageType.Headline);
            return extraParameter;
        }

        private void OnMessageReceived(object sender, Modules.MessagesModule.Models.Message e)
        {
            if (e.MessageType == MessageType.Headline)
            {
                //Parse message and raise event
                var handler = HeadlineReceive;
                if (handler != null)
                {
                    handler.Invoke(this, e);
                }
            }
        }

        //private string BuildJid(int userId)
        //{
        //    var jid = string.Format("{0}-{1}@{2}", userId, appId, chatEndpoint);
        //    return jid;
        //}

        private string BuildSmallJid(string userId)
        {
            var jid = string.Format("{0}-{1}", userId, appId);
            return jid;
        }
    }
}
