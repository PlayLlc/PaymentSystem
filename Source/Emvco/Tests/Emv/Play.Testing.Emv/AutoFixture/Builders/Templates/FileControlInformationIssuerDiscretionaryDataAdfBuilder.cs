using AutoFixture.Kernel;

using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Randoms;

namespace Play.Testing.Emv;

public class FileControlInformationIssuerDiscretionaryDataAdfBuilder : ConstructedValueSpecimenBuilder<FileControlInformationIssuerDiscretionaryDataAdf>
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(FileControlInformationIssuerDiscretionaryDataAdfBuilder));

    #endregion

    #region Constructor

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public FileControlInformationIssuerDiscretionaryDataAdfBuilder() : base(Goodbye())
    { }

    #endregion

    #region Instance Members

    private static DefaultConstructedValueSpecimen<FileControlInformationIssuerDiscretionaryDataAdf> Goodbye()
    {
        FileControlInformationIssuerDiscretionaryDataAdf aa = Test();
        DefaultConstructedValueSpecimen<FileControlInformationIssuerDiscretionaryDataAdf> a = new(aa, GetContentOctets());

        return a;
    }

    private static FileControlInformationIssuerDiscretionaryDataAdf Test()
    {
        FileControlInformationIssuerDiscretionaryDataAdf a = FileControlInformationIssuerDiscretionaryDataAdf.Decode(new byte[] {0xBF, 0x0C, 0x00}.AsMemory());

        return a;
    }

    private static byte[] GetContentOctets() => new byte[] {0xBF, 0x0C, 0x00};
    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(FileControlInformationIssuerDiscretionaryDataAdf))
            return new NoSpecimen();

        return GetDefault();
    }

    #endregion
}