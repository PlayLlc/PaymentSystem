using System.Linq.Expressions;

namespace Play.Persistence.Mongo.Helpers;

public class AddFieldConfig<_, _Titem>
{
    #region Instance Values

    public _Titem Field { get; set; } = default!;

    public Expression<Func<_, IEnumerable<_Titem>>> FieldDefinition { get; set; } = default!;

    #endregion
}