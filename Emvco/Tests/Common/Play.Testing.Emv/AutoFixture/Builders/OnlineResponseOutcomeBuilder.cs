using AutoFixture.Kernel;

using Play.Emv.Ber;

namespace Play.Testing.Emv;

public class OnlineResponseOutcomeBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(OnlineResponseOutcomeBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(OnlineResponseOutcome))
            return new NoSpecimen();

        return OnlineResponseOutcome.NotAvailable;
    }

    #endregion
}