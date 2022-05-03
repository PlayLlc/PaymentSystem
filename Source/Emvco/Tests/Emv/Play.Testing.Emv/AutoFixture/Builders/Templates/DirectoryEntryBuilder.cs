using AutoFixture.Kernel;

using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.Templates;

namespace Play.Testing.Emv;

public class DirectoryEntryBuilder : ConstructedValueSpecimenBuilder<DirectoryEntry>
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(DirectoryEntryBuilder));

    private static readonly byte[] _RawTagLengthValue =
    {
        0x61, 0x10,

        // Application Dedicated File Name
        0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x98, 0x08, 0x40,

        // Application Priority Indicator
        0x87, 0x01, 0x02,

        // Kernel Identifier
        0x9F, 0x2A, 0x01, 0x03
    };

    private static readonly byte[] _ContentOctets =
    {
        // Application Dedicated File Name
        0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x98, 0x08, 0x40,

        // Application Priority Indicator
        0x87, 0x01, 0x02,

        // Kernel Identifier
        0x9F, 0x2A, 0x01, 0x03
    };

    #endregion

    #region Constructor

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public DirectoryEntryBuilder() : base(
        new DefaultConstructedValueSpecimen<DirectoryEntry>(DirectoryEntry.Decode(_RawTagLengthValue.AsMemory()), _ContentOctets))
    { }

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(DirectoryEntry))
            return new NoSpecimen();

        return GetDefault();
    }

    #endregion
}