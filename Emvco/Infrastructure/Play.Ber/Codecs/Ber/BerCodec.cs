using System;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Ber.Lengths;

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
    /// <exception cref="Play.Ber.Exceptions.BerException"></exception>
    /// <exception cref="Play.Ber.Exceptions.BerInternalException"></exception>
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
    /// <exception cref="BerException"></exception>
    /// <exception cref="BerInternalException"></exception>
    public TagLengthValue DecodeTagLengthValue(ReadOnlySpan<byte> value)
    {
        Tag tag = new(value);
        Length length = Length.Parse(value[tag.GetByteCount()..]);

        return new TagLengthValue(tag, value[((byte) tag.GetByteCount() + length.GetByteCount())..]);
    }

    public Tag DecodeTag(ReadOnlySpan<byte> value) => new(value);

    /// <summary>
    ///     DecodeTagLengthValues
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="Play.Ber.Exceptions.BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Play.Ber.Exceptions.BerInternalException"></exception>
    public TagLengthValue[] DecodeTagLengthValues(ReadOnlyMemory<byte> value) => DecodeTagLengthValues(value.Span);

    /// <summary>
    ///     DecodeTagLengthValues
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="Play.Ber.Exceptions.BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Play.Ber.Exceptions.BerInternalException"></exception>
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

    /// <summary>
    ///     Decodes the first <see cref="Tag" /> found in the
    ///     <param name="value"></param>
    /// </summary>
    /// <param name="value"></param>
    /// <returns>
    ///     <see cref="TagLength" />
    /// </returns>
    public uint GetFirstTag(ReadOnlySpan<byte> value) => _TagLengthFactory.ParseFirst(value).GetTag();

    public byte[] GetContentOctets(ReadOnlySpan<byte> value) =>
        value[_TagLengthFactory.ParseFirst(value).GetTagLengthByteCount()..].ToArray();

    private byte[] EncodeEmptyDataObject(IEncodeBerDataObjects value)
    {
        Span<byte> result = stackalloc byte[Tag.GetByteCount(value) + 1];
        Tag.Serialize(value).AsSpan().CopyTo(result);

        return result.ToArray();
    }

    #endregion

    #region Serialization

    /// <summary>
    ///     EncodeValue
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="Play.Ber.Exceptions.BerException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private byte[] EncodeValue(IEncodeBerDataObjects value)
    {
        if (value is PrimitiveValue primitiveValue)
            return EncodeTagLengthValue(primitiveValue);

        if (value is ConstructedValue constructedValue)
            return EncodeValue(constructedValue);

        if (value is SetOf setOfValues)
            return EncodeValue(setOfValues);

        throw new BerInternalException("This exception should never be thrown");
    }

    #endregion
}