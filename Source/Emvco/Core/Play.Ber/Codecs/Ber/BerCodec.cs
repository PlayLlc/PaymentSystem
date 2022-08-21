using System;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.InternalFactories;
using Play.Ber.Lengths;
using Play.Ber.Tags;
using Play.Core.Specifications;

namespace Play.Ber.Codecs;

// TODO: You need to consolidate the Decoding in here. Also, the Primitive Value Encoding is confusing as it only encodes
// TODO: the value octet contents. You need a .ToTlv as well or something
/// <summary>
///     The object used for encoding and decoding objects to and from ASN.1 defined types
/// </summary>
public partial class BerCodec
{
    #region Instance Values

    private readonly TagLengthFactory _TagLengthFactory;
    private readonly ValueFactory _ValueFactory;

    #endregion

    #region Constructor

    /// <param name="configuration">
    ///     The configuration used to instantiate a <see cref="BerCodec" /> object. It contains metadata that allows this
    ///     code base to create internal encoding maps for each ASN.1 defined type
    /// </param>
    /// <exception cref="InvalidOperationException"></exception>
    public BerCodec(BerConfiguration configuration)
    {
        _TagLengthFactory = new TagLengthFactory();
        _ValueFactory = new ValueFactory(configuration._PlayCodecMap);
    }

    #endregion

    #region Instance Members

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public byte[] GetContentOctets(ReadOnlySpan<byte> value)
    {
        TagLength tagLength = _TagLengthFactory.ParseFirstTagLength(value);

        return value[tagLength.ValueRange()].ToArray();
    }

    #region Encode

    /// <exception cref="BerParsingException"></exception>
    private byte[] EncodeEmptyDataObject(IEncodeBerDataObjects value)
    {
        TagLength tagLength = new(value.GetTag(), Array.Empty<byte>());

        return tagLength.Encode();
    }

    #endregion

    #endregion

    #region Decode

    /// <summary>
    ///     Decodes the first <see cref="Tag" /> found in the argument provided
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public Tag DecodeFirstTag(ReadOnlySpan<byte> value) => new(value);

    /// <summary>
    ///     Decodes the first <see cref="TagLength" /> found in the argument provided
    /// </summary>
    /// <param name="value"></param>
    /// <remarks>
    ///     This is expected to be a BER encoded Tag-Length sequence
    /// </remarks>
    /// <returns>
    ///     <see cref="TagLength" />
    /// </returns>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public TagLength DecodeFirstTagLength(ReadOnlySpan<byte> value)
    {
        Tag tag = new(value);
        Length length = Length.Parse(value[tag.GetByteCount()..]);

        return new TagLength(tag, length);
    }

    /// <summary>
    ///     Parses a sequence of metadata containing concatenated Tag-Length values and returns an array of the Tag-Length
    ///     pairs.
    /// </summary>
    /// <remarks>
    ///     This method expects that the argument provided will only contain Tag-Length pairs. It will not handle a sequence of
    ///     Tag-Length-Value
    /// </remarks>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public TagLength[] DecodeTagLengths(ReadOnlySpan<byte> value)
    {
        SpanOwner<TagLength> spanOwner = SpanOwner<TagLength>.Allocate(value.Length / 2);
        Span<TagLength> buffer = spanOwner.Span;

        int bufferOffset = 0;

        for (int j = 0; j < value.Length; bufferOffset++)
        {
            buffer[bufferOffset] = DecodeFirstTagLength(value[j..]);

            j += buffer[bufferOffset].GetTag().GetByteCount() + buffer[bufferOffset].GetLength().GetByteCount();
        }

        return buffer[..bufferOffset].ToArray();
    }

    /// <summary>
    ///     Parses a sequence of metadata containing a concatenation of Tag identifiers and returns an array of Tags.
    /// </summary>
    /// <remarks>
    ///     This method expects that the argument provided will only contain Tag-Length pairs. It will not handle a sequence of
    ///     Tag-Length-Value
    /// </remarks>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public Tag[] DecodeTags(ReadOnlySpan<byte> value)
    {
        if (value.Length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<Tag> buffer = stackalloc Tag[value.Length];
            int i = 0;

            for (int j = 0; j < value.Length; i++)
            {
                Tag tempValue = new(value[j..]);
                buffer[i] = tempValue;
                j += tempValue.GetByteCount();
            }

            return buffer[..i].ToArray();
        }
        else
        {
            using SpanOwner<Tag> spanOwner = SpanOwner<Tag>.Allocate(value.Length);
            Span<Tag> buffer = spanOwner.Span;
            int i = 0;

            for (int j = 0; j < value.Length; i++)
            {
                Tag tempValue = new(value[j..]);
                buffer[i] = tempValue;
                j += tempValue.GetByteCount();
            }

            return buffer[..i].ToArray();
        }
    }

    // HACK: We should probably encapsulate this method and not expose it to the outside world
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public EncodedTlvSiblings DecodeChildren(ReadOnlyMemory<byte> value)
    {
        TagLength tagLength = _TagLengthFactory.ParseFirstTagLength(value.Span);

        if (tagLength.GetValueByteCount() == 0)
            return new EncodedTlvSiblings();

        return DecodeSiblings(value[tagLength.ValueRange()]);
    }

    // HACK: We should probably encapsulate this method and not expose it to the outside world
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public EncodedTlvSiblings DecodeSiblings(ReadOnlyMemory<byte> value) => new(_TagLengthFactory.GetTagLengthArray(value.Span), value);

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public TagLengthValue DecodeTagLengthValue(ReadOnlySpan<byte> value)
    {
        TagLength tagLength = _TagLengthFactory.ParseFirstTagLength(value);

        return new TagLengthValue(tagLength.GetTag(), value[tagLength.ValueRange()]);
    }

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="OverflowException"></exception>
    public TagLengthValue[] DecodeTagLengthValues(ReadOnlyMemory<byte> value) => DecodeTagLengthValues(value.Span);

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="OverflowException"></exception>
    public TagLengthValue[] DecodeTagLengthValues(ReadOnlySpan<byte> value)
    {
        TagLength[]? tagLengthArray = _TagLengthFactory.GetTagLengthArray(value);

        if (tagLengthArray.Length == 0)
            return Array.Empty<TagLengthValue>();

        TagLengthValue[] result = new TagLengthValue[tagLengthArray.Length];

        for (int i = 0, j = 0; i < tagLengthArray.Length; i++)
        {
            Range contentOctetsValueRange = tagLengthArray[i].ValueRange();
            int startOfValueRange = j + contentOctetsValueRange.Start.Value;
            int endOfValueRange = j + contentOctetsValueRange.End.Value;

            result[i] = new TagLengthValue(tagLengthArray[i].GetTag(), value[startOfValueRange..endOfValueRange]);

            j += contentOctetsValueRange.End.Value;
        }

        return result;
    }

    #endregion
}