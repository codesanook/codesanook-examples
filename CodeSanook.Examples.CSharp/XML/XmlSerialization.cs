using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using Xunit;

namespace Codesanook.Examples.CSharp.XML
{

//Use DataContractSerizalizer
//https://theburningmonk.com/2010/05/net-tips-xml-serialize-or-deserialize-dictionary-in-csharp/

    [DataContract]
    public class CustomException : Exception
    {
        public CustomException() { }
        public CustomException(string message, Exception innerException) : base(message, innerException) { }

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
    }
}
