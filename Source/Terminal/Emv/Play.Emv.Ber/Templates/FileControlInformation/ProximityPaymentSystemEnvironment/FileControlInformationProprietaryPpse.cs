using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.InternalFactories;
using Play.Ber.Tags;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.Templates;

public record FileControlInformationProprietaryPpse : FileControlInformationProprietaryTemplate
{
    #region Static Metadata

    public static Tag[] ChildTags = {FileControlInformationIssuerDiscretionaryDataTemplate.Tag};

    #endregion

    #region Instance Values

    private readonly FileControlInformationIssuerDiscretionaryDataPpse _FileControlInformationIssuerDiscretionaryDataPpse;

    #endregion

    #region Constructor

    public FileControlInformationProprietaryPpse(FileControlInformationIssuerDiscretionaryDataPpse fileControlInformationProprietaryDataPpse)
    {
        _FileControlInformationIssuerDiscretionaryDataPpse = fileControlInformationProprietaryDataPpse;
    }

    #endregion

    #region Serialization

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CardDataMissingException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static FileControlInformationProprietaryPpse Decode(ReadOnlyMemory<byte> value)
    {
        if (_Codec.DecodeFirstTag(value.Span) == Tag)
            return Decode(_Codec.DecodeChildren(value));

        return Decode(_Codec.DecodeSiblings(value));
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exceptions._Temp.BerFormatException"></exception>
    /// <exception cref="CardDataMissingException"></exception>
    public static FileControlInformationProprietaryPpse Decode(EncodedTlvSiblings encodedTlvSiblings)
    {
        FileControlInformationIssuerDiscretionaryDataPpse fciProprietary =
            _Codec.AsConstructed(FileControlInformationIssuerDiscretionaryDataPpse.Decode, FileControlInformationIssuerDiscretionaryDataTemplate.Tag,
                encodedTlvSiblings)
            ?? throw new CardDataMissingException(
                $"A problem occurred while decoding {nameof(FileControlInformationIssuerDiscretionaryDataPpse)}. A {nameof(FileControlInformationIssuerDiscretionaryDataPpse)} was expected but could not be found");

        return new FileControlInformationProprietaryPpse(fciProprietary);
    }

    #endregion

    #region Instance Members

    public override Tag[] GetChildTags() => ChildTags;

    public CommandTemplate AsCommandTemplate(PoiInformation poiInformation, IReadTlvDatabase database) =>
        _FileControlInformationIssuerDiscretionaryDataPpse.AsCommandTemplate(_Codec, poiInformation, database);

    public CommandTemplate AsCommandTemplate(IReadTlvDatabase database) => _FileControlInformationIssuerDiscretionaryDataPpse.AsCommandTemplate(database);

    public ApplicationDedicatedFileName[] GetApplicationDedicatedFileNames() =>
        _FileControlInformationIssuerDiscretionaryDataPpse.GetApplicationDedicatedFileNames();

    public TagLength[] GetDataObjectsRequestedByCard() => _FileControlInformationIssuerDiscretionaryDataPpse.GetRequestedSdolItems();
    public List<DirectoryEntry> GetDirectoryEntries() => _FileControlInformationIssuerDiscretionaryDataPpse.GetDirectoryEntries();

    public override FileControlInformationIssuerDiscretionaryDataPpse GetFileControlInformationIssuerDiscretionaryData() =>
        _FileControlInformationIssuerDiscretionaryDataPpse;

    public override Tag GetTag() => 0xA5;
    public bool IsDirectoryEntryListEmpty() => _FileControlInformationIssuerDiscretionaryDataPpse.IsDirectoryEntryListEmpty();

    protected override IEncodeBerDataObjects?[] GetChildren()
    {
        return new IEncodeBerDataObjects?[] {_FileControlInformationIssuerDiscretionaryDataPpse};
    }

    #endregion
}