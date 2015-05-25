// Events.cs
//
//Copyright © 2006 - 2012 Dieter Lunn
//Modified 2012 Paul Freund ( freund.paul@lvl3.org )
//
//This library is free software; you can redistribute it and/or modify it under
//the terms of the GNU Lesser General Public License as published by the Free
//Software Foundation; either version 3 of the License, or (at your option)
//any later version.
//
//This library is distributed in the hope that it will be useful, but WITHOUT
//ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
//FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more
//
//You should have received a copy of the GNU Lesser General Public License along
//with this library; if not, write to the Free Software Foundation, Inc., 59
//Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System;
using XMPP.tags;

namespace XMPP
{
    public enum LogType
    {
        Warn,
        Debug,
        Info,
        Error,
        Fatal
    }

    public enum ErrorPolicyType
    {
        Informative,        // Just tell about the error
        Deactivate,         // Deactivate account
        Reconnect,          // Automatically handeled and turned to Deactivate if neccesary
        Severe             // The application can not run like this
    }

    public enum ErrorType
    {
        None

        ,// Default
        Undefined

        ,// Internal
        AuthenticationFailed,                      // Deactivate
        XMPPVersionNotSupported,                   // Deactivate
        NoSupportedAuthenticationMethod,           // Deactivate
        InvalidXMLFragment,                        // Informative
        BindingToResourceFailed,                   // Deactivate
        MD5AuthError,                              // Deactivate
        OAUTH2AuthError,                           // Deactivate
        SCRAMAuthError,                            // Deactivate
        PLAINAuthError,                            // Deactivate
        InvalidHostName,                           // Deactivate
        WriteStateMismatch,                        // Reconnect
        ConnectToServerFailed,                     // Reconnect
        ServerDisconnected,                        // Reconnect
        SocketReadInterrupted,                     // Reconnect
        SocketWriteInterrupted,                    // Reconnect
        InvalidSSLCertificate,                     // Deactivate
        SocketChangeFailed,                        // Reconnect
        MissingPassword,                           // Deactivate
        MissingHost,                               // Deactivate
        MissingJID,                                // Deactivate
        ServerError                                // Reconnect

        ,// External
        RegisterControlChannel,                    // Reconnect
        UnregisterControlChannel,                  // Reconnect
        WaitForPushEnabled,                        // Reconnect
        BackgroundTaskCreate,                      // Reconnect
        InvalidSettings,                           // Deactivate
        InvalidJID,                                // Deactivate
        InvalidHostname,                           // Deactivate
        InvalidConnectionId,                       // Deactivate
        NotConnected,                              // Informative
        NoInternet,                                // Informative

        NoHardwareSlotsAllowed,                    // Deactivate

        RegisterSystemEvents,                      // Severe
        UnregisterSystemEvents,                    // Severe
        RequestBackgroundAccess                    // Severe

        //,// Server
        //Stanza_bad_request,
        //Stanza_conflict,
        //Stanza_feature_not_implemented,
        //Stanza_forbidden,
        //Stanza_gone,
        //Stanza_internal_server_error,
        //Stanza_item_not_found,
        //Stanza_jid_malformed,
        //Stanza_not_acceptable,
        //Stanza_not_authorized,
        //Stanza_not_allowed,                                   
        //Stanza_payment_required,
        //Stanza_policy_violation,
        //Stanza_recipient_unavailable,                         
        //Stanza_redirect,
        //Stanza_registration_required,
        //Stanza_remote_server_not_found,
        //Stanza_remote_server_timeout,
        //Stanza_resource_constraint,
        //Stanza_service_unavailable,
        //Stanza_subscription_required,
        //Stanza_undefined_condition,
        //Stanza_unexpected_request

        //,// Stream
        //Stream_bad_format,
        //Stream_bad_namespace_prefix,
        //Stream_conflict,
        //Stream_connection_timeout,
        //Stream_host_gone,
        //Stream_host_unknown,
        //Stream_improper_addressing,
        //Stream_internal_server_error,
        //Stream_invalid_from,
        //Stream_invalid_id,
        //Stream_invalid_namespace,
        //Stream_invalid_xml,                                   
        //Stream_not_authorized,
        //Stream_policy_violation,
        //Stream_remote_connection_failed,
        //Stream_reset,
        //Stream_resource_constraint,
        //Stream_restricted_xml,
        //Stream_see_other_host,
        //Stream_system_shutdown,
        //Stream_undefined_condition,
        //Stream_unsupported_encoding,
        //Stream_unsupported_stanza_type,
        //Stream_unsupported_version,
        //Stream_xml_not_well_formed           
    }

    public enum ChunkDirection
    {
        Incomming,
        Outgoing
    }
}

namespace XMPP.common
{
    public class TagEventArgs : EventArgs
    {
        public TagEventArgs(Tag tag_) { this.tag = tag_; }
        public Tag tag;
    }

    public class LogEventArgs : EventArgs
    {
        public LogEventArgs(string message_, LogType type_) { this.message = message_; this.type = type_; }
        public string message;
        public LogType type;
    }

