using ErrorOr;
using Marshmallow.Extensions.Extensions;

namespace Marshmallow.Core.Entities;

public class Topic : EntityBase<Guid>
{
    private HashSet<Message>? _messages;
    private HashSet<Group>? _groups;

    private Topic() { }

    private Topic(string name)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public IReadOnlyCollection<Message>? Messages => _messages;

    public IReadOnlyCollection<Group> Groups => _groups;

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