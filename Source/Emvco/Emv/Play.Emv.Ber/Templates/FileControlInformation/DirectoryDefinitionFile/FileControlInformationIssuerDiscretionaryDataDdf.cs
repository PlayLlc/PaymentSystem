using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Ber.Templates;

public record FileControlInformationIssuerDiscretionaryDataDdf : FileControlInformationIssuerDiscretionaryDataTemplate
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

    public static FileControlInformationIssuerDiscretionaryDataDdf Decode(ReadOnlyMemory<byte> rawBer) => Decode(_Codec.DecodeChildren(rawBer));

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Exceptions._Temp.BerFormatException"></exception>
    public static FileControlInformationIssuerDiscretionaryDataDdf Decode(EncodedTlvSiblings encodedChildren) =>
        new(_Codec.AsPrimitive(ApplicationCapabilitiesInformation.Decode, ApplicationCapabilitiesInformation.Tag, encodedChildren));

    #endregion
}