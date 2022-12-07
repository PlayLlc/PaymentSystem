using AutoFixture.Kernel;

using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Templates;

namespace Play.Testing.Emv;

public class FileControlInformationIssuerDiscretionaryDataAdfBuilder : ConstructedValueSpecimenBuilder<FileControlInformationIssuerDiscretionaryDataDdf>
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(FileControlInformationIssuerDiscretionaryDataAdfBuilder));
    private static readonly byte[] _RawTagLengthValue = {0xBF, 0x0C, 0x00};
    private static readonly byte[] _ContentOctets = Array.Empty<byte>();

    #endregion

    #region Constructor

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public FileControlInformationIssuerDiscretionaryDataAdfBuilder() : base(
        new DefaultConstructedValueSpecimen<FileControlInformationIssuerDiscretionaryDataDdf>(
            FileControlInformationIssuerDiscretionaryDataDdf.Decode(_RawTagLengthValue.AsMemory()), _ContentOctets))
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

        if (type != typeof(FileControlInformationIssuerDiscretionaryDataDdf))
            return new NoSpecimen();

        return GetDefault();
    }

    #endregion
}