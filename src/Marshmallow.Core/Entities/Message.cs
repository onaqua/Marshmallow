using ErrorOr;
using Marshmallow.Extensions.Extensions;

namespace Marshmallow.Core.Entities;

public class Message : EntityBase<Guid>
{
    private Message() { }
    private Message(
        Topic topic,
        Payload payload,
        IReadOnlyCollection<Header> headers)
    {
        Topic = topic;
        Payload = payload;
        Headers = headers;
    }

    public Guid TopicId { get; private set; }
    public Topic Topic { get; private set; }
    public Payload Payload { get; private set; }

    public IReadOnlyCollection<Header> Headers { get; private set; }

    public DateTime CreatedDateTime { get; private set; } = DateTime.UtcNow;

    public int Offset { get; }

    public static ErrorOr<Message> Create(
        Topic topic,
        Payload payload,
        IReadOnlyCollection<Header> headers)
    {
        List<Error> errors = [];

        if (topic.IsNull())
        {
            errors.Add(Error.NotFound(
                code: "Message.Create",
                description: "Topic not found"));
        }

        if (payload.IsNull())
        {
            errors.Add(Error.Validation(
                code: "Message.Create",
                description: "Payload cannot be null or empty"));
        }

        if (errors.IsNotEmpty())
        {
            return errors;
        }

        return new Message(topic, payload, headers);
    }
}
