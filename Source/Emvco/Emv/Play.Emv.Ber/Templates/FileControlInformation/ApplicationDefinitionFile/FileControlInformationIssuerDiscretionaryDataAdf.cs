using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Ber.Templates;

public record FileControlInformationIssuerDiscretionaryDataAdf : FileControlInformationIssuerDiscretionaryDataTemplate
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

    /// <exception cref="BerParsingException"></exception>
    public static FileControlInformationIssuerDiscretionaryDataAdf Decode(ReadOnlyMemory<byte> value) => Decode(_Codec.DecodeChildren(value));

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static FileControlInformationIssuerDiscretionaryDataAdf Decode(EncodedTlvSiblings encodedTlvSiblings)
    {
        LogEntry? logEntry = _Codec.AsPrimitive(LogEntry.Decode, LogEntry.Tag, encodedTlvSiblings);
        ApplicationCapabilitiesInformation? aci = _Codec.AsPrimitive(ApplicationCapabilitiesInformation.Decode, ApplicationCapabilitiesInformation.Tag,
            encodedTlvSiblings);

        return new FileControlInformationIssuerDiscretionaryDataAdf(logEntry, aci);
    }

    #endregion
}