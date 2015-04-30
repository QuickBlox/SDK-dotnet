using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Quickblox.Sdk.Core;

namespace Quickblox.Sdk.Serializer
{
    public class XmlSerializer : ISerializer
    {
        private const string XmlTagPattern = @"<\?xml.*\?>";

        public string ContentType
        {
            get { return "application/xml"; }
        }

        public string Serialize<T>(T storedObj)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var memoryStream = new MemoryStream())
            {
                XmlWriterSettings writerSettings = new XmlWriterSettings {OmitXmlDeclaration = true};
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");
                XmlWriter xmlWriter =  XmlWriter.Create(memoryStream, writerSettings);
                serializer.Serialize(xmlWriter, storedObj, namespaces);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return new StreamReader(memoryStream).ReadToEnd();
            }
        }

        public T Deserialize<T>(string content)
        {
            var regex = new Regex(XmlTagPattern);
            var removeXmlTag = regex.Replace(content, String.Empty);
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(removeXmlTag)))
            {
                return (T)serializer.Deserialize(memoryStream);
            }
        }
    }
}
