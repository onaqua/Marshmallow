using ErrorOr;

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