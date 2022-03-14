using System;
using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.Exceptions;

namespace Play.Emv.Templates;

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

    public override Tag[] GetChildTags() => ChildTags;

    public CommandTemplate AsCommandTemplate(PoiInformation poiInformation, TagLengthValue[] selectionDataObjectListValues) =>
        _FileControlInformationIssuerDiscretionaryDataPpse.AsCommandTemplate(_Codec, poiInformation, selectionDataObjectListValues);

    public CommandTemplate AsCommandTemplate(IQueryTlvDatabase database) =>
        _FileControlInformationIssuerDiscretionaryDataPpse.AsCommandTemplate(database);

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

    #region Serialization

    public static FileControlInformationProprietaryPpse Decode(ReadOnlyMemory<byte> value) => Decode(_Codec.DecodeChildren(value));

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Exceptions._Temp.BerFormatException"></exception>
    /// <exception cref="CardDataMissingException"></exception>
    public static FileControlInformationProprietaryPpse Decode(EncodedTlvSiblings encodedTlvSiblings)
    {
        FileControlInformationIssuerDiscretionaryDataPpse fciProprietary =
            _Codec.AsConstructed(FileControlInformationIssuerDiscretionaryDataPpse.Decode,
                                 FileControlInformationIssuerDiscretionaryDataTemplate.Tag, encodedTlvSiblings)
            ?? throw new
                CardDataMissingException($"A problem occurred while decoding {nameof(FileControlInformationIssuerDiscretionaryDataPpse)}. A {nameof(FileControlInformationIssuerDiscretionaryDataPpse)} was expected but could not be found");

        return new FileControlInformationProprietaryPpse(fciProprietary);
    }

    #endregion

    #region Equality

    public override bool Equals(ConstructedValue? other) =>
        other is FileControlInformationProprietaryPpse fileControlInformationProprietaryTemplate
        && Equals(fileControlInformationProprietaryTemplate);

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

    public override bool Equals(object? obj) =>
        obj is FileControlInformationProprietaryPpse fileControlInformationProprietaryTemplate
        && Equals(fileControlInformationProprietaryTemplate);

    public override int GetHashCode(ConstructedValue obj) => obj.GetHashCode();
    public int GetHashCode(FileControlInformationProprietaryPpse obj) => obj.GetHashCode();
    public override int GetHashCode() => _FileControlInformationIssuerDiscretionaryDataPpse.GetHashCode();

    #endregion
}