using AutoFixture.Kernel;

using Play.Emv.Ber;
using Play.Emv.Ber.ValueTypes;
using Play.Randoms;

namespace Play.Testing.Emv;

public class MessageTableEntryBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(MessageTableEntryBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(MessageTableEntry))
            return new NoSpecimen();

        return new MessageTableEntry(Randomize.Arrays.Bytes(8));
    }

    #endregion
}