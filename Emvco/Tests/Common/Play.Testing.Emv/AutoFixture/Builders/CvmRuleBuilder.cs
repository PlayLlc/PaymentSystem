using AutoFixture.Kernel;

using Play.Emv.Ber;
using Play.Emv.Ber.ValueTypes;
using Play.Randoms;

namespace Play.Testing.Emv;

public class CvmRuleBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(CvmRuleBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(CvmRule))
            return new NoSpecimen();

        return new CvmRule(Randomize.Arrays.Bytes(2));
    }

    #endregion
}