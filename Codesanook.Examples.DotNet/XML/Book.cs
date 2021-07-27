using System.Xml.Serialization;

namespace Codesanook.Examples.DotNet.XML
{
    public class Book
    {
        [XmlAttribute]
        public string Category { get; set; }

        [XmlElement(Namespace = "https://www.codesanook.com/common")]
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
    }
}
