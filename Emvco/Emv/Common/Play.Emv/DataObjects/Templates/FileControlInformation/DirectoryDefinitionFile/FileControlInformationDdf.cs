using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Exceptions;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Templates;

public class FileControlInformationDdf : FileControlInformationTemplate
{
    #region Instance Values

    private readonly DedicatedFileName _DedicatedFileName;
    private readonly FileControlInformationProprietaryDdf _FileControlInformationProprietary;

    #endregion

    #region Constructor

    public FileControlInformationDdf(
        DedicatedFileName dedicatedFileName,
        FileControlInformationProprietaryDdf fileControlInformationProprietary)
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

    public static FileControlInformationDdf Decode(ReadOnlyMemory<byte> rawBer) => Decode(_Codec.DecodeChildren(rawBer));

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CardDataMissingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    private static FileControlInformationDdf Decode(EncodedTlvSiblings encodedSiblings)
    {
        DedicatedFileName dedicatedFileName = _Codec.AsPrimitive(DedicatedFileName.Decode, DedicatedFileName.Tag, encodedSiblings)
            ?? throw new
                CardDataMissingException($"A problem occurred while decoding {nameof(FileControlInformationDdf)}. A {nameof(DedicatedFileName)} was expected but could not be found");

        FileControlInformationProprietaryDdf fciProprietary =
            _Codec.AsConstructed(FileControlInformationProprietaryDdf.Decode, FileControlInformationProprietaryTemplate.Tag,
                                 encodedSiblings)
            ?? throw new
                CardDataMissingException($"A problem occurred while decoding {nameof(FileControlInformationDdf)}. A {nameof(FileControlInformationProprietaryDdf)} was expected but could not be found");

        return new FileControlInformationDdf(dedicatedFileName, fciProprietary);
    }

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is FileControlInformationDdf fci && Equals(fci);
    public override bool Equals(ConstructedValue? other) => other is FileControlInformationDdf adf && Equals(adf);

    public bool Equals(FileControlInformationDdf other) =>
        _DedicatedFileName.Equals(other._DedicatedFileName)
        && _FileControlInformationProprietary.Equals(other._FileControlInformationProprietary);

    public override bool Equals(ConstructedValue? x, ConstructedValue? y)
    {
        if (x == null)
            return y == null;

        if (y == null)
            return false;

        return x.Equals(y);
    }

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

    public override int GetHashCode(ConstructedValue obj) => obj.GetHashCode();

    #endregion
}