using System.Linq.Expressions;

namespace Play.Infrastructure.Persistence.Mongo;

public class AddFieldConfig<T, Titem>
{
    public Titem Field { get; set; } = default!;

    public Expression<Func<T, IEnumerable<Titem>>> FieldDefinition { get; set; } = default!;
}
