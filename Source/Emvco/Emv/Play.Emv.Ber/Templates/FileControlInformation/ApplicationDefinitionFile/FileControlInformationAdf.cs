using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Ber.Templates;

public record FileControlInformationAdf : FileControlInformationTemplate
{
    #region Instance Values

    private readonly DedicatedFileName _DedicatedFileName;
    private readonly FileControlInformationProprietaryAdf _FileControlInformationProprietary;

    #endregion

    #region Constructor

    public FileControlInformationAdf(DedicatedFileName dedicatedFileName, FileControlInformationProprietaryAdf fileControlInformationProprietary)
    {
        _DedicatedFileName = dedicatedFileName;
        _FileControlInformationProprietary = fileControlInformationProprietary;
    }

    #endregion

    #region Instance Members

    public bool TryGetLanguagePreference(out LanguagePreference? result) => _FileControlInformationProprietary.TryGetLanguagePreference(out result);
    public Tag[] GetRequestedDataObjects() => _FileControlInformationProprietary.GetRequestedDataObjects();

    public bool TryGetProcessingOptionsDataObjectList(out ProcessingOptionsDataObjectList? result) =>
        _FileControlInformationProprietary.TryGetProcessingOptionsDataObjectList(out result);

    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public bool GetProcessingOptionsDataObjectListResult(IReadTlvDatabase database, out CommandTemplate? result) =>
        _FileControlInformationProprietary.TryGetProcessingOptionsRelatedData(database, out result);

    public DedicatedFileName GetDedicatedFileName() => _DedicatedFileName;
    public override FileControlInformationProprietaryAdf GetFileControlInformationProprietary() => _FileControlInformationProprietary;
    public override Tag GetTag() => Tag;

    public override Tag[] GetChildTags()
    {
        return new[] {DedicatedFileName.Tag, FileControlInformationProprietaryTemplate.Tag};
    }

    /// <exception cref="OverflowException"></exception>
    public override ushort GetValueByteCount(BerCodec codec) => GetValueByteCount();

    public bool IsDataObjectRequested(Tag tag) => _FileControlInformationProprietary.IsDataObjectRequested(tag);
    public bool IsNetworkOf(RegisteredApplicationProviderIndicators rid) => rid == _DedicatedFileName.GetRegisteredApplicationProviderIdentifier();

    public bool TryGetApplicationCapabilitiesInformation(out ApplicationCapabilitiesInformation? result) =>
        _FileControlInformationProprietary.TryGetApplicationCapabilitiesInformation(out result);

    protected override IEncodeBerDataObjects?[] GetChildren()
    {
        return new IEncodeBerDataObjects?[] {_DedicatedFileName, _FileControlInformationProprietary};
    }

    #endregion

    #region Serialization

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CardDataMissingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static FileControlInformationAdf Decode(ReadOnlyMemory<byte> rawBer) => Decode(_Codec.DecodeChildren(rawBer));

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CardDataMissingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    private static FileControlInformationAdf Decode(EncodedTlvSiblings encodedSiblings)
    {
        DedicatedFileName dedicatedFileName = _Codec.AsPrimitive(DedicatedFileName.Decode, DedicatedFileName.Tag, encodedSiblings)
            ?? throw new CardDataMissingException(
                $"A problem occurred while decoding {nameof(FileControlInformationAdf)}. A {nameof(DedicatedFileName)} was expected but could not be found");

        FileControlInformationProprietaryAdf fciProprietary =
            _Codec.AsConstructed(FileControlInformationProprietaryAdf.Decode, FileControlInformationProprietaryTemplate.Tag, encodedSiblings)
            ?? throw new CardDataMissingException(
                $"A problem occurred while decoding {nameof(FileControlInformationAdf)}. A {nameof(FileControlInformationProprietaryAdf)} was expected but could not be found");

        return new FileControlInformationAdf(dedicatedFileName, fciProprietary);
    }

    #endregion

    #region Equality

    public bool Equals(FileControlInformationAdf x, FileControlInformationAdf y) => x.Equals(y);

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