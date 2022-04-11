using AutoFixture.Kernel;

using Play.Emv.Ber;

namespace Play.Testing.Emv;

public class TerminalCategoryCodeBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(TerminalCategoryCodeBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(TerminalCategoryCode))
            return new NoSpecimen();

        TerminalCategoryCode[] all = TerminalCategoryCode.GetAll();

        return all[new Random().Next(0, all.Length - 1)];
    }

    #endregion
}