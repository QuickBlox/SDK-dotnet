// Parser.cs
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
using XMPP.states;
using XMPP.tags;

namespace XMPP.common
{
	public class Parser
	{
        private readonly Manager _manager;

        private string _dataQueue = string.Empty;
        private bool _streamStarted = false;

		public Parser(Manager manager)
		{
            _manager = manager;
		}

        public void Parse(string message)
        {
            if (_manager.State is ClosedState || _manager.State is DisconnectState)
                return;

            Enqueue(message);

            string fragment = string.Empty;
            do
            {
                fragment = Dequeue();

                if (string.IsNullOrEmpty(fragment))
                    continue;

                if (fragment == "</stream:stream>") // Disconnect
                {
#if DEBUG
                    _manager.Events.LogMessage(this, LogType.Info, "End of stream received from server");
#endif
                    return;
                }

                Tag newElement = Tag.Get(fragment);

                if (newElement == null)
                    newElement = Tag.Get(fragment);

                if (newElement != null)
                {
                    newElement.Timestamp = DateTime.Now;
                    newElement.Account = _manager.Settings.Account;
                    _manager.Events.NewTag(this, newElement);
                }
                else
                {
                    if( _manager.State.GetType() == typeof(states.RunningState) )
                        _manager.Events.Error(this, ErrorType.InvalidXMLFragment, ErrorPolicyType.Informative, "Parsing a fragment failed");
                    else
                        _manager.Events.Error(this, ErrorType.InvalidXMLFragment, ErrorPolicyType.Reconnect, "Parsing a fragment failed in a critical situation");
                }
            }
            while (!string.IsNullOrEmpty(fragment));
        }

        public void Clear()
        {
            _streamStarted = false;
            _dataQueue = string.Empty;
        }

        public void Enqueue(string data)
        {
            if (_manager.Transport == Transport.Socket)
            {
                // Stream gets opened
                if (data.Contains("<stream:stream"))
                {
                    _dataQueue = string.Empty;
                    data = data.Substring(data.IndexOf("<stream:stream"));
                    int posStreamTagEnd = FirstOfUnEscaped(data, '>');
                    data = data.Insert(posStreamTagEnd + 1, "</stream:stream>");
                    _streamStarted = true;
                }

                if (_dataQueue.Length == 0 && !data.StartsWith("<"))
                {
                    _manager.Events.Error(this, ErrorType.InvalidXMLFragment, ErrorPolicyType.Reconnect, "Parsing a fragment failed in a critical situation");
                    return;
                }

                // Add opened stream
                if (_streamStarted)
                    _dataQueue += data;
            }
            else
            {
                _dataQueue += data;
            }
        }

        public string Dequeue()
        {
            // End
            if (_dataQueue.StartsWith("</stream:stream>"))
            {
                Clear();
                return "</stream:stream>";
            }

            return GetNextFragment();
        }

        private string GetNextFragment()
        {
            if (string.IsNullOrEmpty(_dataQueue))
                return _dataQueue;

            bool validElement = false;
            int posElementEnd = 0;

            int posOpen = FirstOfUnEscaped(_dataQueue, '<');
            int posClose = FirstOfUnEscaped(_dataQueue, '>');
            int posFirstSpace = FirstOfUnEscaped(_dataQueue, ' ');

            if (posOpen == -1 || posClose == -1)
                return string.Empty;

            // Empty element
            if ((_dataQueue.Substring(posOpen, 2) == "<?" && _dataQueue.Substring(posClose - 1, 2) == "?>"))
            {
                posElementEnd += posClose + 1;
                validElement = false;
            }
            // Self closing tag
            else if (_dataQueue[posClose - 1] == '/')
            {
                posElementEnd += posClose + 1;
                validElement = true;
            }
            // Close tag
            else if (_dataQueue[posOpen + 1] == '/')
            {
                posElementEnd += posClose + 1;
                validElement = false;
            }
            // Open Tag
            else
            {
                // Get Tag name
                var nameLength = -1;

                if (posClose == -1 || posFirstSpace < posClose)
                    nameLength = posFirstSpace;
                else if (posFirstSpace == -1 || posClose < posFirstSpace)
                    nameLength = posClose;

                // Can't close tag , try again later
                if (nameLength == -1)
                    return string.Empty;

                // Got Tag name
                string elementName = _dataQueue.Substring(posOpen + 1, nameLength - 1);

                // Search for the end tag in the rest of the stream 
                int endTagEndPosition = FirstEndTag(_dataQueue.Substring(posClose + 1), elementName);
                int absoluteEndTagEndPosition = endTagEndPosition + posClose + 1; // endTagEndPosition is relative from end of start element
                // We found the end tag
                if (endTagEndPosition != -1 && absoluteEndTagEndPosition <= _dataQueue.Length)
                {
                    posElementEnd = absoluteEndTagEndPosition;
                    validElement = true;
                }
                else
                {
                    return string.Empty;
                }
            }

            if (posElementEnd == 0 || posElementEnd > _dataQueue.Length)
                return string.Empty;

            string fragment = _dataQueue.Substring(0, posElementEnd);
            _dataQueue = _dataQueue.Substring(posElementEnd);

            if (validElement)
                return fragment;
            else
                return string.Empty;
        }

        private int FirstOfUnEscaped(string data, char token)
        {
            var pos = -1;
            while (pos == -1 && !string.IsNullOrEmpty(data))
            {
                var newIndex = data.IndexOf(token);

                if (newIndex == -1)
                    return -1;

                if (newIndex == 0 || data[newIndex - 1] != '\\')
                    pos = newIndex;

                data = data.Substring(newIndex + 1);
            }

            return pos;
        }

        private int FirstEndTag(string data, string name)
        {
            string workingCopy = data;
            var dataPosition = 0;
            int depth = 1;
            var pos = -1;
            while (depth > 0 && !string.IsNullOrEmpty(workingCopy))
            {
                var newIndex = workingCopy.IndexOf(name);

                // Token not found -> -1
                if (newIndex == -1)
                    return -1;

                // Check if this is data
                if (newIndex - 2 > 0 && workingCopy[newIndex - 2] == '\\') { }
                else if (newIndex - 3 > 0 && workingCopy[newIndex - 3] == '\\') { }

                // Is a start tag 
                else if (workingCopy[newIndex - 1] == '<')
                    depth++;

                // Is a close tag
                else if (workingCopy[newIndex - 2] == '<' && workingCopy[newIndex - 1] == '/')
                    depth--;

                dataPosition += newIndex + name.Length;
                workingCopy = data.Substring(dataPosition);

                // No more open elements
                if (depth == 0)
                    pos = dataPosition + 1;
            }

            return pos;
        }
	}
}                                                                                                                                                                                                                                                                                    