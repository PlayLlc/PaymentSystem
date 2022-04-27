using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.Templates;

public record FileControlInformationProprietaryAdf : FileControlInformationProprietaryTemplate
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
        FileControlInformationIssuerDiscretionaryDataAdf fileControlInformationIssuerDiscretionaryData, ApplicationLabel? applicationLabel,
        ApplicationPriorityIndicator? applicationPriorityIndicator, ProcessingOptionsDataObjectList? processingOptionsDataObjectList,
        LanguagePreference? languagePreference, IssuerCodeTableIndex? issuerCodeTableIndex, ApplicationPreferredName? applicationPreferredName)
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

    public Tag[] GetRequestedDataObjects() => _ProcessingOptionsDataObjectList?.GetRequestedItems().Select(a => a.GetTag()).ToArray() ?? Array.Empty<Tag>();

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

    /// <summary>
    ///     TryGetProcessingOptionsRelatedData
    /// </summary>
    /// <param name="database"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="BerParsingException"></exception>
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
    public CommandTemplate GetProcessingOptionsRelatedData(IReadTlvDatabase database) => _ProcessingOptionsDataObjectList!.AsCommandTemplate(database);

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
            ApplicationLabel.Tag, ApplicationPreferredName.Tag, ApplicationPriorityIndicator.Tag, FileControlInformationIssuerDiscretionaryDataAdf.Tag,
            IssuerCodeTableIndex.Tag, LanguagePreference.Tag, ProcessingOptionsDataObjectList.Tag
        };
    }

    public bool IsDataObjectRequested(Tag tag) => _ProcessingOptionsDataObjectList?.Exists(tag) ?? false;

    protected override IEncodeBerDataObjects?[] GetChildren()
    {
        return new IEncodeBerDataObjects?[]
        {
            _ApplicationLabel, _ApplicationPreferredName, _ApplicationPriorityIndicator, _FileControlInformationIssuerDiscretionaryDataAdf,
            _IssuerCodeTableIndex, _LanguagePreference, _ProcessingOptionsDataObjectList
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
            _Codec.AsConstructed(FileControlInformationIssuerDiscretionaryDataAdf.Decode, FileControlInformationIssuerDiscretionaryDataAdf.Tag, encodedChildren)
            ?? throw new CardDataMissingException(
                $"A problem occurred while decoding {nameof(FileControlInformationIssuerDiscretionaryDataAdf)}. A {nameof(FileControlInformationIssuerDiscretionaryDataAdf)} was expected but could not be found");

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

        return new FileControlInformationProprietaryAdf(fciProprietaryTemplate, applicationLabel, applicationPriorityIndicator, processingOptionsDataObjectList,
            languagePreference, issuerCodeTableIndex, applicationPreferredName);
    }

    #endregion
}