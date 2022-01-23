using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Emv.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.DataElements;
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

    public override Tag[] GetChildTags()
    {
        return ChildTags;
    }

    public CommandTemplate AsCommandTemplate(BerCodec codec, PoiInformation poiInformation, TagLengthValue[] selectionDataObjectListValues)
    {
        return _FileControlInformationProprietaryPpse.AsCommandTemplate(poiInformation, selectionDataObjectListValues);
    }

    public CommandTemplate AsCommandTemplate(IQueryTlvDatabase database)
    {
        return _FileControlInformationProprietaryPpse.AsCommandTemplate(database);
    }

    public ApplicationDedicatedFileName[] GetApplicationDedicatedFileNames()
    {
        return _FileControlInformationProprietaryPpse.GetApplicationDedicatedFileNames();
    }

    public TagLength[] GetDataObjectsRequestedByCard()
    {
        return _FileControlInformationProprietaryPpse.GetDataObjectsRequestedByCard();
    }

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

    public List<DirectoryEntry> GetDirectoryEntries()
    {
        return _FileControlInformationProprietaryPpse.GetDirectoryEntries();
    }

    public override FileControlInformationProprietaryPpse GetFileControlInformationProprietary()
    {
        return _FileControlInformationProprietaryPpse;
    }

    public override Tag GetTag()
    {
        return Tag;
    }

    public bool IsDirectoryEntryListEmpty()
    {
        return _FileControlInformationProprietaryPpse.IsDirectoryEntryListEmpty();
    }

    public bool IsPointOfInteractionApduCommandRequested()
    {
        return _FileControlInformationProprietaryPpse.GetFileControlInformationIssuerDiscretionaryData()
            .IsPointOfInteractionApduCommandRequested();
    }

    protected override IEncodeBerDataObjects?[] GetChildren()
    {
        return new IEncodeBerDataObjects?[] {_DedicatedFileName, _FileControlInformationProprietaryPpse};
    }

    #endregion

    #region Serialization

    public static FileControlInformationPpse Decode(ReadOnlyMemory<byte> rawBer)
    {
        return Decode(_Codec.DecodeChildren(rawBer));
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    /// <exception cref="BerInternalException"></exception>
    private static FileControlInformationPpse Decode(EncodedTlvSiblings encodedTlvSiblings)
    {
        DedicatedFileName? dedicatedFileName = _Codec.AsPrimitive(DedicatedFileName.Decode, DedicatedFileName.Tag, encodedTlvSiblings);

        FileControlInformationProprietaryPpse fciProprietaryPpse =
            _Codec.AsConstructed(FileControlInformationProprietaryPpse.Decode, FileControlInformationProprietaryTemplate.Tag,
                                 encodedTlvSiblings)
            ?? throw new
                InvalidOperationException($"A problem occurred while decoding {nameof(FileControlInformationPpse)}. A {nameof(FileControlInformationProprietaryPpse)} was expected but could not be found");

        return new FileControlInformationPpse(dedicatedFileName, fciProprietaryPpse);
    }

    #endregion

    #region Equality

    public override bool Equals(object? obj)
    {
        return obj is FileControlInformationPpse fci && Equals(fci);
    }

    public override bool Equals(ConstructedValue? other)
    {
        return other is FileControlInformationPpse ppse && Equals(ppse);
    }

    public bool Equals(FileControlInformationPpse other)
    {
        return (_DedicatedFileName != null)
            && _DedicatedFileName.Equals(other._DedicatedFileName)
            && _FileControlInformationProprietaryPpse.Equals(other._FileControlInformationProprietaryPpse);
    }

    public override bool Equals(ConstructedValue? x, ConstructedValue? y)
    {
        if (x == null)
            return y == null;

        if (y == null)
            return false;

        return x.Equals(y);
    }

    public bool Equals(FileControlInformationPpse x, FileControlInformationPpse y)
    {
        return x.Equals(y);
    }

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

    public override int GetHashCode(ConstructedValue obj)
    {
        return obj.GetHashCode();
    }

    #endregion
}