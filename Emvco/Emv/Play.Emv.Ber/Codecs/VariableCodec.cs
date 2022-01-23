using System.Runtime.CompilerServices;

using Play.Ber.Codecs;
using Play.Ber.InternalFactories;

namespace Play.Ber.Emv.Codecs;

// TODO: Move the actual functionality higher up to Play.Codec
public class VariableCodec : BerPrimitiveCodec
{
    #region Static Metadata

    private static readonly OctetStringCodec _OctetStringCodec = new();
    public static readonly BerEncodingId Identifier = GetBerEncodingId(typeof(VariableCodec));

    #endregion

    #region Instance Members

    public override BerEncodingId GetIdentifier()
    {
        return Identifier;
    }

    public override bool IsValid(ReadOnlySpan<byte> value)
    {
        return _OctetStringCodec.IsValid(value);
    }

    protected override void Validate(ReadOnlySpan<byte> value)
    {
        _OctetStringCodec.IsValid(value);
    }

    public override byte[] Encode<T>(T value)
    {
        throw new NotImplementedException();
    }

    public override byte[] Encode<T>(T value, int length)
    {
        throw new NotImplementedException();
    }

    public override byte[] Encode<T>(T[] value)
    {
        return _OctetStringCodec.Encode(value);
    }

    public override byte[] Encode<T>(T[] value, int length)
    {
        return _OctetStringCodec.Encode(value, length);
    }

    public override ushort GetByteCount<T>(T value)
    {
        throw new NotImplementedException();
    }

    public override ushort GetByteCount<T>(T[] value)
    {
        return (ushort) (value.Length * Unsafe.SizeOf<T>());
    }

    #endregion

    #region Serialization

    public override DecodedMetadata Decode(ReadOnlySpan<byte> value)
    {
        return _OctetStringCodec.Decode(value);
    }

    #endregion
}