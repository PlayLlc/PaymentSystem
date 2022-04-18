using AutoFixture.Kernel;

using Play.Globalization.Time;
using Play.Icc.Exceptions;

namespace Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders;

public class DateTimeUtcBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(DateTimeUtcBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => new(nameof(DateTimeUtcBuilder));

    /// <exception cref="IccProtocolException"></exception>
    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(ShortDate))
            return new NoSpecimen();

        return DateTimeUtc.Now;
    }

    #endregion
}