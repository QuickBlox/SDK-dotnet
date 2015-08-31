using System;

namespace Quickblox.Sdk.Serializer
{
    internal interface IFactorySerializer
    {
        ISerializer CreateSerializer(String contentType);

        ISerializer CreateSerializer();
    }
}
