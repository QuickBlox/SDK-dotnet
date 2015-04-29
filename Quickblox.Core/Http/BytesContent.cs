using System;

namespace Quickblox.Sdk.Http
{
    public class BytesContent
    {
        public String ContentType { get; set; }

        public String FileName { get; set; }

        public String Name { get; set; }

        public Byte[] Bytes { get; set; }
    }
}
