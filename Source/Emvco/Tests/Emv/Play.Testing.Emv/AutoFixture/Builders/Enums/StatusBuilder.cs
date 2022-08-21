using AutoFixture.Kernel;

using Play.Emv.Ber.Enums;

namespace Play.Testing.Emv;

public class StatusBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(StatusBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(DisplayStatuses))
            return new NoSpecimen();

        DisplayStatuses[] all = DisplayStatuses.Empty.GetAll();

        return all[new Random().Next(0, all.Length - 1)];
    }

    #endregion
}