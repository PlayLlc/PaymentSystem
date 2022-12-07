using AutoFixture.Kernel;

using Play.Emv.Ber.Enums;
using Play.Icc.Exceptions;

namespace Play.Testing.Emv;

public class ValueQualifierBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(ValueQualifierBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="IccProtocolException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(ValueQualifiers))
            return new NoSpecimen();

        ValueQualifiers[] all = ValueQualifiers.Empty.GetAll();

        return all[new Random().Next(0, all.Length - 1)];
    }

    #endregion
}