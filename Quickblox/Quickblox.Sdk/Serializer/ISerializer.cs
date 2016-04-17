using System;

namespace Quickblox.Sdk.Serializer
{
    internal interface ISerializer
    {
        String ContentType { get; }

        String Serialize<T>(T storedObj);

        T Deserialize<T>(String content);
    }
}
