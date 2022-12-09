using System.ComponentModel.DataAnnotations;

using Play.Domain;

namespace Play.Persistence.Gremlin;

public record Vertex : GraphDbObject
{
    #region Instance Values

    [Required]
    [StringLength(1)]
    public string Label { get; set; } = string.Empty;

    [Required]
    public dynamic Id { get; set; } = default!;

    [Required]
    public IEnumerable<Property> Properties { get; set; } = Array.Empty<Property>();

    public PersistenceObjects[] NestedObjects { get; set; } = Array.Empty<PersistenceObjects>();

    #endregion

    #region Instance Members

    public static Vertex Create(IDto dto)
    {
        Dictionary<string, dynamic> nestedProperties = dto.GetProperties().Where(a => a.Value.GetType() == typeof(IDto)).ToDictionary(a => a.Key, b => b.Value);
        Dictionary<string, dynamic> flatProperties = dto.GetProperties().Where(a => a.Value.GetType() != typeof(IDto)).ToDictionary(a => a.Key, b => b.Value);

        var properties = Property.CreateProperties(flatProperties).ToList();
        var vertex = new Vertex()
        {
            Id = dto.GetId(),
            Label = GetLabel(dto),
            Properties = properties
        };

        List<PersistenceObjects> nestedObjects = new();

        foreach (var nestedProperty in nestedProperties)
            nestedObjects.Add(PersistenceObjects.Create(dto, (IDto) nestedProperty.Value));

        return new Vertex()
        {
            Id = dto.GetId(),
            Label = GetLabel(dto),
            Properties = properties,
            NestedObjects = nestedObjects.ToArray()
        };
    }

    #endregion
}