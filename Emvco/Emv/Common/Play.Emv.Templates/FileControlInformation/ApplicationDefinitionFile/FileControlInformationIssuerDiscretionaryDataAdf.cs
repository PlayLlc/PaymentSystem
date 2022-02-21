using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.DataElements;
using Play.Emv.DataElements.Emv;

namespace Play.Emv.Templates.FileControlInformation;

public class FileControlInformationIssuerDiscretionaryDataAdf : FileControlInformationIssuerDiscretionaryDataTemplate
{
    #region Static Metadata

    public new static readonly Tag Tag = FileControlInformationIssuerDiscretionaryDataTemplate.Tag;

    #endregion

    #region Instance Values

    private readonly LogEntry? _LogEntry;
    private readonly ApplicationCapabilitiesInformation? _ThirdPartyData;

    #endregion

    #region Constructor

    public FileControlInformationIssuerDiscretionaryDataAdf(LogEntry? logEntry, ApplicationCapabilitiesInformation? thirdPartyData)
    {
        _LogEntry = logEntry;
        _ThirdPartyData = thirdPartyData;
    }

    #endregion

    #region Instance Members

    public override Tag GetTag() => Tag;

    public override Tag[] GetChildTags()
    {
        return new[] {LogEntry.Tag, ApplicationCapabilitiesInformation.Tag};
    }

    protected override IEncodeBerDataObjects?[] GetChildren()
    {
        return new IEncodeBerDataObjects?[] {_LogEntry, _ThirdPartyData};
    }

    public bool TryGetApplicationCapabilitiesInformation(out ApplicationCapabilitiesInformation? result)
    {
        if (_ThirdPartyData == null)
        {
            result = default;

            return false;
        }

        result = _ThirdPartyData!;

        return true;
    }

    #endregion

    #region Serialization

    public static FileControlInformationIssuerDiscretionaryDataAdf Decode(ReadOnlyMemory<byte> value) =>
        Decode(_Codec.DecodeChildren(value));

    /// <exception cref="BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerInternalException"></exception>
    public static FileControlInformationIssuerDiscretionaryDataAdf Decode(EncodedTlvSiblings encodedTlvSiblings) =>
        new(_Codec.AsPrimitive(LogEntry.Decode, LogEntry.Tag, encodedTlvSiblings),
            _Codec.AsPrimitive(ApplicationCapabilitiesInformation.Decode, ApplicationCapabilitiesInformation.Tag, encodedTlvSiblings));

    #endregion

    #region Equality

    public override bool Equals(ConstructedValue? x, ConstructedValue? y)
    {
        if (x == null)
            return y == null;

        return (y != null) && x.Equals(y);
    }

    public bool Equals(FileControlInformationIssuerDiscretionaryDataAdf? x, FileControlInformationIssuerDiscretionaryDataAdf? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override bool Equals(ConstructedValue? other) => other is FileControlInformationIssuerDiscretionaryDataAdf adf && Equals(adf);

    public bool Equals(FileControlInformationIssuerDiscretionaryDataAdf other)
    {
        if (_LogEntry is null && other._LogEntry is not null)
            return false;
        if (_LogEntry is not null && !_LogEntry.Equals(other._LogEntry))
            return false;

        if (_ThirdPartyData is null && other._ThirdPartyData is not null)
            return false;
        if (_ThirdPartyData is not null && !_ThirdPartyData.Equals(other._ThirdPartyData))
            return false;

        return true;
    }

    public override bool Equals(object? obj) =>
        obj is FileControlInformationIssuerDiscretionaryDataAdf fileControlInformationIssuerDiscretionaryDataAdf
        && Equals(fileControlInformationIssuerDiscretionaryDataAdf);

    public override int GetHashCode(ConstructedValue obj) => obj.GetHashCode();
    public int GetHashCode(FileControlInformationIssuerDiscretionaryDataAdf obj) => obj.GetHashCode();
    public override int GetHashCode() => (int) unchecked(((643949 * Tag) + _LogEntry?.GetHashCode()) ?? 0);

    #endregion
}