using AutoFixture.Kernel;

using Play.Emv.Ber;

namespace Play.Testing.Emv;

public class StartOutcomeBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(StartOutcomeBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(StartOutcome))
            return new NoSpecimen();

        StartOutcome[] all = StartOutcome.GetAll();

        return all[new Random().Next(0, all.Length - 1)];
    }

    #endregion
}