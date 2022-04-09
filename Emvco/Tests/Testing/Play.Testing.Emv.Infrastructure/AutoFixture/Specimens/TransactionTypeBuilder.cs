using AutoFixture.Kernel;

using Play.Emv.Ber;
using Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders;
using Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders.Specimens;

namespace Play.Testing.Emv.Infrastructure.AutoFixture;

internal class TransactionTypeBuilder : SpecimenBuilder
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