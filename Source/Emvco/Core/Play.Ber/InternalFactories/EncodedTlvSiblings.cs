using System;
using System.Buffers;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;

namespace Play.Ber.InternalFactories;

/// <summary>
///     An stateful object used when decoding constructed ASN.1 types. It includes the metadata that the
///     <see cref="BerCodec" /> will need to successfully parse the constructed object's siblings
/// </summary>
public readonly struct EncodedTlvSiblings
{
    #region Instance Values

    private readonly ReadOnlyMemory<byte> _SiblingEncodings = new();
    private readonly Memory<TagLength> _SiblingMetadata = new();

    #endregion

    #region Constructor

    internal EncodedTlvSiblings(Memory<TagLength>? siblingMetadata, ReadOnlyMemory<byte>? siblingEncodings)
    {
        _SiblingMetadata = siblingMetadata is null ? new Memory<TagLength>() : siblingMetadata!.Value;

        _SiblingEncodings = siblingEncodings is null ? Array.Empty<byte>() : siblingEncodings!.Value;
    }

    internal EncodedTlvSiblings(Memory<TagLength> siblingMetadata, ReadOnlyMemory<byte> siblingEncodings)
    {
        _SiblingMetadata = siblingMetadata;
        _SiblingEncodings = siblingEncodings;
    }

    #endregion

    #region Instance Members

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public TagLengthValue[] AsTagLengthValues()
    {
        TagLengthValue[]? result = new TagLengthValue[_SiblingMetadata.Length];

        for (int i = 0; i < _SiblingMetadata.Length; i++)
        {
            Tag tag = _SiblingMetadata.Span[i].GetTag();
            result[i] = new TagLengthValue(tag, GetValueOctetsOfSibling(tag));
        }

        return result;
    }

    public uint GetFirstTag() => _SiblingMetadata.Span[0].GetTag();

    private int GetSequenceOfCount(uint tag)
    {
        int result = 0;

        for (int i = 0; i < _SiblingMetadata.Length; i++)
        {
            if (_SiblingMetadata.Span[i].GetTag() == tag)
                result++;
        }

        return result;
    }

    /// <exception cref="BerParsingException"></exception>
    internal Tag GetTag(uint value)
    {
        for (int i = 0; i < _SiblingMetadata.Length; i++)
        {
            if (_SiblingMetadata.Span[i].GetTag() == value)
                return _SiblingMetadata.Span[i].GetTag();
        }

        throw new BerParsingException($"The Tag provided with a value of {value:X} could not be found"
            + $"from the {nameof(BerConfiguration)} mappings provided. Please make sure your {nameof(PrimitiveValue)} and {nameof(ConstructedValue)} objects have the correct "
            + "tags and try again");
    }

    public uint[] GetTags()
    {
        uint[] result = new uint[_SiblingMetadata.Length];

        for (int i = 0; i < _SiblingMetadata.Length; i++)
            result[i] = _SiblingMetadata.Span[i].GetTag();

        return result;
    }

    /// <summary>
    ///     GetRawTlvForNextInstanceInSetOf
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="sequenceNumber"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    private ReadOnlyMemory<byte> GetRawTlvForNextInstanceInSetOf(uint tag, int sequenceNumber)
    {
        int offset = 0;
        int currentSequenceNumber = -1;

        for (int i = 0; i < _SiblingMetadata.Length; i++)
        {
            if (tag == _SiblingMetadata.Span[i].GetTag())
                currentSequenceNumber++;

            if (currentSequenceNumber == sequenceNumber)
                return _SiblingEncodings[offset..(offset + _SiblingMetadata.Span[i].GetTagLengthValueByteCount())];

            offset += _SiblingMetadata.Span[i].GetTagLengthValueByteCount();
        }

        throw new BerParsingException("There is an internal error when decoding Sequence Of values");
    }

    /// <exception cref="BerParsingException"></exception>
    private ReadOnlyMemory<byte> GetValueOctetsForNextInstanceInSetOf(uint tag, int sequenceNumber)
    {
        int offset = 0;
        int currentSequenceNumber = -1;

        for (int i = 0; i < _SiblingMetadata.Length; i++)
        {
            if (tag == _SiblingMetadata.Span[i].GetTag())
                currentSequenceNumber++;

            if (currentSequenceNumber == sequenceNumber)
            {
                int contentOctetsOffset = offset + _SiblingMetadata.Span[i].GetValueOffset();

                return _SiblingEncodings[contentOctetsOffset..(contentOctetsOffset + _SiblingMetadata.Span[i].GetValueByteCount())];
            }

            offset += _SiblingMetadata.Span[i].GetTagLengthValueByteCount();
        }

        throw new BerParsingException("There is an internal error when decoding Sequence Of values");
    }

    public int SiblingCount() => _SiblingMetadata.Length;

    /// <summary>
    ///     Searches for all siblings with the tag provided and returns the raw encoded value content
    /// </summary>
    /// ///
    /// <exception cref="BerParsingException"></exception>
    public bool TryGetRawSetOf(uint tag, out Span<ReadOnlyMemory<byte>> sequenceOfResult)
    {
        int setOfCount = GetSequenceOfCount(tag);
        using SpanOwner<ReadOnlyMemory<byte>> spanOwner = SpanOwner<ReadOnlyMemory<byte>>.Allocate(setOfCount);

        if (setOfCount == 0)
        {
            sequenceOfResult = Array.Empty<ReadOnlyMemory<byte>>();

            return false;
        }

        sequenceOfResult = spanOwner.Span;

        for (int i = 0; i < setOfCount; i++)
            sequenceOfResult[i] = GetRawTlvForNextInstanceInSetOf(tag, i).ToArray();

        return true;
    }

    /// <exception cref="BerParsingException"></exception>
    public ReadOnlySpan<byte> GetValueOctetsOfSibling(Tag tag)
    {
        if (!TryGetValueOctetsOfSibling(tag, out ReadOnlySpan<byte> encodedSibling))
            throw new BerParsingException($"The {nameof(EncodedTlvSiblings)} did not contain a sibling with the tag {tag.ToString()}");

        return encodedSibling;
    }

    public bool TryGetValueOctetsOfSibling(Tag tag, out ReadOnlySpan<byte> encodedSibling)
    {
        encodedSibling = null;
        int offset = 0;

        for (int i = 0; i < _SiblingMetadata.Length; i++)
        {
            if (tag == _SiblingMetadata.Span[i].GetTag())
            {
                int resultOffset = offset + _SiblingMetadata.Span[i].GetTag().GetByteCount() + _SiblingMetadata.Span[i].GetLength().GetByteCount();

                encodedSibling = _SiblingEncodings[resultOffset..(resultOffset + _SiblingMetadata.Span[i].GetLength().GetContentLength())].Span;

                return true;
            }

            offset += _SiblingMetadata.Span[i].GetTagLengthValueByteCount();
        }

        return false;
    }

    public bool TryGetValueOctetsOfSibling(Tag tag, out ReadOnlyMemory<byte> encodedSibling)
    {
        encodedSibling = null;
        int offset = 0;

        for (int i = 0; i < _SiblingMetadata.Length; i++)
        {
            if (tag == _SiblingMetadata.Span[i].GetTag())
            {
                int resultOffset = offset + _SiblingMetadata.Span[i].GetTag().GetByteCount() + _SiblingMetadata.Span[i].GetLength().GetByteCount();

                encodedSibling = _SiblingEncodings[resultOffset..(resultOffset + _SiblingMetadata.Span[i].GetLength().GetContentLength())];

                return true;
            }

            offset += _SiblingMetadata.Span[i].GetTagLengthValueByteCount();
        }

        return false;
    }

    #endregion
}