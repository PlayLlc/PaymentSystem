﻿#nullable enable
using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.InternalFactories;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.Templates;

public record FileControlInformationIssuerDiscretionaryDataPpse : FileControlInformationIssuerDiscretionaryDataTemplate
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
        SetOf<DirectoryEntry> directoryEntries, TerminalCategoriesSupportedList? terminalCategoriesSupportedList,
        SelectionDataObjectList? selectionDataObjectList)
    {
        _DirectoryEntry = directoryEntries;
        _TerminalCategoriesSupportedList = terminalCategoriesSupportedList;
        _SelectionDataObjectList = selectionDataObjectList;
    }

    #endregion

    #region Serialization

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="CardDataMissingException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static FileControlInformationIssuerDiscretionaryDataPpse Decode(ReadOnlyMemory<byte> value)
    {
        if (_Codec.DecodeFirstTag(value.Span) == Tag)
            return Decode(_Codec.DecodeChildren(value));

        return Decode(_Codec.DecodeSiblings(value));
    }

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="CardDataMissingException"></exception>
    public static FileControlInformationIssuerDiscretionaryDataPpse Decode(EncodedTlvSiblings encodedTlvSiblings)
    {
        TerminalCategoriesSupportedList? terminalCategoriesSupportedList = default;
        SelectionDataObjectList? selectionDataObjectList = default;

        if (!encodedTlvSiblings.TryGetRawSetOf(DirectoryEntry.Tag, out Span<ReadOnlyMemory<byte>> sequenceOfResult))
        {
            throw new CardDataMissingException(
                $"A problem occurred while decoding {nameof(FileControlInformationIssuerDiscretionaryDataDdf)}. A Set of {nameof(DirectoryEntry)} objects were expected but could not be found");
        }

        SetOf<DirectoryEntry> directoryEntry = new(sequenceOfResult.ToArray().Select(DirectoryEntry.Decode).ToArray());

        //if (encodedTlvSiblings.TryGetValueOctetsOfSibling(TerminalCategoriesSupportedList.Tag, out ReadOnlyMemory<byte> rawTerminalCategoriesSupportedList))
        //{
        //    terminalCategoriesSupportedList =
        //        (_Codec.Decode(TerminalCategoriesSupportedList.EncodingId, rawTerminalCategoriesSupportedList.Span) as
        //            DecodedResult<TerminalCategoriesSupportedList>)?.Value;
        //}

        //if (encodedTlvSiblings.TryGetValueOctetsOfSibling(SelectionDataObjectList.Tag, out ReadOnlyMemory<byte> rawSelectionDataObjectList))
        //{
        //    selectionDataObjectList = (_Codec.Decode(SelectionDataObjectList.EncodingId, rawSelectionDataObjectList.Span) as
        //        DecodedResult<SelectionDataObjectList>)?.Value;
        //}

        terminalCategoriesSupportedList = _Codec.AsPrimitive(TerminalCategoriesSupportedList.Decode, TerminalCategoriesSupportedList.Tag, encodedTlvSiblings);
        selectionDataObjectList = _Codec.AsPrimitive(SelectionDataObjectList.Decode, SelectionDataObjectList.Tag, encodedTlvSiblings);

        return new FileControlInformationIssuerDiscretionaryDataPpse(directoryEntry, terminalCategoriesSupportedList, selectionDataObjectList);
    }

    #endregion

    #region Equality

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

    #endregion

    #region Instance Members

    public override Tag[] GetChildTags() => ChildTags;

    /// <summary>
    ///     AsCommandTemplate
    /// </summary>
    /// <param name="codec"></param>
    /// <param name="poiInformation"></param>
    /// <param name="selectionDataObjectListValues"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    public CommandTemplate AsCommandTemplate(BerCodec codec, PoiInformation poiInformation, IReadTlvDatabase database)
    {
        if ((_SelectionDataObjectList != null) && (!_TerminalCategoriesSupportedList?.IsPointOfInteractionApduCommandRequested() ?? false))
            return _SelectionDataObjectList.AsCommandTemplate(database);

        if (_TerminalCategoriesSupportedList?.IsPointOfInteractionApduCommandRequested() ?? false)
            return CommandTemplate.Decode(poiInformation.EncodeValue().AsSpan());

        if ((_SelectionDataObjectList != null) && (_TerminalCategoriesSupportedList?.IsPointOfInteractionApduCommandRequested() ?? false))
        {
            Span<byte> dolEncoding = _SelectionDataObjectList.AsCommandTemplate(database).EncodeValue();
            Span<byte> terminalCategoryEncoding = poiInformation.EncodeValue(codec);

            return CommandTemplate.Decode(dolEncoding.ConcatArrays(terminalCategoryEncoding).AsSpan());
        }

        return new CommandTemplate(0);
    }

    /// <summary>
    ///     AsCommandTemplate
    /// </summary>
    /// <param name="database"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public CommandTemplate AsCommandTemplate(IReadTlvDatabase database)
    {
        if ((_SelectionDataObjectList != null) && (!_TerminalCategoriesSupportedList?.IsPointOfInteractionApduCommandRequested() ?? false))
            return _SelectionDataObjectList.AsCommandTemplate(database);

        if (database.IsPresentAndNotEmpty(PoiInformation.Tag) && (_TerminalCategoriesSupportedList?.IsPointOfInteractionApduCommandRequested() ?? false))
            return CommandTemplate.Decode(database.Get(PoiInformation.Tag).EncodeValue(_Codec).AsSpan());

        if ((_SelectionDataObjectList != null)
            && database.IsPresentAndNotEmpty(PoiInformation.Tag)
            && (_TerminalCategoriesSupportedList?.IsPointOfInteractionApduCommandRequested() ?? false))
        {
            Span<byte> dolEncoding = _SelectionDataObjectList.AsCommandTemplate(database).EncodeValue();
            Span<byte> terminalCategoryEncoding = database.Get(PoiInformation.Tag).EncodeValue(_Codec);

            return CommandTemplate.Decode(dolEncoding.ConcatArrays(terminalCategoryEncoding).AsSpan());
        }

        return new CommandTemplate(0);
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
        return new IEncodeBerDataObjects?[] {_DirectoryEntry, _SelectionDataObjectList, _TerminalCategoriesSupportedList };
    }

    #endregion
}