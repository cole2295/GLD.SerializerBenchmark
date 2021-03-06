///
/// https://github.com/ServiceStack/ServiceStack.Text
/// PM> Install-Package ServiceStack.Text
/// TODO: DateTime fields is still under work.

using System.IO;
using ServiceStack.Text;

namespace GLD.SerializerBenchmark
{
    internal class ServiceStackTypeSerializer : ISerDeser
    {

        #region ISerDeser Members

        public string Serialize<T>(object person)
        {
            using (var sw = new StringWriter())
            {
                TypeSerializer.SerializeToWriter<T>((T)person, sw);
                return sw.ToString();
            }
        }

        public T Deserialize<T>(string serialized)
        {
            using (var sr = new StringReader(serialized))
            {
                return TypeSerializer.DeserializeFromReader<T>(sr);
            }
        }

        #endregion
    }
}