using System.Text;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace Messages
{
    public class JSerializer<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context) => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
    }

    public class JDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            var text = Encoding.UTF8.GetString(data);

            return JsonConvert.DeserializeObject<T>(text);
        }
    }
}
