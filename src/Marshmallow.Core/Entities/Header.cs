namespace Marshmallow.Core.Entities;

public class Header : EntityBase<Guid>
{ 
    public string Key { get; set; }
    public string Value { get; set; }
}

