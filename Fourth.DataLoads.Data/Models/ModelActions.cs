using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Text;
using Fourth.DataLoads.Data.Interfaces;

namespace Fourth.DataLoads.Data.Models
{
    public static class ModelActions
    {
        /// <summary>
        /// Serializer for generic inputs marked as serialized
        /// </summary>
        /// <typeparam name="T">Type type to serialize that is marked as so.</typeparam>
        /// <param name="input">The class instance to serialize.</param>
        /// <returns>The serialized XML as a string</returns>
        public static string ToXml<T>(this IMarker input)
        {
            string xml = string.Empty;
            var serializer = new XmlSerializer(typeof(T));
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Encoding = new UnicodeEncoding(false, false), 
                Indent = false,
                OmitXmlDeclaration = false
            };

            using (StringWriter textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, input);
                }
                xml = textWriter.ToString();
            }

            return xml;
        }
    }
}
