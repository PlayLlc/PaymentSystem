using AutoFixture.Kernel;

using Play.Emv.Ber;
using Play.Icc.Exceptions;

namespace Play.Testing.Emv;

public class AlternateInterfacePreferenceOutcomeBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(AlternateInterfacePreferenceOutcomeBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="IccProtocolException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(AlternateInterfacePreferenceOutcome))
            return new NoSpecimen();

        return AlternateInterfacePreferenceOutcome.NotAvailable;
    }

    #endregion
}