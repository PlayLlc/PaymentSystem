using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Ber.Templates;

public record FileControlInformationDdf : FileControlInformationTemplate
{
    #region Instance Values

    private readonly DedicatedFileName _DedicatedFileName;
    private readonly FileControlInformationProprietaryDdf _FileControlInformationProprietary;

    #endregion

    #region Constructor

    public FileControlInformationDdf(DedicatedFileName dedicatedFileName, FileControlInformationProprietaryDdf fileControlInformationProprietary)
    {
        _DedicatedFileName = dedicatedFileName;
        _FileControlInformationProprietary = fileControlInformationProprietary;
    }

    #endregion

    #region Instance Members

    public DedicatedFileName GetDedicatedFileName() => _DedicatedFileName;
    public override FileControlInformationProprietaryDdf GetFileControlInformationProprietary() => _FileControlInformationProprietary;
    public override Tag GetTag() => Tag;

    public override Tag[] GetChildTags()
    {
        return new[] {DedicatedFileName.Tag, FileControlInformationProprietaryTemplate.Tag};
    }

    protected override IEncodeBerDataObjects?[] GetChildren()
    {
        return new IEncodeBerDataObjects?[] {_DedicatedFileName, _FileControlInformationProprietary};
    }

    #endregion

    #region Serialization

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CardDataMissingException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static FileControlInformationDdf Decode(ReadOnlyMemory<byte> value)
    {
        if (_Codec.DecodeFirstTag(value.Span) == Tag)
            return Decode(_Codec.DecodeSiblings(value));

        return Decode(_Codec.DecodeChildren(value));
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CardDataMissingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private static FileControlInformationDdf Decode(EncodedTlvSiblings encodedSiblings)
    {
        DedicatedFileName dedicatedFileName = _Codec.AsPrimitive(DedicatedFileName.Decode, DedicatedFileName.Tag, encodedSiblings)
            ?? throw new CardDataMissingException(
                $"A problem occurred while decoding {nameof(FileControlInformationDdf)}. A {nameof(DedicatedFileName)} was expected but could not be found");

        FileControlInformationProprietaryDdf fciProprietary =
            _Codec.AsConstructed(FileControlInformationProprietaryDdf.Decode, FileControlInformationProprietaryTemplate.Tag, encodedSiblings)
            ?? throw new CardDataMissingException(
                $"A problem occurred while decoding {nameof(FileControlInformationDdf)}. A {nameof(FileControlInformationProprietaryDdf)} was expected but could not be found");

        return new FileControlInformationDdf(dedicatedFileName, fciProprietary);
    }

    #endregion

    #region Equality

    public bool Equals(FileControlInformationDdf x, FileControlInformationDdf y) => x.Equals(y);

    public override int GetHashCode()
    {
        const int hash = 647;

        unchecked
        {
            int result = GetTag().GetHashCode() * hash;
            result += _DedicatedFileName.GetHashCode();
            result += _FileControlInformationProprietary.GetHashCode();

            return result;
        }
    }

    #endregion
}