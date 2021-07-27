using Codesanook.Examples.DotNet.XML;
using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using Xunit;

namespace Codesanook.Examples.DotNet.XML
{

    //Use DataContractSerizalizer
    //https://theburningmonk.com/2010/05/net-tips-xml-serialize-or-deserialize-dictionary-in-csharp/

    [DataContract]
    public class CustomException : Exception
    {
        public CustomException() { }
        public CustomException(string message, Exception innerException) : base(message, innerException) { }

        public CustomException(string message) : base(message)
        {
        }

        [DataMember]
        public override IDictionary Data => base.Data;
    }

    public class XmlSerialization
    {
        [Fact]
        public void Serialize()
        {
            var xsSubmit = new XmlSerializer(typeof(CustomException));
            var subReq = new CustomException();

            using (var sww = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, subReq);
                    Console.WriteLine(sww.ToString()); // Your XML
                }
            }
        }

        [Fact]
        public void DeserializeTest()
        {
            using var streamReader = new StreamReader("Xml/book-with-namespace.xml");
            //using var streamReader = new StreamReader("Xml/book-without-namespace.xml");
            var namespaceManager = new XmlNamespaceManager(new NameTable());
            const string ns = "https://www.codesanook.com";
            namespaceManager.AddNamespace("cs", ns);
            namespaceManager.AddNamespace("c", "https://www.codesanook.com/common");
            namespaceManager.AddNamespace("", "https://www.codesanook.com/default");
            var context = new XmlParserContext(null, namespaceManager, null, XmlSpace.None);
            var settings = new XmlReaderSettings
            {
                ConformanceLevel = ConformanceLevel.Fragment,
                IgnoreComments = true,
            };

            using var xmlReader = XmlReader.Create(streamReader, settings, context);
            //using var xmlReader = new XmlTextReader(streamReader);
            //xmlReader.Namespaces = false;
            var xmlRootAttribute = new XmlRootAttribute()
            {
                ElementName = "Book",
                Namespace = ns
            };
            var serializer = new XmlSerializer(typeof(Book));
            var book = (Book)serializer.Deserialize(xmlReader);
            Assert.NotNull(book);
            Assert.Equal("web", book.Category);
        }
    }
}
