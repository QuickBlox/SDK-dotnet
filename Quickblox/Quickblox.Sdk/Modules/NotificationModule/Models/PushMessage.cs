using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.NotificationModule.Models
{
    public class PushMessage : IMessage
    {
        private string formatMessage;

        private const string ToastBaseMessage1 = "<?xml version='1.0' encoding='utf-8'?>" +
                                                "<wp:Notification xmlns:wp='WPNotification'>" +
                                                "<wp:Toast>" +
                                                "<wp:Text1>{0}</wp:Text1>" +
                                                "<wp:Text2></wp:Text2>" +
                                                "<wp:Param></wp:Param>" +
                                                "</wp:Toast>" +
                                                "</wp:Notification>";

        private const string ToastBaseMessage2 = "<?xml version='1.0' encoding='utf-8'?>" +
                                               "<wp:Notification xmlns:wp='WPNotification'>" +
                                               "<wp:Toast>" +
                                               "<wp:Text1>{0}</wp:Text1>" +
                                               "<wp:Text2>{1}</wp:Text2>" +
                                               "<wp:Param></wp:Param>" +
                                               "</wp:Toast>" +
                                               "</wp:Notification>";

        private const string ToastBaseMessage3 = "<?xml version='1.0' encoding='utf-8'?>" +
                                               "<wp:Notification xmlns:wp='WPNotification'>" +
                                               "<wp:Toast>" +
                                               "<wp:Text1>{0}</wp:Text1>" +
                                               "<wp:Text2>{1}</wp:Text2>" +
                                               "<wp:Param>{2}</wp:Param>" +
                                               "</wp:Toast>" +
                                               "</wp:Notification>";

        private const string MessageKey = "mpns=";
        private const string HeaderKey = "headers=";
        private const string HeaderValue = "Content-Type,text/xml,Content-Length,226,X-NotificationClass,2,X-WindowsPhone-Target,toast";

        internal PushMessage()
        {
        }

        public PushMessage(String title)
        {
            this.formatMessage = String.Format(ToastBaseMessage1, title);
            this.Header = HeaderValue;
            this.Title = title;
        }

        public PushMessage(String title, String text)
        {
            this.formatMessage = String.Format(ToastBaseMessage2, title, text);
            this.Header = HeaderValue;
            this.Title = title;
            this.Text = text;
        }

        public PushMessage(String title, String text, String parameter)
        {
            this.formatMessage = String.Format(ToastBaseMessage3, title, text, parameter);
            this.Header = HeaderValue;
            this.Title = title;
            this.Text = text;
            this.Parameter = parameter;
        }

        [JsonIgnore]
        public String Title { get; private set; }

        [JsonIgnore]
        public String Text { get; private set; }

        [JsonIgnore]
        public String Parameter { get; private set; }

        [JsonIgnore]
        public String Header { get; private set; }

        private String BuildHeader()
        {
            return HeaderKey + Convert.ToBase64String(Encoding.UTF8.GetBytes(HeaderValue));
        }

        private String BuildText(string formattedText)
        {
            return MessageKey + Convert.ToBase64String(Encoding.UTF8.GetBytes(formattedText));
        }

        public string BuildMessage()
        {
            var sb = new StringBuilder();
            sb.Append(this.BuildText(this.formatMessage));
            sb.Append("&");
            sb.Append(this.BuildHeader());
            return sb.ToString();
        }

        public void DeserializeMessage(string compressedString)
        {
            var splited = compressedString.Split('&');
            foreach (var part in splited)
            {
                if (part.StartsWith(MessageKey))
                {
                    var bytes = Convert.FromBase64String(part.Remove(0, MessageKey.Length));
                    var message = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                    var document = XDocument.Parse(message);
                    this.Title = document.Descendants("{WPNotification}Text1").First().Value;
                    this.Text = document.Descendants("{WPNotification}Text2").First().Value;
                    this.Parameter = document.Descendants("{WPNotification}Param").First().Value;
                }

                if (part.StartsWith(HeaderKey))
                {
                    var bytes = Convert.FromBase64String(part.Remove(0, HeaderKey.Length));
                    this.Header = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                }
            }
        }
    }
}
