#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.Exceptions;

namespace Play.Emv.Templates;

public class FileControlInformationIssuerDiscretionaryDataPpse : FileControlInformationIssuerDiscretionaryDataTemplate
{
    #region Static Metadata

    public static readonly Tag[] ChildTags = {DirectoryEntry.Tag, SelectionDataObjectList.Tag, TerminalCategoriesSupportedList.Tag};

    #endregion

    #region Instance Values

    private readonly SetOf<DirectoryEntry> _DirectoryEntry;

    /// <summary>
    ///     If the <see cref="SelectionDataObjectList" /> is not null, then it means the PICC is issuing a
    ///     <see cref="SendPoiInformationCommand" />. The PICC will tailor the available Applications based
    ///     on the information provided
    /// </summary>
    /// <remarks>
    ///     The <see cref="SendPoiInformationCommand" /> is optional and is a Terminal implementation decision
    /// </remarks>
    private readonly SelectionDataObjectList? _SelectionDataObjectList;

    /// <summary>
    ///     TerminalCategoryCodef="TerminalCategoryCode.TransitGate" /> is returned as part of
    ///     <see cref="TerminalCategoriesSupportedList" /> then it means the PICC is issuing a
    ///     <see cref="SendPoiInformationCommand" />. The PICC will tailor the available Applications based
    ///     on the information provided
    /// </summary>
    /// <remarks>
    ///     The <see cref="SendPoiInformationCommand" /> is optional and is a Terminal implementation decision
    /// </remarks>
    private readonly TerminalCategoriesSupportedList? _TerminalCategoriesSupportedList;

    #endregion

    #region Constructor

    // SelectionDataObjectList 
    public FileControlInformationIssuerDiscretionaryDataPpse(
        SetOf<DirectoryEntry> directoryEntries,
        TerminalCategoriesSupportedList? terminalCategoriesSupportedList,
        SelectionDataObjectList? selectionDataObjectList)
    {
        _DirectoryEntry = directoryEntries;
        _TerminalCategoriesSupportedList = terminalCategoriesSupportedList;
        _SelectionDataObjectList = selectionDataObjectList;
    }

    #endregion

    #region Instance Members

    public override Tag[] GetChildTags() => ChildTags;

    public CommandTemplate AsCommandTemplate(BerCodec codec, PoiInformation poiInformation, TagLengthValue[] selectionDataObjectListValues)
    {
        if ((_SelectionDataObjectList != null) && (!_TerminalCategoriesSupportedList?.IsPointOfInteractionApduCommandRequested() ?? false))
            return _SelectionDataObjectList.AsCommandTemplate(selectionDataObjectListValues);

        if (_TerminalCategoriesSupportedList?.IsPointOfInteractionApduCommandRequested() ?? false)
            return new CommandTemplate(poiInformation.EncodeValue(codec));

        if ((_SelectionDataObjectList != null) && (_TerminalCategoriesSupportedList?.IsPointOfInteractionApduCommandRequested() ?? false))
        {
            Span<byte> dolEncoding = _SelectionDataObjectList.AsCommandTemplate(selectionDataObjectListValues).GetValueAsByteArray();
            Span<byte> terminalCategoryEncoding = poiInformation.EncodeValue(codec);

            return new CommandTemplate(dolEncoding.ConcatArrays(terminalCategoryEncoding));
        }

        return new CommandTemplate(Array.Empty<byte>());
    }

    /// <summary>
    ///     AsCommandTemplate
    /// </summary>
    /// <param name="database"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public CommandTemplate AsCommandTemplate(IQueryTlvDatabase database)
    {
        if ((_SelectionDataObjectList != null) && (!_TerminalCategoriesSupportedList?.IsPointOfInteractionApduCommandRequested() ?? false))
            return _SelectionDataObjectList.AsCommandTemplate(database);

        if (database.IsPresentAndNotEmpty(PoiInformation.Tag)
            && (_TerminalCategoriesSupportedList?.IsPointOfInteractionApduCommandRequested() ?? false))
            return new CommandTemplate(database.Get(PoiInformation.Tag).EncodeValue());

        if ((_SelectionDataObjectList != null)
            && database.IsPresentAndNotEmpty(PoiInformation.Tag)
            && (_TerminalCategoriesSupportedList?.IsPointOfInteractionApduCommandRequested() ?? false))
        {
            Span<byte> dolEncoding = _SelectionDataObjectList.AsCommandTemplate(database).GetValueAsByteArray();
            Span<byte> terminalCategoryEncoding = database.Get(PoiInformation.Tag).EncodeValue();

            return new CommandTemplate(dolEncoding.ConcatArrays(terminalCategoryEncoding));
        }

        return new CommandTemplate(Array.Empty<byte>());
    }

