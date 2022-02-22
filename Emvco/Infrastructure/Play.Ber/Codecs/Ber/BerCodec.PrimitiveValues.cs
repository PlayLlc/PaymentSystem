using System;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber.DataObjects;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Core.Exceptions;
using Play.Core.Specifications;

namespace Play.Ber.Codecs;

public partial class BerCodec
{
    #region Instance Members

    public T? AsPrimitive<T>(Func<ReadOnlyMemory<byte>, T> decodeFunc, uint tag, EncodedTlvSiblings encodedTlvSiblings)
        where T : PrimitiveValue
    {
        if (!encodedTlvSiblings.TryGetValueOctetsOfChild(tag, out ReadOnlyMemory<byte> rawDedicatedFileName))
            return null;

        return decodeFunc.Invoke(rawDedicatedFileName);
    }

    public T? AsPrimitive<T>(Func<ReadOnlyMemory<byte>, BerCodec, T> decodeFunc, uint tag, EncodedTlvSiblings encodedTlvSiblings)
        where T : PrimitiveValue
    {
        if (!encodedTlvSiblings.TryGetValueOctetsOfChild(tag, out ReadOnlyMemory<byte> rawDedicatedFileName))
            return null;

        return decodeFunc.Invoke(rawDedicatedFileName, this);
    }

    public T? AsPrimitive<T>(Func<ReadOnlyMemory<byte>, BerCodec, T> decodeFunc, ReadOnlyMemory<byte> rawEncoding) where T : PrimitiveValue
    {
        CheckCore.ForEmptySequence(rawEncoding, nameof(rawEncoding));

        return decodeFunc.Invoke(rawEncoding, this);
    }

    public ushort GetByteCount(PlayEncodingId playEncodingId, dynamic value) => _ValueFactory.GetByteCount(playEncodingId, value);

    public ushort GetByteCount<T>(PlayEncodingId playEncodingId, T value) where T : struct =>
        _ValueFactory.GetByteCount(playEncodingId, value);

    public ushort GetByteCount<T>(PlayEncodingId playEncodingId, T[] value) where T : struct =>
        _ValueFactory.GetByteCount(playEncodingId, value);

    #endregion

    #region Serialization

    /// <summary>
    ///     Decode
    /// </summary>
    /// <param name="codecIdentifier"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public DecodedMetadata Decode(PlayEncodingId codecIdentifier, ReadOnlySpan<byte> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        return _ValueFactory.Decode(codecIdentifier, value);
    }

    // TODO: WARNING=================================================================================
    // TODO: This is syntactic sugar and isn't very efficient. Ensure this isn't a bottleneck when
    // TODO: running benchmark testing 
    // TODO: WARNING=================================================================================
    public byte[] EncodeValue(PlayEncodingId playEncodingId, dynamic value) => this.EncodeValue(playEncodingId, value);
    public byte[] EncodeValue(PlayEncodingId playEncodingId, dynamic value, int length) => this.EncodeValue(playEncodingId, value, length);
    public byte[] EncodeValue<T>(PlayEncodingId playEncodingId, T value) where T : struct => _ValueFactory.Encode(playEncodingId, value);

    public byte[] EncodeValue<T>(PlayEncodingId playEncodingId, T value, int length) where T : struct =>
        _ValueFactory.Encode(playEncodingId, value, length);

    public byte[] EncodeValue<T>(PlayEncodingId playEncodingId, T[] value) where T : struct => _ValueFactory.Encode(playEncodingId, value);

    public byte[] EncodeValue<T>(PlayEncodingId playEncodingId, T[] value, int length) where T : struct =>
        _ValueFactory.Encode(playEncodingId, value, length);

    /// <summary>
    ///     EncodeTagLengthValue
    /// </summary>
    /// <param name="value"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    /// <exception cref="Play.Ber.Exceptions.BerException"></exception>
    public byte[] EncodeTagLengthValue(PrimitiveValue value, int length)
    {
        ReadOnlySpan<byte> contentOctets = value.EncodeValue(this, length);

        return EncodeTagLengthValue(value, contentOctets);
    }

    /// <summary>
    ///     EncodeTagLengthValue
    /// </summary>
    /// <param name="value"></param>
    /// <param name="contentOctets"></param>
    /// <returns></returns>
    /// <exception cref="Play.Ber.Exceptions.BerException"></exception>
    private byte[] EncodeTagLengthValue(PrimitiveValue value, ReadOnlySpan<byte> contentOctets)
    {
        TagLength tagLength = new(value.GetTag(), contentOctets);

        if (tagLength.GetTagLengthValueByteCount() < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<byte> buffer = stackalloc byte[tagLength.GetTagLengthValueByteCount()];
            tagLength.Encode().CopyTo(buffer);
            contentOctets.CopyTo(buffer[tagLength.GetValueOffset()..]);

            return buffer.ToArray();
        }
        else
        {
            using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(tagLength.GetTagLengthValueByteCount());
            Span<byte> buffer = spanOwner.Span;
            tagLength.Encode().CopyTo(buffer);
            contentOctets.CopyTo(buffer[tagLength.GetValueOffset()..]);

            return buffer.ToArray();
        }
    }

    /// <summary>
    ///     EncodeTagLengthValue
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="Play.Ber.Exceptions.BerException"></exception>
    public byte[] EncodeTagLengthValue(PrimitiveValue value)
    {
        ReadOnlySpan<byte> contentOctets = value.EncodeValue(this);

        return EncodeTagLengthValue(value, contentOctets);
    }

    #endregion
}