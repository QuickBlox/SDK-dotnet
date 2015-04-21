using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.NotificationModule.Models
{
    public class MessageConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is IMessage))
            {
                writer.WriteNull();
                return;
            }

            IMessage message = (IMessage) value;
            string messageString = message.BuildMessage();
            writer.WriteValue(messageString);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(IMessage))
            {
                var pushMessage = new PushMessage();
                pushMessage.DeserializeMessage(reader.Value.ToString());
                return pushMessage;
            }

            throw new JsonSerializationException(String.Format("Unexpected token { 0 } when parsing enum.",
                reader.TokenType, CultureInfo.InvariantCulture));
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IMessage)))
            {
                return true;
            }
            return false;
        }
    }
}
