using AutoFixture.Kernel;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;

namespace Play.Testing.Emv;

public class FileControlInformationIssuerDiscretionaryPpseBuilder : ConstructedValueSpecimenBuilder<FileControlInformationIssuerDiscretionaryDataPpse>
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(FileControlInformationIssuerDiscretionaryPpseBuilder));

    private static readonly byte[] _RawTagLengthValue =
    {
        // Fci Issuer Discretionary Ppse
        0xBF, 0x0C, 0x24,

        // SetOf<Directory Entry>[0]
        0x61, 0x10, 0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x98, 0x08,
        0x40, 0x87, 0x01, 0x02, 0x9F, 0x2A, 0x01, 0x03,

        // SetOf<Directory Entry>[1]
        0x61, 0x10, 0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x98, 0x08,
        0x40, 0x87, 0x01, 0x02, 0x9F, 0x2A, 0x01, 0x03
    };

    private static readonly byte[] _ContentOctets =
    {
        // SetOf<Directory Entry>[0]
        0x61, 0x10, 0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x98, 0x08,
        0x40, 0x87, 0x01, 0x02, 0x9F, 0x2A, 0x01, 0x03,

        // SetOf<Directory Entry>[1]
        0x61, 0x10, 0x4F, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x98, 0x08,
        0x40, 0x87, 0x01, 0x02, 0x9F, 0x2A, 0x01, 0x03
    };

    #endregion

    #region Constructor

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public FileControlInformationIssuerDiscretionaryPpseBuilder() : base(
        new DefaultConstructedValueSpecimen<FileControlInformationIssuerDiscretionaryDataPpse>(
            FileControlInformationIssuerDiscretionaryDataPpse.Decode(_RawTagLengthValue), _ContentOctets))
    { }

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(FileControlInformationIssuerDiscretionaryDataPpse))
            return new NoSpecimen();

        return GetDefault();
    }

    #endregion
}