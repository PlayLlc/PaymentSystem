using System.Linq.Expressions;

namespace Play.MerchantPortal.Infrastructure.Persistence.Mongo.Mongo.Helpers;

public class AddFieldConfig<T, Titem>
{
    public Titem Field { get; set; } = default!;

    public Expression<Func<T, IEnumerable<Titem>>> FieldDefinition { get; set; } = default!;
}
