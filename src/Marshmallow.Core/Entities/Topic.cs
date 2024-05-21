using ErrorOr;
using Marshmallow.Extensions.Extensions;

namespace Marshmallow.Core.Entities;

public class Producer : EntityBase<Guid>
{
    private Producer() { }
    private Producer(Topic topic, string name)
    {
        Name = name;
        Topic = topic;
    }

    public string Name { get; private set; }

    public Topic Topic { get; private set; }

    public Guid TopicId { get; private set; }

    public static ErrorOr<Producer> Create(Topic topic, string name)
    {
        if (name.IsNullOrEmpty())
        {
            return Error.Unexpected(
                code: "Producer.Create",
                description: "Name cannot be empty or null.");
        }

        if (topic.IsNull())
        {
            return Error.Unexpected(
                code: "Producer.Create",
                description: "Topic cannot be null.");
        }

        return new Producer(topic, name);
    }
}

public class Topic : EntityBase<Guid>
{
    private HashSet<Message>? _messages = new HashSet<Message>();
    private HashSet<Group>? _groups = new HashSet<Group>();
    private HashSet<Producer>? _producers = new HashSet<Producer>();

    private Topic() { }

    private Topic(string name)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public IReadOnlyCollection<Group>? Groups => _groups;

    public IReadOnlyCollection<Message>? Messages => _messages;

    public IReadOnlyCollection<Producer>? Producers => _producers;

    public void AddProducer(Producer producer)
    {
        if (_producers.IsNull())
        {
            throw new InvalidOperationException($"Need to _.Include(_ => {nameof(Producers)})");
        }

        if (producer.IsNull())
        {
            throw new InvalidOperationException($"Producer cannot be null.");
        }

        _producers.Add(producer);
    }

    public void RemoveProducer(Producer producer)
    {
        if (_producers.IsNull())
        {
            throw new InvalidOperationException($"Need to _.Include(_ => {nameof(Producers)})");
        }

        if (producer.IsNull())
        {
            throw new InvalidOperationException($"Producer cannot be null.");
        }

        _producers.RemoveWhere(x => x.Id.Equals(producer.Id));
    }

    public ErrorOr<Message> AddMessage(Message message)
    {
        List<Error> errors = [];

        if (Messages.IsNull())
        {
            throw new InvalidOperationException($"Need to _.Include(_ => {nameof(Messages)})");
        }

        if (message.IsNull())
        {
            errors.Add(Error.Validation(
                code: "Topic.AddMessage",
                description: "Message cannot be null"));
        }

        if (errors.IsNotEmpty())
        {
            return errors;
        }

        return message;
    }

    public static ErrorOr<Topic> Create(string name)
    {
        List<Error> errors = [];

        if (name.IsNullOrEmpty())
        {
            errors.Add(Error.Validation(
                code: "Topic.Create",
                description: "Name cannot be empty or whitespace only"));
        }

        if (errors.IsNotEmpty())
        {
            return errors;
        }

        return new Topic(name);
    }
}