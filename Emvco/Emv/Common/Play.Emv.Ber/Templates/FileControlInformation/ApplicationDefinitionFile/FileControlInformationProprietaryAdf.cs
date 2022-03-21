using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.Templates;

public class FileControlInformationProprietaryAdf : FileControlInformationProprietaryTemplate
{
    #region Instance Values

    private readonly FileControlInformationIssuerDiscretionaryDataAdf _FileControlInformationIssuerDiscretionaryDataAdf;
    private readonly ApplicationLabel? _ApplicationLabel;
    private readonly ApplicationPreferredName? _ApplicationPreferredName;
    private readonly ApplicationPriorityIndicator? _ApplicationPriorityIndicator;
    private readonly IssuerCodeTableIndex? _IssuerCodeTableIndex;
    private readonly LanguagePreference? _LanguagePreference;
    private readonly ProcessingOptionsDataObjectList? _ProcessingOptionsDataObjectList;

    #endregion

    #region Constructor

    public FileControlInformationProprietaryAdf(
        FileControlInformationIssuerDiscretionaryDataAdf fileControlInformationIssuerDiscretionaryData,
        ApplicationLabel? applicationLabel,
        ApplicationPriorityIndicator? applicationPriorityIndicator,
        ProcessingOptionsDataObjectList? processingOptionsDataObjectList,
        LanguagePreference? languagePreference,
        IssuerCodeTableIndex? issuerCodeTableIndex,
        ApplicationPreferredName? applicationPreferredName)
    {
        _FileControlInformationIssuerDiscretionaryDataAdf = fileControlInformationIssuerDiscretionaryData;
        _ApplicationLabel = applicationLabel;
        _ApplicationPriorityIndicator = applicationPriorityIndicator;
        _ProcessingOptionsDataObjectList = processingOptionsDataObjectList;
        _LanguagePreference = languagePreference;
        _IssuerCodeTableIndex = issuerCodeTableIndex;
        _ApplicationPreferredName = applicationPreferredName;
    }

    #endregion

    #region Instance Members

    public override FileControlInformationIssuerDiscretionaryDataAdf GetFileControlInformationIssuerDiscretionaryData() =>
        _FileControlInformationIssuerDiscretionaryDataAdf;

    public override Tag GetTag() => Tag;

    public bool TryGetApplicationCapabilitiesInformation(out ApplicationCapabilitiesInformation? result) =>
        _FileControlInformationIssuerDiscretionaryDataAdf.TryGetApplicationCapabilitiesInformation(out result);

    public Tag[] GetRequestedDataObjects() =>
        _ProcessingOptionsDataObjectList?.GetRequestedItems().Select(a => a.GetTag()).ToArray() ?? Array.Empty<Tag>();

    public bool TryGetProcessingOptionsDataObjectList(out ProcessingOptionsDataObjectList? result)
    {
        if (_ProcessingOptionsDataObjectList is null)
        {
            result = default;

            return false;
        }

        result = _ProcessingOptionsDataObjectList;

        return true;
    }

    public bool TryGetProcessingOptionsRelatedData(IReadTlvDatabase database, out CommandTemplate? result)
    {
        if (_ProcessingOptionsDataObjectList is null)
        {
            result = default;

            return false;
        }

        result = _ProcessingOptionsDataObjectList.AsCommandTemplate(database);

        return true;
    }

    // BUG: I'm pretty sure this needs to be wrapped in a PDOL Related Data object
    public CommandTemplate GetProcessingOptionsRelatedData(IReadTlvDatabase database) =>
        _ProcessingOptionsDataObjectList!.AsCommandTemplate(database);

    public bool TryGetLanguagePreference(out LanguagePreference? result)
    {
        if (_LanguagePreference != null)
        {
            result = _LanguagePreference;

            return true;
        }

        result = default;

        return false;
    }

    public override Tag[] GetChildTags()
    {
        return new[]
        {
            ApplicationLabel.Tag, ApplicationPreferredName.Tag, ApplicationPriorityIndicator.Tag,
            FileControlInformationIssuerDiscretionaryDataAdf.Tag, IssuerCodeTableIndex.Tag, LanguagePreference.Tag,
            ProcessingOptionsDataObjectList.Tag
        };
    }

    public bool IsDataObjectRequested(Tag tag) => _ProcessingOptionsDataObjectList?.Contains(tag) ?? false;

    protected override IEncodeBerDataObjects?[] GetChildren()
    {
        return new IEncodeBerDataObjects?[]
        {
            _ApplicationLabel, _ApplicationPreferredName, _ApplicationPriorityIndicator,
            _FileControlInformationIssuerDiscretionaryDataAdf, _IssuerCodeTableIndex, _LanguagePreference,
            _ProcessingOptionsDataObjectList
        };
    }

    #endregion

    #region Serialization

