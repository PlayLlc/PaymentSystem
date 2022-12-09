using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.Entities;

namespace Play.Persistence.Gremlin;

public class Property
{
    #region Instance Values

    [Required]
    [MinLength(1)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public dynamic Value { get; set; } = null!;

    #endregion

    #region Instance Members

    public static Property CreateProperty(string name, dynamic value)
    {
        if (value.GetType() == typeof(IDto))
            throw new InvalidOperationException();

        if (value.GetType() == typeof(IAggregate))
            throw new InvalidOperationException();

        if (value.GetType() == typeof(IEntity))
            throw new InvalidOperationException();

        return new Property()
        {
            Name = name,
            Value = value
        };
    }

    public static IEnumerable<Property> CreateProperties(IDictionary<string, dynamic> values)
    {
        foreach (var value in values)
            yield return CreateProperty(value.Key, value.Value);
    }

    public static IEnumerable<Property> CreateProperties(IEnumerable<PropertyInfo> values)
    {
        foreach (var value in values)
            yield return CreateProperty(GetPropertyName(value), GetPropertyValue(value));
    }

    private static string GetPropertyName(PropertyInfo value) => value.GetType().GetProperties().First().Name;
    private static dynamic? GetPropertyValue(PropertyInfo value) => value.GetType().GetProperties().First().GetValue(value);

    #endregion
}