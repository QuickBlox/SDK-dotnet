using System;

namespace Quickblox.Sdk.Modules.NotificationModule.Models
{
    public class SimplePushMessage : IMessage
    {
        private string formatText;

        public SimplePushMessage(String text)
        {
            this.formatText = text;
        }

        public string BuildMessage()
        {
            return formatText;
        }

        public void DeserializeMessage(string compressedString)
        {
            this.formatText = compressedString;
        }
    }
}
