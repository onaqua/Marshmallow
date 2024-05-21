using ErrorOr;
using Marshmallow.Extensions.Extensions;

namespace Marshmallow.Core.Entities;

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