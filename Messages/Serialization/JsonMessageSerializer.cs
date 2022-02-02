using System.Text;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace Messages.Serialization;

public class JsonMessageSerializer<T> : ISerializer<T>
{
    public byte[] Serialize(T data, SerializationContext context)
    {
        var json = JsonConvert.SerializeObject(data);

        return Encoding.UTF8.GetBytes(json);
    }
}