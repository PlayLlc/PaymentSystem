using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Gremlin.Net.Process.Traversal;
using Gremlin.Net.Structure;

using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.Entities;
using Play.Domain.Events;

namespace Play.Persistence.Gremlin;

//public record GraphDbObject
//{
//    public static string GetLabel(dynamic obj) => obj.GetType().Name;

//    public static IEnumerable<Property> GetProperties()
//    {
//        throw new NotImplementedException();
//    }
//}

//public record PersistenceObjects
//{
//    public Vertex In { get; set; } = null!;
//    public Vertex Out { get; set; } = null!;
//    public Edge Edge { get; set; } = null!;

//    public static PersistenceObjects Create(DomainEvent domainEvent)
//    {
//        var inVertex = Vertex.Create(domainEvent.Target!.AsDto());
//        var outVertex = Vertex.Create(domainEvent.Source!.AsDto());
//    }

//    internal static PersistenceObjects Create(IDto source, IDto target)
//    {
//        Vertex sourceVertex = Vertex.Create(source);
//        var targetVertex = Vertex.Create(target);
//        var edge = new Edge()
//        {
//            Label = "HasA",
//            In = targetVertex,
//            Out = sourceVertex
//        };

//        return new PersistenceObjects()
//        {
//            Edge = edge,
//            In = targetVertex,
//            Out = sourceVertex
//        };
//    }

//}

//public record Vertex : GraphDbObject
//{

//    #region Instance Values

//    [Required]
//    [StringLength(1)]
//    public string Label { get; set; } = string.Empty;

//    [Required]
//    public dynamic Id { get; set; } = default!;

//    [Required]
//    public IEnumerable<Property> Properties { get; set; } = Array.Empty<Property>(); 
//    public PersistenceObjects[] NestedObjects { get; set; } = Array.Empty<PersistenceObjects>();

//    public static Vertex Create(IDto dto)
//    {
//        var properties = dto.GetProperties();

//        var nestedObjects = properties.Values.Where(a => a.GetType() == typeof(IDto));
//        var p = PersistenceObjects.Create()
//        //var properties = Property.CreateProperties(dto.GetProperties());
//        return new Vertex()
//        {
//            Id = dto.GetId(),
//            Label = GetLabel(dto), 
//        }
//    }

//    private PersistenceObjects GetNestedObjects(IEnumerable<IDto> nestedObjects)
//    {
//        foreach (var dto in nestedObjects)
//        {

//        }
//    }
//    #endregion
//}

//public class Property
//{
//    #region Instance Values

//    [Required]
//    [MinLength(1)] 
//    public string Name { get; set; } = string.Empty;

//    [Required]
//    public dynamic Value { get; set; } = null!;

//    public static Property CreateProperty(string name, dynamic value) =>
//        new Property()
//        {
//            Name = name,
//            Value = value
//        };

//    public static IEnumerable<Property> CreateProperties(IDictionary<string, dynamic> values)
//    {
//        if (values.Any(a => a.Value.GetType() == typeof(IDto)))
//            throw new InvalidOperationException();

//        foreach (var value in values)
//            yield return CreateProperty(value.Key, value.Value);
//    }

//    #endregion
//}

//public record Edge : GraphDbObject
//{
//    #region Instance Values

//    [Required]
//    [StringLength(1)]
//    public string Label { get; set; } = string.Empty;

//    public IEnumerable<Property> Properties { get; set; } = Array.Empty<Property>();
//    public Vertex In { get; set; } = null!;
//    public Vertex Out { get; set; } = null!;

//    public static Edge Create(DomainEvent domainEvent)
//    {
//        var label = GetLabel(domainEvent);
//        var outVertex = domainEvent.Source;
//        var inVertex = domainEvent.Target;

//    }

//    #endregion
//}

//public static class Converter
//{
//    private string CreateNestedRelationship(IEnumerable<IDto> nestedEntities)
//    {
//        IEnumerable<Vertex> childVertices = new List<Vertex>();

//        foreach (var vertex in childVertices)
//        {
//            Vertex child = new Vertex() { Id = vertex.Id,  }
//        }
//    }
//}

//public class Hello<_T, _E> where _T : IAggregate where _E : IEntity
//{
//    #region Instance Members

//    public void Hi(DomainEvent<_T, _E> poopie)
//    {
//        var fromVerticeLabel = GetLabel(poopie.Source);
//        var toVerticeLabeltarget = GetLabel(poopie.Target);
//        var edgeLabel = poopie.GetType().Name;

//        var dateTime = poopie.DateTimeUtc;
//    }

//    public void ToVerticeStuff(IEntity poopie)
//    {
//        var label = GetLabel(poopie);
//        var dto = poopie.AsDto();

//        Dictionary<string, dynamic> properties = new();

//        foreach (var property in dto.GetProperties())
//            properties.Add(property.Key, property.Value);
//    }

//    public IEnumerable<Vertex> GetNestedVertexes(IDto dto)
//    {
//        var properties = dto.GetProperties();

//        if (!properties.Any(a => typeof(IDto) == a.GetType()))
//            return Array.Empty<Vertex>();

//        foreach (var nestedDto in dto.GetProperties().Where(a => a.Value.GetType() == typeof(IDto)))
//            throw new NotImplementedException();

//        throw new NotImplementedException();
//    }

//    public void FromVerticeStuff(IAggregate poopie)
//    {
//        var label = GetLabel(poopie);
//    }

//    public void EdgeStuff(DomainEvent<_T, _E> poopie)
//    {
//        var label = GetLabel(poopie);
//    }

//    #endregion
//}