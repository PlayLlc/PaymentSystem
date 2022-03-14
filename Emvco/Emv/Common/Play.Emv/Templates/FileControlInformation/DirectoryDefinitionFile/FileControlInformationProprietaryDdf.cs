using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.DataElements;
using Play.Emv.Exceptions;

namespace Play.Emv.Templates;

public class FileControlInformationProprietaryDdf : FileControlInformationProprietaryTemplate
{
    #region Instance Values

    private readonly FileControlInformationIssuerDiscretionaryDataDdf _FileControlInformationIssuerDiscretionaryDataAdf;
    private readonly ShortFileIdentifier _ShortFileIdentifier;

    #endregion

    #region Constructor

    public FileControlInformationProprietaryDdf(
        FileControlInformationIssuerDiscretionaryDataDdf fileControlInformationIssuerDiscretionaryData,
        ShortFileIdentifier shortFileIdentifier)
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

    public static FileControlInformationProprietaryDdf Decode(ReadOnlyMemory<byte> rawBer) => Decode(_Codec.DecodeChildren(rawBer));

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static FileControlInformationProprietaryDdf Decode(EncodedTlvSiblings encodedChildren)
    {
        FileControlInformationIssuerDiscretionaryDataDdf fciProprietaryTemplate =
            _Codec.AsConstructed(FileControlInformationIssuerDiscretionaryDataDdf.Decode,
                                 FileControlInformationIssuerDiscretionaryDataDdf.Tag, encodedChildren)
            ?? throw new
                CardDataMissingException($"A problem occurred while decoding {nameof(FileControlInformationProprietaryDdf)}. A {nameof(FileControlInformationIssuerDiscretionaryDataDdf)} was expected but could not be found");

        ShortFileIdentifier shortFileIdentifier = _Codec.AsPrimitive(ShortFileIdentifier.Decode, ShortFileIdentifier.Tag, encodedChildren)
            ?? throw new
                CardDataMissingException($"A problem occurred while decoding {nameof(ShortFileIdentifier)}. A {nameof(FileControlInformationProprietaryDdf)} was expected but could not be found");

        return new FileControlInformationProprietaryDdf(fciProprietaryTemplate, shortFileIdentifier);
    }

    #endregion

    #region Equality

    public override bool Equals(ConstructedValue? other) =>
        other is FileControlInformationProprietaryDdf fileControlInformationProprietaryTemplate
        && Equals(fileControlInformationProprietaryTemplate);

    public bool Equals(FileControlInformationProprietaryDdf? other)
    {
        if (other == null)
            return false;

        return (other.GetTag() == Tag)
            && _ShortFileIdentifier.Equals(other._ShortFileIdentifier)
            && _FileControlInformationIssuerDiscretionaryDataAdf.Equals(other._FileControlInformationIssuerDiscretionaryDataAdf);
    }

    public override bool Equals(ConstructedValue? x, ConstructedValue? y) =>
        x is FileControlInformationProprietaryDdf x1 && y is FileControlInformationProprietaryDdf y1 && x1.Equals(y1);

    public bool Equals(FileControlInformationProprietaryDdf? x, FileControlInformationProprietaryDdf? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override bool Equals(object? obj) =>
        obj is FileControlInformationProprietaryDdf fileControlInformationProprietaryTemplate
        && Equals(fileControlInformationProprietaryTemplate);

    public override int GetHashCode(ConstructedValue obj) => obj.GetHashCode();
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