    public ApplicationDedicatedFileName[] GetApplicationDedicatedFileNames()
    {
        return _DirectoryEntry.Select(a => a.GetApplicationDedicatedFileName()).ToArray();
    }

    public List<DirectoryEntry> GetDirectoryEntries() => _DirectoryEntry.ToList();
    public TagLength[] GetRequestedSdolItems() => _SelectionDataObjectList?.GetRequestedItems() ?? Array.Empty<TagLength>();
    public override Tag GetTag() => Tag;
    public bool IsDirectoryEntryListEmpty() => GetDirectoryEntries().Count == 0;

    public bool IsPointOfInteractionApduCommandRequested()
    {
        if (_SelectionDataObjectList != null)
            return true;

        if (_TerminalCategoriesSupportedList == null)
            return false;

        return _TerminalCategoriesSupportedList.IsPointOfInteractionApduCommandRequested();
    }

    protected override IEncodeBerDataObjects?[] GetChildren()
    {
        return new IEncodeBerDataObjects?[] {_DirectoryEntry, _TerminalCategoriesSupportedList, _SelectionDataObjectList};
    }

    #endregion

    #region Serialization

    public static FileControlInformationIssuerDiscretionaryDataPpse Decode(ReadOnlyMemory<byte> value) =>
        Decode(_Codec.DecodeChildren(value));

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static FileControlInformationIssuerDiscretionaryDataPpse Decode(EncodedTlvSiblings encodedTlvSiblings)
    {
        TerminalCategoriesSupportedList? terminalCategoriesSupportedList = default;
        SelectionDataObjectList? selectionDataObjectList = default;

        if (!encodedTlvSiblings.TryGetRawSetOf(DirectoryEntry.Tag, out Span<ReadOnlyMemory<byte>> sequenceOfResult))
            throw new CardDataMissingException(
                $"A problem occurred while decoding {nameof(FileControlInformationIssuerDiscretionaryDataDdf)}. A Set of {nameof(DirectoryEntry)} objects were expected but could not be found");

        SetOf<DirectoryEntry> directoryEntry = new(sequenceOfResult.ToArray().Select(DirectoryEntry.Decode).ToArray());

        if (encodedTlvSiblings.TryGetValueOctetsOfChild(TerminalCategoriesSupportedList.Tag,
            out ReadOnlyMemory<byte> rawTerminalCategoriesSupportedList))
        {
            terminalCategoriesSupportedList =
                (_Codec.Decode(TerminalCategoriesSupportedList.EncodingId, rawTerminalCategoriesSupportedList.Span) as
                    DecodedResult<TerminalCategoriesSupportedList>)!.Value;
        }

        if (encodedTlvSiblings.TryGetValueOctetsOfChild(SelectionDataObjectList.Tag, out ReadOnlyMemory<byte> rawSelectionDataObjectList))
        {
            selectionDataObjectList =
                ((DecodedResult<SelectionDataObjectList>) _Codec.Decode(SelectionDataObjectList.EncodingId,
                    rawSelectionDataObjectList.Span)).Value;
        }

        return new FileControlInformationIssuerDiscretionaryDataPpse(directoryEntry, terminalCategoriesSupportedList,
            selectionDataObjectList);
    }

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is FileControlInformationIssuerDiscretionaryDataPpse fci && Equals(fci);
    public override bool Equals(ConstructedValue? other) => other is FileControlInformationIssuerDiscretionaryDataPpse ppse && Equals(ppse);

    public bool Equals(FileControlInformationIssuerDiscretionaryDataPpse other)
    {
        if (_DirectoryEntry.Count != other._DirectoryEntry.Count)
            return false;

        if (!_TerminalCategoriesSupportedList?.Equals(other._TerminalCategoriesSupportedList)
            ?? (other._TerminalCategoriesSupportedList == null))
            return false;

        return _DirectoryEntry.All(other._DirectoryEntry.Contains);
    }

    public override bool Equals(ConstructedValue? x, ConstructedValue? y) =>
        Equals((FileControlInformationIssuerDiscretionaryDataPpse?) x, (FileControlInformationIssuerDiscretionaryDataPpse?) y);

    public static bool Equals(FileControlInformationIssuerDiscretionaryDataPpse? x, FileControlInformationIssuerDiscretionaryDataPpse? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override int GetHashCode()
    {
        const int hash = 738459837;

        unchecked
        {
            int result = hash;
            result += 13 * Tag.GetHashCode();
            result += _TerminalCategoriesSupportedList?.GetHashCode() ?? 0;
            result += _DirectoryEntry.Aggregate(0, (total, next) => next.GetHashCode() + total);

            return result;
        }
    }

    public override int GetHashCode(ConstructedValue obj) => obj.GetHashCode();

    #endregion
}