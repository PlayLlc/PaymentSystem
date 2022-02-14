using System;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Ber.Emv.Tests.TestDoubles;

public record MockDataObjectList : DataObjectList
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = BinaryCodec.Identifier;
    public static readonly Tag Tag = 0x9F38;

    #endregion

    #region Constructor

    public MockDataObjectList(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);

    #endregion

    #region Serialization

    public static MockDataObjectList Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static MockDataObjectList Decode(ReadOnlySpan<byte> value) => new(value.ToArray());

    #endregion

    #region Equality

    public bool Equals(MockDataObjectList? x, MockDataObjectList? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(MockDataObjectList obj) => obj.GetHashCode();

    #endregion
}