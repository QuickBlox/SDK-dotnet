using System;
using Newtonsoft.Json;

namespace Quickblox.Sdk.Modules.NotificationModule.Models
{
    public interface IMessage
    {
        string BuildMessage();

        void DeserializeMessage(String compressedString);
    }
}
