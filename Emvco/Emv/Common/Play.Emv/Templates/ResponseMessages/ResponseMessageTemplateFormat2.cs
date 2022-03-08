using System;
using System.Linq;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;

namespace Play.Emv.Templates.ResponseMessages;

public class ResponseMessageTemplateFormat2 : ConstructedValue
{
    #region Static Metadata

    public static readonly Tag Tag = 0x80;

    #endregion

    #region Instance Values

    private readonly TagLengthValue[] _Value;

    #endregion

    #region Constructor

    public ResponseMessageTemplateFormat2(TagLengthValue[] value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public override Tag GetTag() => Tag;
    public TagLengthValue[] GetTlvChildren() => _Value;
    public int GetTlvChildrenCount() => _Value.Length;

    public override ushort GetValueByteCount(BerCodec codec)
    {
        checked
        {
            return (ushort) _Value.Sum(a => a.GetTagLengthValueByteCount());
        }
    }

    public static TagLengthValue[] DecodeValue(BerCodec codec, ReadOnlyMemory<byte> rawBer) =>
        codec.DecodeSiblings(rawBer).AsTagLengthValues();

    #endregion

    #region Serialization

    public static ResponseMessageTemplateFormat2 Decode(BerCodec codec, ReadOnlyMemory<byte> rawBer) =>
        new(codec.DecodeTagLengthValues(codec.GetContentOctets(rawBer.Span).AsSpan()));

    public override byte[] EncodeTagLengthValue(BerCodec codec) => throw new NotImplementedException();

    /// <summary>
    ///     EncodeValue
    /// </summary>
    /// <param name="codec"></param>
    /// <returns></returns>
    /// <exception cref="Play.Ber.Exceptions.BerException"></exception>
    public override byte[] EncodeValue(BerCodec codec)
    {
        return _Value.SelectMany(a => a.EncodeTagLengthValue()).ToArray();
    }

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is ResponseMessageTemplateFormat2 template && Equals(template);

    public override bool Equals(ConstructedValue? x, ConstructedValue? y)
    {
        if (x == null)
            return y == null;

        if (y == null)
            return false;

        return x.Equals(y);
    }

    public bool Equals(ResponseMessageTemplateFormat2 x, ResponseMessageTemplateFormat2 y) => x.Equals(y);

    public bool Equals(ResponseMessageTemplateFormat2? other)
    {
        if (other is null)
            return false;

        if (other.GetTlvChildrenCount() != GetTlvChildrenCount())
            return false;

        for (int i = 0; i < _Value.Length; i++)
        {
            if (_Value[i] != other._Value[i])
                return false;
        }

        return true;
    }

    public override bool Equals(ConstructedValue? other) => other is ResponseMessageTemplateFormat2 f2 && Equals(f2);

    public override int GetHashCode()
    {
        unchecked
        {
            const int hash = 5059;
            int result = hash * Tag.GetHashCode();
            result += _Value.Sum(a => a.GetHashCode() * hash);

            return result;
        }
    }

    public override int GetHashCode(ConstructedValue obj) => obj.GetHashCode();

    #endregion
}