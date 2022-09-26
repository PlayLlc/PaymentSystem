using System.Linq.Expressions;

namespace Play.Persistence.Mongo.Helpers;

public class AddFieldConfig<T, Titem>
{
    #region Instance Values

    public Titem Field { get; set; } = default!;

    public Expression<Func<T, IEnumerable<Titem>>> FieldDefinition { get; set; } = default!;

    #endregion
}