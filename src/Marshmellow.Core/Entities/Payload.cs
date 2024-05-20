using ErrorOr;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Marshmallow.Core.Entities;

public class Payload : ValueObject, IEquatable<Payload>
{
    private Payload() { }
    private Payload(byte[] value)
    {
        Value = value;
    }

    public byte[] Value { get; set; } = null!;

    public static async Task<ErrorOr<Payload>> Create<T>(T value)
    {
        return await Task.Run(() =>
        {
            using (MemoryStream ms = new MemoryStream())
            using (BsonDataWriter datawriter = new BsonDataWriter(ms))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(datawriter, value);

                return new Payload(ms.ToArray());
            }
        });
    }

    public bool Equals(Payload? other)
    {
        if (other is null)
        {
            return false;
        }

        return other.Value.Equals(this.Value);
    }
}