    public static FileControlInformationProprietaryAdf Decode(ReadOnlyMemory<byte> rawBer) => Decode(_Codec.DecodeChildren(rawBer));

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="CardDataMissingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static FileControlInformationProprietaryAdf Decode(EncodedTlvSiblings encodedChildren)
    {
        FileControlInformationIssuerDiscretionaryDataAdf fciProprietaryTemplate =
            _Codec.AsConstructed(FileControlInformationIssuerDiscretionaryDataAdf.Decode,
                                 FileControlInformationIssuerDiscretionaryDataAdf.Tag, encodedChildren)
            ?? throw new
                CardDataMissingException($"A problem occurred while decoding {nameof(FileControlInformationIssuerDiscretionaryDataAdf)}. A {nameof(FileControlInformationIssuerDiscretionaryDataAdf)} was expected but could not be found");

        ApplicationLabel? applicationLabel = null;
        ApplicationPreferredName? applicationPreferredName = null;
        IssuerCodeTableIndex? issuerCodeTableIndex = null;
        LanguagePreference? languagePreference = null;
        ApplicationPriorityIndicator? applicationPriorityIndicator =
            _Codec.AsPrimitive(ApplicationPriorityIndicator.Decode, ApplicationPriorityIndicator.Tag, encodedChildren);
        ProcessingOptionsDataObjectList? processingOptionsDataObjectList = _Codec.AsPrimitive(ProcessingOptionsDataObjectList.Decode,
         ProcessingOptionsDataObjectList.Tag, encodedChildren);

        // EMV Book 1 Section 12.2.4 tells us not to enforce encoding errors for the following data elements.
        // Instead, we treat it as if it wasn't returned from the ICC at all
        try
        {
            applicationLabel = _Codec.AsPrimitive(ApplicationLabel.Decode, ApplicationLabel.Tag, encodedChildren);
            applicationPreferredName = _Codec.AsPrimitive(ApplicationPreferredName.Decode, ApplicationPreferredName.Tag, encodedChildren);
            issuerCodeTableIndex = _Codec.AsPrimitive(IssuerCodeTableIndex.Decode, IssuerCodeTableIndex.Tag, encodedChildren);
            languagePreference = _Codec.AsPrimitive(LanguagePreference.Decode, LanguagePreference.Tag, encodedChildren);
        }
        catch (BerParsingException)
        {
            // TODO: Logging
        }
        catch (CodecParsingException)
        {
            // TODO: Logging
        }
        catch (Exception)
        {
            // TODO: Logging
        }

        return new FileControlInformationProprietaryAdf(fciProprietaryTemplate, applicationLabel, applicationPriorityIndicator,
                                                        processingOptionsDataObjectList, languagePreference, issuerCodeTableIndex,
                                                        applicationPreferredName);
    }

    #endregion

    #region Equality

    public override bool Equals(ConstructedValue? other) =>
        other is FileControlInformationProprietaryAdf fileControlInformationProprietaryTemplate
        && Equals(fileControlInformationProprietaryTemplate);

    public bool Equals(FileControlInformationProprietaryAdf? other)
    {
        if (other == null)
            return false;

        if (!ApplicationPreferredName.StaticEquals(_ApplicationPreferredName, other._ApplicationPreferredName))
            return false;
        if (!ApplicationPriorityIndicator.StaticEquals(_ApplicationPriorityIndicator, other._ApplicationPriorityIndicator))
            return false;
        if (!IssuerCodeTableIndex.StaticEquals(_IssuerCodeTableIndex, other._IssuerCodeTableIndex))
            return false;
        if (!LanguagePreference.StaticEquals(_LanguagePreference, other._LanguagePreference))
            return false;
        if (!ProcessingOptionsDataObjectList.StaticEquals(_ProcessingOptionsDataObjectList, other._ProcessingOptionsDataObjectList))
            return false;

        return (other.GetTag() == Tag)
            && _ApplicationLabel.Equals(other._ApplicationLabel)
            && _FileControlInformationIssuerDiscretionaryDataAdf.Equals(other._FileControlInformationIssuerDiscretionaryDataAdf);
    }

    public override bool Equals(ConstructedValue? x, ConstructedValue? y) =>
        x is FileControlInformationProprietaryAdf x1 && y is FileControlInformationProprietaryAdf y1 && x1.Equals(y1);

    public bool Equals(FileControlInformationProprietaryAdf? x, FileControlInformationProprietaryAdf? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override bool Equals(object? obj) =>
        obj is FileControlInformationProprietaryAdf fileControlInformationProprietaryTemplate
        && Equals(fileControlInformationProprietaryTemplate);

    public override int GetHashCode(ConstructedValue obj) => obj.GetHashCode();
    public int GetHashCode(FileControlInformationProprietaryTemplate obj) => obj.GetHashCode();

    public override int GetHashCode()
    {
        const int hash = 164267;

        unchecked
        {
            int result = GetTag().GetHashCode() * hash;

            return (_ApplicationLabel.GetHashCode()
                    + _FileControlInformationIssuerDiscretionaryDataAdf.GetHashCode()
                    + _ApplicationPreferredName?.GetHashCode())
                ?? (0 + _ApplicationPriorityIndicator?.GetHashCode())
                ?? (0 + _IssuerCodeTableIndex?.GetHashCode())
                ?? (0 + _LanguagePreference?.GetHashCode()) ?? (0 + _ProcessingOptionsDataObjectList?.GetHashCode()) ?? 0;
        }
    }

    #endregion
}