using System;

namespace Quickblox.Sdk.Serializer
{
    public interface IFactorySerializer
    {
        ISerializer CreateSerializer(String contentType);

        ISerializer CreateSerializer();
    }
}
