using System;

using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Metadata;

namespace Play.Ber.Codecs;

public abstract class BerPrimitiveCodec : IPlayCodec
{
    #region Instance Members

    protected static BerEncodingId GetBerEncodingId(Type encoder) => new(encoder);

    /// <summary>
    ///     An method to get the Identifier for an instance of this class
    /// </summary>
    /// <returns></returns>
    public abstract BerEncodingId GetIdentifier();

    public abstract ushort GetByteCount<T>(T value) where T : struct;
    public abstract ushort GetByteCount<T>(T[] value) where T : struct;
    public abstract bool IsValid(ReadOnlySpan<byte> value);
    public abstract byte[] Encode<T>(T value) where T : struct;
    public abstract byte[] Encode<T>(T value, int length) where T : struct;
    public abstract byte[] Encode<T>(T[] value) where T : struct;
    public abstract byte[] Encode<T>(T[] value, int length) where T : struct;
    public abstract void Encode<T>(T value, Span<byte> buffer, ref int offset) where T : struct;
    public abstract void Encode<T>(T value, int length, Span<byte> buffer, ref int offset) where T : struct;
    public abstract void Encode<T>(T[] value, Span<byte> buffer, ref int offset) where T : struct;
    public abstract void Encode<T>(T[] value, int length, Span<byte> buffer, ref int offset) where T : struct;

    #endregion

    #region Serialization

    public abstract DecodedMetadata Decode(ReadOnlySpan<byte> value);

    #endregion
}