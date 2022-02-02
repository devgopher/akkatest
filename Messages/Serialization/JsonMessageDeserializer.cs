using System.Text;
using System.Text.Unicode;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace Messages.Serialization
{
    public class JsonMessageDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull)
                return default;

            var strg = Encoding.UTF8.GetString(data);

            return JsonConvert.DeserializeObject<T>(strg);
        }
    }
}
