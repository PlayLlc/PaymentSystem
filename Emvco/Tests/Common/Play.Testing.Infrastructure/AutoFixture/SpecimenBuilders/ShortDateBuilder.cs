using AutoFixture.Kernel;

using Play.Core.Exceptions;
using Play.Globalization.Time;
using Play.Icc.Exceptions;

namespace Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders;

public class ShortDateBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(ShortDateBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="IccProtocolException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(ShortDate))
            return new NoSpecimen();

        return ShortDate.Today;
    }

    #endregion
}