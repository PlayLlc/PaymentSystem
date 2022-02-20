using System.Numerics;

using Play.Codecs;
using Play.Codecs.Strings;
using Play.Core.Extensions;
using Play.Interchange.Codecs;
using Play.Interchange.Exceptions;
using Play.Interchange.Messages.DataFields;

namespace Play.Interchange.DataFields;

public abstract record VariableDataField<T> : InterchangeDataField
{
    #region Instance Values

    protected readonly T _Value;

    #endregion

    #region Constructor

    protected VariableDataField(T value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public override ushort GetByteCount() => (ushort) (GetLeadingOctetByteCount() + _Codec.GetByteCount(GetEncodingId(), _Value!));

    public override byte[] Encode()
    {
        byte[] result = new byte[GetByteCount()];
        int offset = 0;
        _Codec.Encode(GetEncodingId(), _Value!, result.AsMemory(), ref offset);

        return result;
    }

    public override void Encode(Memory<byte> buffer, ref int offset)
    {
        EncodeLeadingOctets(buffer.Span, ref offset);
        _Codec.Encode(GetEncodingId(), _Value!, buffer, ref offset);
    }

    protected abstract ushort GetMaxByteCount();

    protected void EncodeLeadingOctets(Span<byte> buffer, ref int offset)
    {
        ReadOnlySpan<byte> leadingOctets =
            PlayEncoding.Numeric.GetBytes(_Codec.GetByteCount(GetEncodingId(), _Value!), GetLeadingOctetByteCount());

        leadingOctets.CopyTo(buffer[offset..]);
        offset += leadingOctets.Length;
    }

    protected abstract ushort GetLeadingOctetByteCount();

    #endregion
}

public abstract record FixedDataField<T> : InterchangeDataField
{
    #region Instance Values

    protected readonly T _Value;

    #endregion

    #region Constructor

    protected FixedDataField(T value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public override ushort GetByteCount() => _Codec.GetByteCount(GetEncodingId(), _Value!);

    public override byte[] Encode()
    {
        byte[] result = new byte[GetByteCount()];
        int offset = 0;
        _Codec.Encode(GetEncodingId(), _Value!, result.AsMemory(), ref offset);

        return result;
    }

    public override void Encode(Memory<byte> buffer, ref int offset)
    {
        _Codec.Encode(GetEncodingId(), _Value!, buffer, ref offset);
    }

    #endregion

    #region Serialization

    public byte[] Decode(ReadOnlySpan<byte> value)
    {
        Check.DataField.ForExactLength(value, GetByteCount(), GetDataFieldId());

        return value.ToArray();
    }

    #endregion
}

public abstract record InterchangeDataField : IRetrieveInterchangeFieldMetadata, IEncodeInterchangeFields
{
    #region Static Metadata

    protected static readonly InterchangeCodec _Codec = new();

    #endregion

    #region Instance Members

    public abstract DataFieldId GetDataFieldId();
    public abstract ushort GetByteCount();
    public abstract InterchangeEncodingId GetEncodingId();
    public abstract byte[] Encode();
    DataField IEncodeInterchangeFields.AsDataField() => new(GetDataFieldId(), Encode());
    public abstract void Encode(Memory<byte> buffer, ref int offset);

    #endregion

    #region Serialization

    public abstract InterchangeDataField Decode(ReadOnlyMemory<byte> value);

    #endregion
}