using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FluentAssertions;
using Nuke.Common.IO;
using Nuke.Common.Tooling;

namespace NMica.Tests.Utils
{
    public static class Extensions
    {
        public static string ToXml(this object obj)
        {
		
            var stringWriter = new StringWriter();
            var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { OmitXmlDeclaration = true, Indent=true });
            var serializer = new XmlSerializer(obj.GetType());
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            serializer.Serialize(xmlWriter, obj, ns);
            return stringWriter.ToString();
        }

        public static string Flatten(this IReadOnlyCollection<Output> outputs)
        {
            return outputs.Aggregate(new StringBuilder(), (sb, x) => sb.AppendLine(x.Text)).ToString();
            
        }
        public static FileAssertion Should(this FileToTest file) => new FileAssertion {File = file};
        public class FileAssertion
        {
            public FileToTest File;
            public void NotExist(string because = "",params object[] reasonArgs)
                => System.IO.File.Exists(File.Path).Should().BeFalse(because,reasonArgs);
            public void Exist(string because = "", params object[] reasonArgs)
                => System.IO.File.Exists(File.Path).Should().BeTrue(because, reasonArgs);
        }
        public static FileToTest FileName(string fileName) => new FileToTest { Path = fileName };
        public class FileToTest {public string Path;}
    }
}