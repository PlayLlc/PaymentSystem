using System;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.InternalFactories;
using Play.Ber.Lengths;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Exceptions;
using Play.Core.Specifications;

namespace Play.Ber.Codecs;

public partial class BerCodec
{
    #region Serialization

    #region Decode Metadata

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public DecodedMetadata Decode(PlayEncodingId codecIdentifier, ReadOnlySpan<byte> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        return _ValueFactory.Decode(codecIdentifier, value);
    }

    #endregion

    #endregion

    #region Decode Primitive

    // DEPRECATING: This method will eventually be deprecated in favor of using strongly typed arguments instead of generic constraints
    public T? AsPrimitive<T>(Func<ReadOnlyMemory<byte>, T> decodeFunc, uint tag, EncodedTlvSiblings encodedTlvSiblings) where T : PrimitiveValue
    {
        if (!encodedTlvSiblings.TryGetValueOctetsOfSibling(tag, out ReadOnlyMemory<byte> rawDedicatedFileName))
            return null;

        return decodeFunc.Invoke(rawDedicatedFileName);
    }

    // DEPRECATING: This method will eventually be deprecated in favor of using strongly typed arguments instead of generic constraints
    public T? AsPrimitive<T>(Func<ReadOnlyMemory<byte>, BerCodec, T> decodeFunc, uint tag, EncodedTlvSiblings encodedTlvSiblings) where T : PrimitiveValue
    {
        if (!encodedTlvSiblings.TryGetValueOctetsOfSibling(tag, out ReadOnlyMemory<byte> rawDedicatedFileName))
            return null;

        return decodeFunc.Invoke(rawDedicatedFileName, this);
    }

    // DEPRECATING: This method will eventually be deprecated in favor of using strongly typed arguments instead of generic constraints
    public T? AsPrimitive<T>(Func<ReadOnlyMemory<byte>, BerCodec, T> decodeFunc, ReadOnlyMemory<byte> rawEncoding) where T : PrimitiveValue
    {
        CheckCore.ForEmptySequence(rawEncoding, nameof(rawEncoding));

        return decodeFunc.Invoke(rawEncoding, this);
    }

    #endregion

    #region Byte Count

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public ushort GetByteCount(PlayEncodingId playEncodingId, dynamic value) => _ValueFactory.GetByteCount(playEncodingId, value);

    // DEPRECATING: This method will eventually be deprecated in favor of using strongly typed arguments instead of generic constraints
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public ushort GetByteCount<T>(PlayEncodingId playEncodingId, T value) where T : struct => _ValueFactory.GetByteCount(playEncodingId, value);

    // DEPRECATING: This method will eventually be deprecated in favor of using strongly typed arguments instead of generic constraints
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public ushort GetByteCount<T>(PlayEncodingId playEncodingId, T[] value) where T : struct => _ValueFactory.GetByteCount(playEncodingId, value);

    #endregion

    #region Encode Value

    // DEPRECATING: This method will eventually be deprecated in favor of using strongly typed arguments instead of generic constraints
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public byte[] EncodeValue(PlayEncodingId playEncodingId, dynamic value) => _ValueFactory.Encode(playEncodingId, value);

    // DEPRECATING: This method will eventually be deprecated in favor of using strongly typed arguments instead of generic constraints
    public byte[] EncodeValue(PlayEncodingId playEncodingId, dynamic value, int length) => _ValueFactory.Encode(playEncodingId, value, length);

    // DEPRECATING: This method will eventually be deprecated in favor of using strongly typed arguments instead of generic constraints
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public byte[] EncodeValue<T>(PlayEncodingId playEncodingId, T value) where T : struct => _ValueFactory.Encode(playEncodingId, value);

    // DEPRECATING: This method will eventually be deprecated in favor of using strongly typed arguments instead of generic constraints
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public byte[] EncodeValue<T>(PlayEncodingId playEncodingId, T value, int length) where T : struct => _ValueFactory.Encode(playEncodingId, value, length);

    // DEPRECATING: This method will eventually be deprecated in favor of using strongly typed arguments instead of generic constraints
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public byte[] EncodeValue<T>(PlayEncodingId playEncodingId, T[] value) where T : struct => _ValueFactory.Encode(playEncodingId, value);

    // DEPRECATING: This method will eventually be deprecated in favor of using strongly typed arguments instead of generic constraints
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public byte[] EncodeValue<T>(PlayEncodingId playEncodingId, T[] value, int length) where T : struct => _ValueFactory.Encode(playEncodingId, value, length);

    #endregion

    #region Encode Tag Length Value

    /// <exception cref="BerParsingException"></exception>
    public byte[] EncodeTagLengthValue(PrimitiveValue value, int length)
    {
        ReadOnlySpan<byte> contentOctets = value.EncodeValue(this, length);

        return EncodeTagLengthValue(value, contentOctets);
    }

    /// <exception cref="BerParsingException"></exception>
    public byte[] EncodeTagLengthValue(Tag tag, ReadOnlySpan<byte> contentOctets)
    {
        Length length = new((ushort) contentOctets.Length);
        Span<byte> buffer = stackalloc byte[tag.GetByteCount() + length.GetByteCount() + contentOctets.Length];

        tag.Serialize().CopyTo(buffer);
        length.Serialize().CopyTo(buffer[tag.GetByteCount()..]);
        contentOctets.CopyTo(buffer[^contentOctets.Length..]);

        return buffer.ToArray();
    }

    /// <exception cref="BerParsingException"></exception>
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

    /// <exception cref="BerParsingException"></exception>
    public byte[] EncodeTagLengthValue(PrimitiveValue value)
    {
        ReadOnlySpan<byte> contentOctets = value.EncodeValue(this);

        return EncodeTagLengthValue(value, contentOctets);
    }

    #endregion
}