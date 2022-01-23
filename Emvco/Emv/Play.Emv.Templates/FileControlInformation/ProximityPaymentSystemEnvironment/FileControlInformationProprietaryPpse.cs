﻿using Play.Ber.DataObjects;
using Play.Ber.Emv.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.DataElements;

namespace Play.Emv.Templates.FileControlInformation;

public class FileControlInformationProprietaryPpse : FileControlInformationProprietaryTemplate
{
    #region Static Metadata

    public static Tag[] ChildTags = {FileControlInformationIssuerDiscretionaryDataTemplate.Tag};

    #endregion

    #region Instance Values

    private readonly FileControlInformationIssuerDiscretionaryDataPpse _FileControlInformationIssuerDiscretionaryDataPpse;

    #endregion

    #region Constructor

    public FileControlInformationProprietaryPpse(
        FileControlInformationIssuerDiscretionaryDataPpse fileControlInformationProprietaryDataPpse)
    {
        _FileControlInformationIssuerDiscretionaryDataPpse = fileControlInformationProprietaryDataPpse;
    }

    #endregion

    #region Instance Members

    public override Tag[] GetChildTags()
    {
        return ChildTags;
    }

    public CommandTemplate AsCommandTemplate(PoiInformation poiInformation, TagLengthValue[] selectionDataObjectListValues)
    {
        return _FileControlInformationIssuerDiscretionaryDataPpse.AsCommandTemplate(_Codec, poiInformation, selectionDataObjectListValues);
    }

    public CommandTemplate AsCommandTemplate(IQueryTlvDatabase database)
    {
        return _FileControlInformationIssuerDiscretionaryDataPpse.AsCommandTemplate(database);
    }

    public ApplicationDedicatedFileName[] GetApplicationDedicatedFileNames()
    {
        return _FileControlInformationIssuerDiscretionaryDataPpse.GetApplicationDedicatedFileNames();
    }

    public TagLength[] GetDataObjectsRequestedByCard()
    {
        return _FileControlInformationIssuerDiscretionaryDataPpse.GetRequestedSdolItems();
    }

    public List<DirectoryEntry> GetDirectoryEntries()
    {
        return _FileControlInformationIssuerDiscretionaryDataPpse.GetDirectoryEntries();
    }

    public override FileControlInformationIssuerDiscretionaryDataPpse GetFileControlInformationIssuerDiscretionaryData()
    {
        return _FileControlInformationIssuerDiscretionaryDataPpse;
    }

    public override Tag GetTag()
    {
        return 0xA5;
    }

    public bool IsDirectoryEntryListEmpty()
    {
        return _FileControlInformationIssuerDiscretionaryDataPpse.IsDirectoryEntryListEmpty();
    }

    protected override IEncodeBerDataObjects?[] GetChildren()
    {
        return new IEncodeBerDataObjects?[] {_FileControlInformationIssuerDiscretionaryDataPpse};
    }

    #endregion

    #region Serialization

    public static FileControlInformationProprietaryPpse Decode(ReadOnlyMemory<byte> value)
    {
        return Decode(_Codec.DecodeChildren(value));
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    /// <exception cref="BerInternalException"></exception>
    public static FileControlInformationProprietaryPpse Decode(EncodedTlvSiblings encodedTlvSiblings)
    {
        FileControlInformationIssuerDiscretionaryDataPpse fciProprietary =
            _Codec.AsConstructed(FileControlInformationIssuerDiscretionaryDataPpse.Decode,
                                 FileControlInformationIssuerDiscretionaryDataTemplate.Tag, encodedTlvSiblings)
            ?? throw new
                InvalidOperationException($"A problem occurred while decoding {nameof(FileControlInformationIssuerDiscretionaryDataPpse)}. A {nameof(FileControlInformationIssuerDiscretionaryDataPpse)} was expected but could not be found");

        return new FileControlInformationProprietaryPpse(fciProprietary);
    }

    #endregion

    #region Equality

    public override bool Equals(ConstructedValue? other)
    {
        return other is FileControlInformationProprietaryPpse fileControlInformationProprietaryTemplate
            && Equals(fileControlInformationProprietaryTemplate);
    }

    public bool Equals(FileControlInformationProprietaryPpse? other)
    {
        if (other == null)
            return false;

        return (other.GetTag() == Tag)
            && _FileControlInformationIssuerDiscretionaryDataPpse.Equals(other._FileControlInformationIssuerDiscretionaryDataPpse);
    }

    public override bool Equals(ConstructedValue? x, ConstructedValue? y)
    {
        if (x == null)
            return y == null;

        return (y != null) && x.Equals(y);
    }

    public bool Equals(FileControlInformationProprietaryPpse? x, FileControlInformationProprietaryPpse? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override bool Equals(object? obj)
    {
        return obj is FileControlInformationProprietaryPpse fileControlInformationProprietaryTemplate
            && Equals(fileControlInformationProprietaryTemplate);
    }

    public override int GetHashCode(ConstructedValue obj)
    {
        return obj.GetHashCode();
    }

    public int GetHashCode(FileControlInformationProprietaryPpse obj)
    {
        return obj.GetHashCode();
    }

    public override int GetHashCode()
    {
        return _FileControlInformationIssuerDiscretionaryDataPpse.GetHashCode();
    }

    #endregion
}