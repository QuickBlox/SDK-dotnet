using System;

namespace Quickblox.Sdk.Core
{
    public interface ISerializer
    {
        String ContentType { get; }

        String Serialize<T>(T storedObj);

        T Deserialize<T>(String content);
    }
}
