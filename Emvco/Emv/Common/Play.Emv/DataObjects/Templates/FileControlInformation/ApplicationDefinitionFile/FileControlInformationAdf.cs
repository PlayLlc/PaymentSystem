using Play.Emv.DataElements;
using Play.Emv.DataElements.Emv;
using Play.Emv.Templates.Exceptions;

namespace Play.Emv.Templates.FileControlInformation;

public class FileControlInformationAdf : FileControlInformationTemplate
{
    #region Instance Values

    private readonly DedicatedFileName _DedicatedFileName;
    private readonly FileControlInformationProprietaryAdf _FileControlInformationProprietary;

    #endregion

    #region Constructor

    public FileControlInformationAdf(
        DedicatedFileName dedicatedFileName,
        FileControlInformationProprietaryAdf fileControlInformationProprietary)
    {
        _DedicatedFileName = dedicatedFileName;
        _FileControlInformationProprietary = fileControlInformationProprietary;
    }

    #endregion

    #region Instance Members

    public bool TryGetLanguagePreference(out LanguagePreference? result) =>
        _FileControlInformationProprietary.TryGetLanguagePreference(out result);

    public Tag[] GetRequestedDataObjects() => _FileControlInformationProprietary.GetRequestedDataObjects();

    public bool TryGetProcessingOptionsDataObjectList(out ProcessingOptionsDataObjectList? result) =>
        _FileControlInformationProprietary.TryGetProcessingOptionsDataObjectList(out result);

    public bool GetProcessingOptionsDataObjectListResult(IQueryTlvDatabase database, out CommandTemplate? result) =>
        _FileControlInformationProprietary.TryGetProcessingOptionsRelatedData(database, out result);

    public DedicatedFileName GetDedicatedFileName() => _DedicatedFileName;
    public override FileControlInformationProprietaryAdf GetFileControlInformationProprietary() => _FileControlInformationProprietary;
    public override Tag GetTag() => Tag;

    public override Tag[] GetChildTags()
    {
        return new[] {DedicatedFileName.Tag, FileControlInformationProprietaryTemplate.Tag};
    }

    public override ushort GetValueByteCount(BerCodec codec) => GetValueByteCount();
    public bool IsDataObjectRequested(Tag tag) => _FileControlInformationProprietary.IsDataObjectRequested(tag);

    public bool IsNetworkOf(RegisteredApplicationProviderIndicators rid) =>
        rid == _DedicatedFileName.GetRegisteredApplicationProviderIdentifier();

    public bool TryGetApplicationCapabilitiesInformation(out ApplicationCapabilitiesInformation? result) =>
        _FileControlInformationProprietary.TryGetApplicationCapabilitiesInformation(out result);

    protected override IEncodeBerDataObjects?[] GetChildren()
    {
        return new IEncodeBerDataObjects?[] {_DedicatedFileName, _FileControlInformationProprietary};
    }

    #endregion

    #region Serialization

    public static FileControlInformationAdf Decode(ReadOnlyMemory<byte> rawBer) => Decode(_Codec.DecodeChildren(rawBer));

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    /// <exception cref="BerInternalException"></exception>
    /// <exception cref="CardDataMissingException"></exception>
    private static FileControlInformationAdf Decode(EncodedTlvSiblings encodedSiblings)
    {
        DedicatedFileName dedicatedFileName = _Codec.AsPrimitive(DedicatedFileName.Decode, DedicatedFileName.Tag, encodedSiblings)
            ?? throw new CardDataMissingException(
                $"A problem occurred while decoding {nameof(FileControlInformationAdf)}. A {nameof(DedicatedFileName)} was expected but could not be found");

        FileControlInformationProprietaryAdf fciProprietary =
            _Codec.AsConstructed(FileControlInformationProprietaryAdf.Decode, FileControlInformationProprietaryTemplate.Tag,
                encodedSiblings)
            ?? throw new CardDataMissingException(
                $"A problem occurred while decoding {nameof(FileControlInformationAdf)}. A {nameof(FileControlInformationProprietaryAdf)} was expected but could not be found");

        return new FileControlInformationAdf(dedicatedFileName, fciProprietary);
    }

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is FileControlInformationAdf fci && Equals(fci);
    public override bool Equals(ConstructedValue? other) => other is FileControlInformationAdf adf && Equals(adf);

    public bool Equals(FileControlInformationAdf other) =>
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

    public override int GetHashCode(ConstructedValue obj) => obj.GetHashCode();

    #endregion
}