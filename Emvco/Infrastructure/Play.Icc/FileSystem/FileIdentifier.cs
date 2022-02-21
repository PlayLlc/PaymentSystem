using System;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Core.Specifications;

namespace Play.Icc.FileSystem;

public record FileIdentifier : PrimitiveValue
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = BinaryIntegerCodec.Identifier;
    public static readonly FileIdentifier CurrentDedicatedFile = new(new byte[] {0x3F, 0xFF});
    public static readonly FileIdentifier MasterFile = new(new byte[] {0x3F, 0x00});
    public static readonly uint Tag = 0x81;

    #endregion

    #region Instance Values

    private readonly ushort _Value;

    #endregion

    #region Constructor

    public FileIdentifier(ushort value)
    {
        _Value = value;
    }

    public FileIdentifier(byte[] value)
    {
        _Value = PlayEncoding.UnsignedInteger.GetUInt16(value);
    }

    #endregion

    #region Instance Members

    public byte[] AsByteArray() => PlayEncoding.UnsignedInteger.GetBytes(_Value);

    public byte[] AsRawTlv()
    {
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(GetTlvByteCount());
        Span<byte> buffer = spanOwner.Span;

        PlayEncoding.UnsignedBinary.GetBytes(Tag).CopyTo(buffer);
        PlayEncoding.UnsignedBinary.GetBytes(Specs.Integer.Int16.ByteCount).CopyTo(buffer[1..]);
        PlayEncoding.UnsignedInteger.GetBytes(_Value).AsSpan().CopyTo(buffer[^Specs.Integer.Int16.ByteCount..]);

        return buffer.ToArray();
    }

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    private int GetTlvByteCount() => 4;
    public override ushort GetValueByteCount(BerCodec codec) => Specs.Integer.Int16.ByteCount;

    #endregion

    #region Serialization

    /// <summary>
    ///     Decode
    /// </summary>
    /// <param name="codec"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static FileIdentifier Decode(BerCodec codec, ReadOnlySpan<byte> value)
    {
        DecodedResult<ushort> result = codec.Decode(BerEncodingId, value) as DecodedResult<ushort>
            ?? throw new InvalidOperationException(
                $"The {nameof(FileIdentifier)} could not be initialized because the {nameof(BinaryIntegerCodec)} returned a null {nameof(DecodedResult<ushort>)}");

        return new FileIdentifier(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(BerEncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(BerEncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(FileIdentifier? x, FileIdentifier? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(FileIdentifier obj) => obj.GetHashCode();

    #endregion
}