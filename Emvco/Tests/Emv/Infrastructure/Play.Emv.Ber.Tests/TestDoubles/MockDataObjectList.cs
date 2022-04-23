using System;
using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Ber.Tests.TestDoubles;

public record MockDataObjectList : DataObjectList
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F38;

    #endregion

    #region Constructor

    public MockDataObjectList(params TagLength[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static MockDataObjectList Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static MockDataObjectList Decode(ReadOnlySpan<byte> value) => new(EmvCodec.GetBerCodec().DecodeTagLengthPairs(value.ToArray()));

    public override MockDataObjectList Decode(TagLengthValue value) => new(Decode(value.EncodeValue().AsSpan()));

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