using AutoFixture.Kernel;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Randoms;

namespace Play.Testing.Emv;

public class DirectoryEntryBuilder : ConstructedValueSpecimenBuilder<DirectoryEntry>
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(DirectoryEntryBuilder));

    #endregion

    #region Constructor

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public DirectoryEntryBuilder() : base(
        new DefaultConstructedValueSpecimen<DirectoryEntry>(DirectoryEntry.Decode(GetContentOctets().AsMemory()), GetContentOctets()))
    { }

    #endregion

    #region Instance Members

    // 0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x98, 0x08, 0x40
    private static byte[] GetContentOctets() =>
        new byte[]
        {
            0x61, 0x10, 0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x98, 0x08,
            0x40, 0x87, 0x01, 0x02, 0x9F, 0x2A, 0x01, 0x03
        };

    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(AmountAuthorizedNumeric))
            return new NoSpecimen();

        return new AmountAuthorizedNumeric(Randomize.Integers.ULong());
    }

    #endregion
}