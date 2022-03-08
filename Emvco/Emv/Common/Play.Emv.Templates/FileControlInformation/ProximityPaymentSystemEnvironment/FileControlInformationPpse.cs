using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.DataObjects;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Templates.FileControlInformation;

public class FileControlInformationPpse : FileControlInformationTemplate
{
    #region Static Metadata

    public static readonly Tag[] ChildTags = {DedicatedFileName.Tag, FileControlInformationProprietaryTemplate.Tag};

    #endregion

    #region Instance Values

    private readonly DedicatedFileName? _DedicatedFileName;
    private readonly FileControlInformationProprietaryPpse _FileControlInformationProprietaryPpse;

    #endregion

    #region Constructor

    public FileControlInformationPpse(
        DedicatedFileName? dedicatedFileName,
        FileControlInformationProprietaryPpse fileControlInformationProprietaryPpsePpse)
    {
        _DedicatedFileName = dedicatedFileName;
        _FileControlInformationProprietaryPpse = fileControlInformationProprietaryPpsePpse;
    }

    #endregion

    #region Instance Members

    public override Tag[] GetChildTags() => ChildTags;

    public CommandTemplate
        AsCommandTemplate(BerCodec codec, PoiInformation poiInformation, TagLengthValue[] selectionDataObjectListValues) =>
        _FileControlInformationProprietaryPpse.AsCommandTemplate(poiInformation, selectionDataObjectListValues);

    public CommandTemplate AsCommandTemplate(IQueryTlvDatabase database) =>
        _FileControlInformationProprietaryPpse.AsCommandTemplate(database);

    public ApplicationDedicatedFileName[] GetApplicationDedicatedFileNames() =>
        _FileControlInformationProprietaryPpse.GetApplicationDedicatedFileNames();

    public TagLength[] GetDataObjectsRequestedByCard() => _FileControlInformationProprietaryPpse.GetDataObjectsRequestedByCard();

    public bool TryGetDedicatedFileName(out DedicatedFileName? result)
    {
        if (_DedicatedFileName is null)
        {
            result = null;

            return false;
        }

        result = _DedicatedFileName;

        return true;
    }

    public List<DirectoryEntry> GetDirectoryEntries() => _FileControlInformationProprietaryPpse.GetDirectoryEntries();
    public override FileControlInformationProprietaryPpse GetFileControlInformationProprietary() => _FileControlInformationProprietaryPpse;
    public override Tag GetTag() => Tag;
    public bool IsDirectoryEntryListEmpty() => _FileControlInformationProprietaryPpse.IsDirectoryEntryListEmpty();

    public bool IsPointOfInteractionApduCommandRequested() =>
        _FileControlInformationProprietaryPpse.GetFileControlInformationIssuerDiscretionaryData()
            .IsPointOfInteractionApduCommandRequested();

    protected override IEncodeBerDataObjects?[] GetChildren()
    {
        return new IEncodeBerDataObjects?[] {_DedicatedFileName, _FileControlInformationProprietaryPpse};
    }

    #endregion

    #region Serialization

    public static FileControlInformationPpse Decode(ReadOnlyMemory<byte> rawBer) => Decode(_Codec.DecodeChildren(rawBer));

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    /// <exception cref="BerInternalException"></exception>
    private static FileControlInformationPpse Decode(EncodedTlvSiblings encodedTlvSiblings)
    {
        DedicatedFileName? dedicatedFileName = _Codec.AsPrimitive(DedicatedFileName.Decode, DedicatedFileName.Tag, encodedTlvSiblings);

        FileControlInformationProprietaryPpse fciProprietaryPpse =
            _Codec.AsConstructed(FileControlInformationProprietaryPpse.Decode, FileControlInformationProprietaryTemplate.Tag,
                encodedTlvSiblings)
            ?? throw new InvalidOperationException(
                $"A problem occurred while decoding {nameof(FileControlInformationPpse)}. A {nameof(FileControlInformationProprietaryPpse)} was expected but could not be found");

        return new FileControlInformationPpse(dedicatedFileName, fciProprietaryPpse);
    }

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is FileControlInformationPpse fci && Equals(fci);
    public override bool Equals(ConstructedValue? other) => other is FileControlInformationPpse ppse && Equals(ppse);

    public bool Equals(FileControlInformationPpse other) =>
        (_DedicatedFileName != null)
        && _DedicatedFileName.Equals(other._DedicatedFileName)
        && _FileControlInformationProprietaryPpse.Equals(other._FileControlInformationProprietaryPpse);

    public override bool Equals(ConstructedValue? x, ConstructedValue? y)
    {
        if (x == null)
            return y == null;

        if (y == null)
            return false;

        return x.Equals(y);
    }

    public bool Equals(FileControlInformationPpse x, FileControlInformationPpse y) => x.Equals(y);

    public override int GetHashCode()
    {
        const int hash = 7159;

        unchecked
        {
            int result = GetTag().GetHashCode() * hash;
            result += hash * _FileControlInformationProprietaryPpse.GetHashCode();
            result += (hash * _DedicatedFileName?.GetHashCode()) ?? 0;

            return result;
        }
    }

    public override int GetHashCode(ConstructedValue obj) => obj.GetHashCode();

    #endregion
}