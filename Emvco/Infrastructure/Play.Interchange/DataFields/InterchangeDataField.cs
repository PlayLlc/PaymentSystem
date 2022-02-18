using Play.Codecs;
using Play.Core.Extensions;
using Play.Interchange.Codecs;

namespace Play.Interchange.DataFields;

public abstract record InterchangeDataField : IRetrieveInterchangeFieldMetadata, IEncodeInterchangeFields
{
    #region Instance Members

    private void EncodeLeadingOctets(ReadOnlySpan<byte> encoding, Span<byte> buffer, ref int offset)
    {
        ReadOnlySpan<byte> leadingOctets = PlayEncoding.Numeric.GetBytes(encoding.Length, GetLeadingOctetByteCount(encoding));

        leadingOctets.CopyTo(buffer[offset..]);
        offset += leadingOctets.Length;
    }

    protected int GetLeadingOctetByteCount(InterchangeCodec codec) => GetByteCount(codec).GetNumberOfDigits() > 2 ? 2 : 1;
    protected static int GetLeadingOctetByteCount(ReadOnlySpan<byte> encoding) => encoding.Length.GetNumberOfDigits() > 2 ? 2 : 1;
    public abstract DataFieldId GetDataFieldId();
    public abstract ushort GetByteCount(InterchangeCodec codec);
    public abstract InterchangeEncodingId GetEncodingId();
    public abstract byte[] Encode(InterchangeCodec codec);
    public DataField AsDataField(InterchangeCodec codec) => new(GetDataFieldId(), Encode(codec));
    public abstract void Encode(InterchangeCodec codec, Span<byte> buffer, ref int offset);

    #endregion
}