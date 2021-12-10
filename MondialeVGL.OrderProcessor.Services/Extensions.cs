using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MondialeVGL.OrderProcessor.Services
{
    public static class Extensions
    {
        public static string Serialize<T>(this T value)
            where T:class
        {
            if (value == null)
            {
                return string.Empty;
            }

            try
            {
                //Create our own namespaces for the output
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                //Add an empty namespace and empty value
                ns.Add("", "");

                using (var ms = new MemoryStream())
                {
                    using (var xmlWriter = new XmlTextWriter(ms, null))
                    {
                        xmlWriter.Formatting = Formatting.Indented;
                        xmlWriter.Indentation = 1;
                        xmlWriter.IndentChar = '\t';

                        var xmlserializer = new XmlSerializer(typeof(T));
                        xmlserializer.Serialize(xmlWriter, value, ns);

                        StreamReader reader = new StreamReader(xmlWriter.BaseStream, Encoding.UTF8, true);
                        ms.Seek(0, SeekOrigin.Begin);
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred in xml serialization.", ex);
            }
        }
    }
}
