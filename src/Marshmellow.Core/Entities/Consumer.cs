using ErrorOr;
using Marshmallow.Extensions.Extensions;

namespace Marshmallow.Core.Entities;

public class Consumer : EntityBase<Guid>
{
    private Consumer() { }
    private Consumer(Group group)
    { 
        Group = group;
    }

    public Group? Group { get; private set; }
    public Guid GroupId { get; private set; }
    public DateTime CreatedDateTime { get; private set; } = DateTime.UtcNow;
    public string? Peer { get; private set; }

    public void SetPeer(string peer)
    {
        if (string.IsNullOrWhiteSpace(peer))
        {
            throw new ArgumentNullException(nameof(peer));
        }

        Peer = peer;
    }

    public static ErrorOr<Consumer> Create(
        Group group)
    {
        if (group is null)
        {
            return Error.Failure(
                code: "Consumer.Create",
                description: "Cannot create consumer, because group does not exists.");
        }

        return new Consumer(group);
    }
}

public class Group : EntityBase<Guid>
{
    private List<Consumer> _consumers;

    private Group() { }
    private Group(
        string name,
        Topic topic)
    {
        Name = name;
        Topic = topic;
    }

    public string Name { get; private set; }
    public Topic Topic { get; private set; }
    public Guid TopicId { get; private set; }
    public IReadOnlyCollection<Consumer> Consumers => _consumers;

    public static ErrorOr<Group> Create(
        string groupName,
        Topic topic)
    {
        List<Error> errors = [];

        if (groupName.IsNullOrEmpty())
        {
            errors.Add(Error.Validation(
                code: "Group.Create",
                description: $"{nameof(groupName)} is null or empty"));
        }

        if (errors.IsNotEmpty())
        {
            return errors;
        }

        return new Group(groupName, topic);
    }
}