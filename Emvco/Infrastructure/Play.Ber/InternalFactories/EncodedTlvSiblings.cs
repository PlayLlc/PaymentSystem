using System;
using System.Buffers;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Core.Exceptions;

namespace Play.Ber.InternalFactories;

/// <summary>
///     An stateful object used when decoding constructed ASN.1 types. It includes the metadata that the
///     <see cref="BerCodec" />
///     will need to successfully parse the constructed object's children
/// </summary>
public readonly struct EncodedTlvSiblings
{
    #region Static Metadata

    private static readonly ArrayPool<ReadOnlyMemory<byte>> _ArrayPool = ArrayPool<ReadOnlyMemory<byte>>.Shared;

    #endregion

    #region Instance Values

    private readonly ReadOnlyMemory<byte> _ChildEncodings;
    private readonly Memory<TagLength> _ChildMetadata;

    #endregion

    #region Constructor

    internal EncodedTlvSiblings(Memory<TagLength> childMetadata, ReadOnlyMemory<byte> childEncodings)
    {
        CheckCore.ForEmptySequence(childMetadata, nameof(childMetadata));
        CheckCore.ForEmptySequence(childEncodings, nameof(childEncodings));

        _ChildMetadata = childMetadata;
        _ChildEncodings = childEncodings;
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     AsTagLengthValues
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Exceptions._Temp.BerFormatException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public TagLengthValue[] AsTagLengthValues()
    {
        TagLengthValue[]? result = new TagLengthValue[_ChildMetadata.Length];

        for (int i = 0; i < _ChildMetadata.Length; i++)
        {
            Tag tag = _ChildMetadata.Span[i].GetTag();
            TagLengthValue? tlv = new(tag, GetValueOctetsOfChild(tag).Span);
        }

        return result;
    }

    public uint GetFirstTag() => _ChildMetadata.Span[0].GetTag();

    private int GetSequenceOfCount(uint tag)
    {
        int result = 0;

        for (int i = 0; i < _ChildMetadata.Length; i++)
        {
            if (_ChildMetadata.Span[i].GetTag() == tag)
                result++;
        }

        return result;
    }

    /// <exception cref="EncodingException"></exception>
    /// <exception cref="BerParsingException"></exception>
    internal Tag GetTag(uint value)
    {
        for (int i = 0; i < _ChildMetadata.Length; i++)
        {
            if (_ChildMetadata.Span[i].GetTag() == value)
                return _ChildMetadata.Span[i].GetTag();
        }

        throw new BerParsingException($"The Tag provided with a value of {value:X} could not be found"
                                      + $"from the {nameof(BerConfiguration)} mappings provided. Please make sure your {nameof(PrimitiveValue)} and {nameof(ConstructedValue)} objects have the correct "
                                      + "tags and try again");
    }

    public uint[] GetTags()
    {
        uint[] result = new uint[_ChildMetadata.Length];

        for (int i = 0; i < _ChildMetadata.Length; i++)
            result[i] = _ChildMetadata.Span[i].GetTag();

        return result;
    }

    /// <summary>
    ///     GetRawTlvForNextInstanceInSetOf
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="sequenceNumber"></param>
    /// <returns></returns>
    /// <exception cref="Exceptions._Temp.BerFormatException"></exception>
    /// <exception cref="BerParsingException"></exception>
    private ReadOnlyMemory<byte> GetRawTlvForNextInstanceInSetOf(uint tag, int sequenceNumber)
    {
        int offset = 0;
        int currentSequenceNumber = -1;

        for (int i = 0; i < _ChildMetadata.Length; i++)
        {
            if (tag == _ChildMetadata.Span[i].GetTag())
                currentSequenceNumber++;

            if (currentSequenceNumber == sequenceNumber)
                return _ChildEncodings[offset..(offset + _ChildMetadata.Span[i].GetTagLengthValueByteCount())];

            offset += _ChildMetadata.Span[i].GetTagLengthValueByteCount();
        }

        throw new BerParsingException("There is an internal error when decoding Sequence Of values");
    }

    /// <exception cref="BerFormatException">Ignore.</exception>
    /// <exception cref="BerParsingException"></exception>
    private ReadOnlyMemory<byte> GetValueOctetsForNextInstanceInSetOf(uint tag, int sequenceNumber)
    {
        int offset = 0;
        int currentSequenceNumber = -1;

        for (int i = 0; i < _ChildMetadata.Length; i++)
        {
            if (tag == _ChildMetadata.Span[i].GetTag())
                currentSequenceNumber++;

            if (currentSequenceNumber == sequenceNumber)
            {
                int contentOctetsOffset = offset + _ChildMetadata.Span[i].GetValueOffset();

                return _ChildEncodings[contentOctetsOffset..(contentOctetsOffset + _ChildMetadata.Span[i].GetValueByteCount())];
            }

            offset += _ChildMetadata.Span[i].GetTagLengthValueByteCount();
        }

        throw new BerParsingException("There is an internal error when decoding Sequence Of values");
    }

    public int SiblingCount() => _ChildMetadata.Length;

    /// <summary>
    ///     Searches for all children with the tag provided and returns the raw encoded value content
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

    /// <summary>
    ///     GetValueOctetsOfChild
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    private ReadOnlyMemory<byte> GetValueOctetsOfChild(Tag tag)
    {
        if (!TryGetValueOctetsOfChild(tag, out ReadOnlyMemory<byte> encodedChild))
            throw new BerParsingException($"The {nameof(EncodedTlvSiblings)} did not contain a sibling with the tag {tag.ToString()}");

        return encodedChild;
    }

    public bool TryGetValueOctetsOfChild(uint tag, out ReadOnlyMemory<byte> encodedChild)
    {
        encodedChild = null;
        int offset = 0;

        for (int i = 0; i < _ChildMetadata.Length; i++)
        {
            if (tag == _ChildMetadata.Span[i].GetTag())
            {
                int resultOffset = offset
                    + _ChildMetadata.Span[i].GetTag().GetByteCount()
                    + _ChildMetadata.Span[i].GetLength().GetByteCount();

                encodedChild = _ChildEncodings[resultOffset..(resultOffset + _ChildMetadata.Span[i].GetLength().GetContentLength())];

                return true;
            }

            offset += _ChildMetadata.Span[i].GetTagLengthValueByteCount();
        }

        return false;
    }

    #endregion
}