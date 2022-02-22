using System.Runtime.CompilerServices;

using Play.Ber.Codecs;
using Play.Ber.InternalFactories;
using Play.Codecs;

using PlayEncodingId = Play.Ber.Codecs.PlayEncodingId;

namespace Play.Emv.Ber.Codecs;

// TODO: Move the actual functionality higher up to Play.Codec
public class VariableCodec : BerPrimitiveCodec
{
    #region Static Metadata

    private static readonly HexadecimalCodec _HexadecimalCodec = new();
    public static readonly PlayEncodingId EncodingId = GetEncodingId(typeof(VariableCodec));

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override bool IsValid(ReadOnlySpan<byte> value) => _HexadecimalCodec.IsValid(value);

    protected override void Validate(ReadOnlySpan<byte> value)
    {
        _HexadecimalCodec.IsValid(value);
    }

    public override byte[] Encode<T>(T value) => throw new NotImplementedException();
    public override byte[] Encode<T>(T value, int length) => throw new NotImplementedException();
    public override byte[] Encode<T>(T[] value) => _HexadecimalCodec.Encode(value);
    public override byte[] Encode<T>(T[] value, int length) => _HexadecimalCodec.Encode(value, length);
    public override ushort GetByteCount<T>(T value) => throw new NotImplementedException();
    public override ushort GetByteCount<T>(T[] value) => (ushort) (value.Length * Unsafe.SizeOf<T>());

    #endregion

    #region Serialization

    public override DecodedMetadata Decode(ReadOnlySpan<byte> value) => _HexadecimalCodec.Decode(value);

    #endregion
}