    public class ChunkLogEventArgs : LogEventArgs
    {
        public ChunkLogEventArgs(string message, ChunkDirection direction) : base(message, LogType.Info)
        {
            Direction = direction;
        }

        public ChunkDirection Direction;
    }

    public class ErrorEventArgs : EventArgs
    {
        public ErrorEventArgs(string message_, ErrorType type_, ErrorPolicyType policy_) { this.message = message_; this.type = type_; this.policy = policy_; }
        public ErrorType type;
        public ErrorPolicyType policy;
        public string message;
    }

    public class ResourceBoundEventArgs : EventArgs
    {
        public ResourceBoundEventArgs(string jid_) { this.jid = jid_; }
        public string jid;
    }
    
	public class Events
    {
        #region internal

            #region Connect
                public event InternalConnect OnConnect;
                public delegate void InternalConnect(object sender, EventArgs e);
                public void Connect(object sender, EventArgs e = default(EventArgs)) { if (OnConnect != null)OnConnect(sender, e); }
            #endregion

            #region Disconnect
                public event InternalDisconnect OnDisconnect;
                public delegate void InternalDisconnect(object sender, EventArgs e);
                public void Disconnect(object sender, EventArgs e = default(EventArgs)) { if (OnDisconnect != null)OnDisconnect(sender, e); }
            #endregion

            #region Send
                public event InternalSend OnSend;
                public delegate void InternalSend(object sender, TagEventArgs e);
                public void Send(object sender, Tag tag) { Send(sender, new TagEventArgs(tag)); }
                public void Send(object sender, TagEventArgs e) { if (OnSend != null)OnSend(sender, e); }
            #endregion

        #endregion

        #region external

            #region NewTag
                public event ExternalNewTag OnNewTag;
                public delegate void ExternalNewTag(object sender, TagEventArgs e);
                public void NewTag(object sender, Tag tag) { NewTag(sender, new TagEventArgs(tag)); }
                public void NewTag(object sender, TagEventArgs e) { if (OnNewTag != null)OnNewTag(sender, e); }
            #endregion

            #region Receive
                public event ExternalReceive OnReceive;
                public delegate void ExternalReceive(object sender, TagEventArgs e);
                public void Receive(object sender, Tag tag) { Receive(sender, new TagEventArgs(tag)); }
                public void Receive(object sender, TagEventArgs e) { if (OnReceive != null) OnReceive(sender, e); }
            #endregion

            #region Error
                public event ExternalError OnError;
                public delegate void ExternalError(object sender, ErrorEventArgs e);
                public void Error(object sender, ErrorType type, ErrorPolicyType policy, string format, params object[] parameters) { Error(sender, type, policy, string.Format(format, parameters)); }
                public void Error(object sender, ErrorType type, ErrorPolicyType policy, string message = "") { Error(sender, new ErrorEventArgs(message, type, policy)); }
                public void Error(object sender, ErrorEventArgs e) { if (OnError != null)OnError(sender, e); }
            #endregion

            #region LogMessage
                public event ExternalLogMessage OnLogMessage;
                public delegate void ExternalLogMessage(object sender, LogEventArgs e);
                public void LogMessage(object sender, LogType type, string format, params object[] parameters) { LogMessage(sender, type, string.Format(format, parameters)); }
                public void LogMessage(object sender, LogType type, string message) { LogMessage(sender, new LogEventArgs(message, type)); }
                public void LogMessage(object sender, LogEventArgs e) { if (OnLogMessage != null)OnLogMessage(sender, e); }
            #endregion

            #region Trace stream
                public event ExternalChunk OnChunk;
                public delegate void ExternalChunk(object sender, ChunkLogEventArgs e);
                public void Chunk(object sender, ChunkLogEventArgs e = default(ChunkLogEventArgs)) { if (OnChunk != null) OnChunk(sender, e); }
            #endregion

            #region Ready
                public event ExternalReady OnReady;
                public delegate void ExternalReady(object sender, EventArgs e);
                public void Ready(object sender, EventArgs e = default(EventArgs)) { if (OnReady != null) OnReady(sender, e); }
            #endregion

            #region ResourceBound
                public event ExternalResourceBound OnResourceBound;
                public delegate void ExternalResourceBound(object sender, ResourceBoundEventArgs e);
                public void ResourceBound(object sender, string jid) { ResourceBound(this, new ResourceBoundEventArgs(jid)); }
                public void ResourceBound(object sender, ResourceBoundEventArgs e) { if (OnResourceBound != null) OnResourceBound(sender, e); }
            #endregion

            #region Connected
                public event ExternalConnected OnConnected;
                public delegate void ExternalConnected(object sender, EventArgs e);
                public void Connected(object sender, EventArgs e = default(EventArgs)) { if (OnConnected != null) OnConnected(sender, e); }
            #endregion

            #region Disconnected
                public event ExternalDisconnected OnDisconnected;
                public delegate void ExternalDisconnected(object sender, EventArgs e);
                public void Disconnected(object sender, EventArgs e = default(EventArgs)) { if (OnDisconnected != null) OnDisconnected(sender, e); }
            #endregion

        #endregion
    }
}



