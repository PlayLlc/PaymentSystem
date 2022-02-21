using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.DataElements.Emv;

namespace Play.Emv.Templates.FileControlInformation;

public class FileControlInformationIssuerDiscretionaryDataDdf : FileControlInformationIssuerDiscretionaryDataTemplate
{
    #region Static Metadata

    public new static readonly Tag Tag = FileControlInformationIssuerDiscretionaryDataTemplate.Tag;

    #endregion

    #region Instance Values

    private readonly ApplicationCapabilitiesInformation? _ApplicationCapabilitiesInformation;

    #endregion

    #region Constructor

    public FileControlInformationIssuerDiscretionaryDataDdf(ApplicationCapabilitiesInformation? applicationCapabilitiesInformation)
    {
        _ApplicationCapabilitiesInformation = applicationCapabilitiesInformation;
    }

    #endregion

    #region Instance Members

    public override Tag GetTag() => Tag;

    public override Tag[] GetChildTags()
    {
        return new[] {ApplicationCapabilitiesInformation.Tag};
    }

    protected override IEncodeBerDataObjects?[] GetChildren()
    {
        return new IEncodeBerDataObjects?[] {_ApplicationCapabilitiesInformation};
    }

    public bool TryGetApplicationCapabilitiesInformation(out ApplicationCapabilitiesInformation? result)
    {
        if (_ApplicationCapabilitiesInformation is null)
        {
            result = null;

            return false;
        }

        result = _ApplicationCapabilitiesInformation!;

        return true;
    }

    #endregion

    #region Serialization

    public static FileControlInformationIssuerDiscretionaryDataDdf Decode(ReadOnlyMemory<byte> rawBer) =>
        Decode(_Codec.DecodeChildren(rawBer));

    /// <exception cref="BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerInternalException"></exception>
    public static FileControlInformationIssuerDiscretionaryDataDdf Decode(EncodedTlvSiblings encodedChildren) =>
        new(_Codec.AsPrimitive(ApplicationCapabilitiesInformation.Decode, ApplicationCapabilitiesInformation.Tag, encodedChildren));

    #endregion

    #region Equality

    public override bool Equals(ConstructedValue? x, ConstructedValue? y)
    {
        if (x == null)
            return y == null;

        return (y != null) && x.Equals(y);
    }

    public bool Equals(FileControlInformationIssuerDiscretionaryDataDdf? x, FileControlInformationIssuerDiscretionaryDataDdf? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override bool Equals(ConstructedValue? other) => other is FileControlInformationIssuerDiscretionaryDataDdf adf && Equals(adf);

    public bool Equals(FileControlInformationIssuerDiscretionaryDataDdf other)
    {
        if (other.GetTag() != GetTag())
            return false;

        return true;
    }

    public override bool Equals(object? obj) =>
        obj is FileControlInformationIssuerDiscretionaryDataDdf fileControlInformationIssuerDiscretionaryDataAdf
        && Equals(fileControlInformationIssuerDiscretionaryDataAdf);

    public override int GetHashCode(ConstructedValue obj) => obj.GetHashCode();
    public int GetHashCode(FileControlInformationIssuerDiscretionaryDataDdf obj) => obj.GetHashCode();
    public override int GetHashCode() => (int) unchecked(643949 * Tag);

    #endregion
}