using System;
using System.Linq;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Ber.Lengths;
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

    /// <summary>
    ///     Decodes the first <see cref="Tag" /> found in the
    ///     <param name="value"></param>
    /// </summary>
    /// <param name="value"></param>
    /// <returns>
    ///     <see cref="TagLength" />
    /// </returns>
    public uint GetFirstTag(ReadOnlySpan<byte> value) => _TagLengthFactory.ParseFirst(value).GetTag();

    public byte[] GetContentOctets(ReadOnlySpan<byte> value) => value[_TagLengthFactory.ParseFirst(value).GetTagLengthByteCount()..].ToArray();

    private byte[] EncodeEmptyDataObject(IEncodeBerDataObjects value)
    {
        TagLength tagLength = new(value.GetTag(), Array.Empty<byte>());

        return tagLength.Encode();
    }

    /// <summary>
    ///     Parses a sequence of metadata containing concatenated Tag-Length values and returns an array
    ///     of the Tag-Length pairs.
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <remarks>
    ///     This method expects that the argument provided will only contain Tag-Length pairs. It will
    ///     not handle a sequence of Tag-Length-Value
    /// </remarks>
    public TagLength[] DecodeTagLengthPairs(ReadOnlySpan<byte> value)
    {
        SpanOwner<TagLength> spanOwner = SpanOwner<TagLength>.Allocate(value.Length / 2);
        Span<TagLength> buffer = spanOwner.Span;

        int bufferOffset = 0;

        for (int j = 0; j < value.Length; bufferOffset++)
        {
            buffer[bufferOffset] = DecodeTagLength(value[j..]);

            j += buffer[bufferOffset].GetTag().GetByteCount() + buffer[bufferOffset].GetLength().GetByteCount();
        }

        return buffer[..bufferOffset].ToArray();
    }

    public bool IsTagPresent(Tag tagToSearch, ReadOnlySpan<byte> value)
    {
        for (int i = 0; i < value.Length;)
        {
            Tag currentTag = new(value[i..]);

            if (currentTag == tagToSearch)
                return true;

            i += currentTag.GetByteCount();
        }

        return false;
    }

    /// <summary>
    ///     Parses a sequence of metadata containing a concatenation of Tag identifiers and returns an array of Tags.
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <remarks>
    ///     This method expects that the argument provided will only contain Tag-Length pairs. It will
    ///     not handle a sequence of Tag-Length-Value
    /// </remarks>
    public Tag[] DecodeTagSequence(ReadOnlySpan<byte> value)
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

    #endregion

    #region Serialization

    /// <summary>
    ///     EncodeValue
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private byte[] EncodeValue(IEncodeBerDataObjects value)
    {
        if (value is PrimitiveValue primitiveValue)
            return EncodeTagLengthValue(primitiveValue);

        if (value is ConstructedValue constructedValue)
            return EncodeValue(constructedValue);

        if (value is SetOf setOfValues)
            return EncodeValue(setOfValues);

        throw new BerParsingException("This exception should never be thrown");
    }

    #endregion

    #region Decode

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public EncodedTlvSiblings DecodeChildren(ReadOnlyMemory<byte> value)
    {
        TagLength tagLength = _TagLengthFactory.ParseFirst(value.Span);

        return DecodeSiblings(value[tagLength.GetValueOffset()..]);
    }

    public EncodedTlvSiblings DecodeSiblings(ReadOnlyMemory<byte> value) => new EncodedTlvSiblings(_TagLengthFactory.GetTagLengthArray(value.Span), value);

    /// <summary>
    ///     Decodes the first <see cref="TagLength" /> found in the
    ///     <param name="value"></param>
    /// </summary>
    /// <param name="value"></param>
    /// <remarks>
    ///     This is expected to be a BER encoded Tag-Length sequence
    /// </remarks>
    /// <returns>
    ///     <see cref="TagLength" />
    /// </returns>
    /// <exception cref="BerParsingException"></exception>
    public TagLength DecodeTagLength(ReadOnlySpan<byte> value)
    {
        Tag tag = new(value);
        Length length = Length.Parse(value[tag.GetByteCount()..]);

        return new TagLength(tag, length);
    }

    /// <summary>
    ///     DecodeTagLengthValue
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    public TagLengthValue DecodeTagLengthValue(ReadOnlySpan<byte> value)
    {
        Tag tag = new(value);
        Length length = Length.Parse(value[tag.GetByteCount()..]);

        return new TagLengthValue(tag, value[(tag.GetByteCount() + length.GetByteCount())..]);
    }

    public Tag DecodeTag(ReadOnlySpan<byte> value) => new(value);

    /// <exception cref="BerParsingException"></exception>
    public TagLengthValue[] DecodeTagLengthValues(ReadOnlyMemory<byte> value) => DecodeTagLengthValues(value.Span);

    /// <exception cref="BerParsingException"></exception>
    public TagLengthValue[] DecodeConstructedTagLengthValues(ReadOnlyMemory<byte> value)
    {
        return DecodeTagLengthValues(value).Where(a => a.GetTag().IsConstructed()).ToArray();
    }

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public TagLengthValue[] DecodeTagLengthValues(ReadOnlySpan<byte> value)
    {
        TagLength[]? tagLengthArray = _TagLengthFactory.GetTagLengthArray(value);

        if (tagLengthArray.Length == 0)
            return Array.Empty<TagLengthValue>();

        TagLengthValue[] result = new TagLengthValue[tagLengthArray.Length];

        for (int i = 0, j = 0; i < tagLengthArray.Length; i++)
            result[i] = new TagLengthValue(tagLengthArray[i].GetTag(), value[j..tagLengthArray[i].GetLength().GetByteCount()]);

        return result;
    }

    #endregion
}