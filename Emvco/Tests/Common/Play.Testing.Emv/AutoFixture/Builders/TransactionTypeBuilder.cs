using AutoFixture.Kernel;

using Play.Emv.Ber;

namespace Play.Testing.Emv;

public class TransactionTypeBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(TransactionTypeBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(TransactionType))
            return new NoSpecimen();

        return new TransactionType((byte) new Random().Next(0, 99));
    }

    #endregion
}