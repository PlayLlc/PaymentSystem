﻿using Play.Ber.Codecs;
using Play.Ber.InternalFactories;
using Play.Emv.Codecs;

namespace Play.Emv.Ber.Codecs;

// TODO: Move the actual functionality higher up to Play.Codec
public class AlphaNumericSpecialDataElementCodec : BerPrimitiveCodec
{
    #region Static Metadata

    private static readonly AlphaNumericSpecialEmvCodec _Codec = new();
    public static readonly BerEncodingId Identifier = GetBerEncodingId(typeof(AlphaNumericSpecialDataElementCodec));

    #endregion

    #region Instance Members

    public override BerEncodingId GetIdentifier() => Identifier;
    public override ushort GetByteCount<T>(T value) => _Codec.GetByteCount(value);
    public override ushort GetByteCount<T>(T[] value) => _Codec.GetByteCount(value);
    public override bool IsValid(ReadOnlySpan<byte> value) => _Codec.IsValid(value);
    public override byte[] Encode<T>(T value) => _Codec.Encode(value);
    public override byte[] Encode<T>(T value, int length) => _Codec.Encode(value);
    public override byte[] Encode<T>(T[] value) => _Codec.Encode(value);
    public override byte[] Encode<T>(T[] value, int length) => _Codec.Encode(value);
    public override void Encode<T>(T value, Span<byte> buffer, ref int offset) => _Codec.Encode(value, buffer, ref offset);

    public override void Encode<T>(T value, int length, Span<byte> buffer, ref int offset) =>
        _Codec.Encode(value, length, buffer, ref offset);

    public override void Encode<T>(T[] value, Span<byte> buffer, ref int offset) => _Codec.Encode(value, buffer, ref offset);

    public override void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset) =>
        _Codec.Encode(value, length, buffer, ref offset);

    #endregion

    #region Serialization

    public override DecodedMetadata Decode(ReadOnlySpan<byte> value) => _Codec.Decode(value);

    #endregion
}