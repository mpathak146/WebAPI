using System.Xml.Serialization;
using System.IO;
using Fourth.DataLoads.Data.Interface;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

namespace Fourth.DataLoads.Data.Models
{
    public static class ModelActions
    {
        public static  string ToXml(this IModelMarker entity)
        {
            XmlSerializer xsSubmit = new XmlSerializer(entity.GetType());
            var xml = "";
            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, entity);
                    xml = sww.ToString();

                }
            }
            return xml;
            //XmlSerializer xmlSerializer = new XmlSerializer(entity.GetType());
            //BinaryFormatter Formatter = new BinaryFormatter();
            //using (var stream = new MemoryStream())
            //{
            //    Formatter.Serialize(stream, entity);
            //    return System.Text.Encoding.Default.GetString(stream.ToArray());
            //}
        }
    }
}
