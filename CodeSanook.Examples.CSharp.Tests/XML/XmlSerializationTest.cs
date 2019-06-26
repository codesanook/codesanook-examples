using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Xunit;

namespace Codesanook.Examples.CSharp.Tests.XML
{
    [Serializable]
    public class MyClass : Exception, ISerializable
    {
        public MyClass() { }
        public MyClass(string message, Exception innerException) : base(message, innerException) { }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("message2", Message);
        }
    }

    public class XmlSerializationTest
    {

        [Fact]
        public void Test()
        {
            var xsSubmit = new XmlSerializer(typeof(MyClass));
            var subReq = new MyClass();

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
