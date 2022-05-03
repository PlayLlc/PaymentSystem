using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.Templates;

public record FileControlInformationProprietaryDdf : FileControlInformationProprietaryTemplate
{
    #region Instance Values

    private readonly FileControlInformationIssuerDiscretionaryDataDdf _FileControlInformationIssuerDiscretionaryDataAdf;
    private readonly ShortFileIdentifier _ShortFileIdentifier;

    #endregion

    #region Constructor

    public FileControlInformationProprietaryDdf(
        FileControlInformationIssuerDiscretionaryDataDdf fileControlInformationIssuerDiscretionaryData, ShortFileIdentifier shortFileIdentifier)
    {
        _FileControlInformationIssuerDiscretionaryDataAdf = fileControlInformationIssuerDiscretionaryData;
        _ShortFileIdentifier = shortFileIdentifier;
    }

    #endregion

    #region Instance Members

    public override FileControlInformationIssuerDiscretionaryDataDdf GetFileControlInformationIssuerDiscretionaryData() =>
        _FileControlInformationIssuerDiscretionaryDataAdf;

    public ShortFileIdentifier GetShortFileIdentifier() => _ShortFileIdentifier;
    public override Tag GetTag() => Tag;

    public override Tag[] GetChildTags()
    {
        return new[] {FileControlInformationIssuerDiscretionaryDataTemplate.Tag, ShortFileIdentifier.Tag};
    }

    protected override IEncodeBerDataObjects?[] GetChildren()
    {
        return new IEncodeBerDataObjects?[] {_FileControlInformationIssuerDiscretionaryDataAdf, _ShortFileIdentifier};
    }

    #endregion

    #region Serialization

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CardDataMissingException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static FileControlInformationProprietaryDdf Decode(ReadOnlyMemory<byte> value)
    {
        if (_Codec.DecodeFirstTag(value.Span) == Tag)
            return Decode(_Codec.DecodeChildren(value));

        return Decode(_Codec.DecodeSiblings(value));
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CardDataMissingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static FileControlInformationProprietaryDdf Decode(EncodedTlvSiblings encodedChildren)
    {
        FileControlInformationIssuerDiscretionaryDataDdf fciProprietaryTemplate =
            _Codec.AsConstructed(FileControlInformationIssuerDiscretionaryDataDdf.Decode, FileControlInformationIssuerDiscretionaryDataDdf.Tag, encodedChildren)
            ?? throw new CardDataMissingException(
                $"A problem occurred while decoding {nameof(FileControlInformationProprietaryDdf)}. A {nameof(FileControlInformationIssuerDiscretionaryDataDdf)} was expected but could not be found");

        ShortFileIdentifier shortFileIdentifier = _Codec.AsPrimitive(ShortFileIdentifier.Decode, ShortFileIdentifier.Tag, encodedChildren)
            ?? throw new CardDataMissingException(
                $"A problem occurred while decoding {nameof(ShortFileIdentifier)}. A {nameof(FileControlInformationProprietaryDdf)} was expected but could not be found");

        return new FileControlInformationProprietaryDdf(fciProprietaryTemplate, shortFileIdentifier);
    }

    #endregion

    #region Equality

    public bool Equals(FileControlInformationProprietaryDdf? x, FileControlInformationProprietaryDdf? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(FileControlInformationProprietaryTemplate obj) => obj.GetHashCode();

    public override int GetHashCode()
    {
        const int hash = 1973;

        unchecked
        {
            int result = GetTag().GetHashCode() * hash;

            return _ShortFileIdentifier.GetHashCode() + _FileControlInformationIssuerDiscretionaryDataAdf.GetHashCode();
        }
    }

    #endregion
}