namespace WalmgateIdentity.Core.Entities;

public class Tenant : IEntity, ISoftDelete
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public string Name { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
