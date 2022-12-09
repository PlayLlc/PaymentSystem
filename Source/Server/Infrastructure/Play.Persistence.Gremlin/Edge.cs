using System.ComponentModel.DataAnnotations;

using Play.Domain.Events;

namespace Play.Persistence.Gremlin;

public record Edge : GraphDbObject
{
    #region Instance Values

    [Required]
    [StringLength(1)]
    public string Label { get; set; } = string.Empty;

    public Vertex In { get; set; } = null!;
    public Vertex Out { get; set; } = null!;
    public IEnumerable<Property> Properties { get; set; } = Array.Empty<Property>();

    #endregion

    #region Instance Members

    public static Edge Create(Vertex source, Vertex target, DomainEvent domainEvent)
    {
        IEnumerable<Property> properties = Property.CreateProperties(domainEvent.GetType()
            .GetProperties()
            .Where(a => (a.Name != nameof(DomainEvent.Description))
                        && (a.Name != nameof(DomainEvent.DomainEventType))
                        && (a.Name != nameof(DomainEventIdentifier))));

        return new Edge()
        {
            Label = GetLabel(domainEvent),
            In = target,
            Out = source,
            Properties = properties
        };
    }

    #endregion
}