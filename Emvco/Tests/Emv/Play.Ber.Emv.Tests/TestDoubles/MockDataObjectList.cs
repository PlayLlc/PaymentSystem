using System;

using Play.Ber.Codecs;
using Play.Ber.Emv.Codecs;
using Play.Ber.Emv.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;

namespace Play.Ber.Emv.Tests.TestDoubles
{
    public record MockDataObjectList : DataObjectList
    {
        #region Static Metadata

        public static readonly BerEncodingId BerEncodingId = UnsignedBinaryCodec.Identifier;
        public static readonly Tag Tag = 0x9F38;

        #endregion

        #region Constructor

        public MockDataObjectList(byte[] value) : base(value)
        { }

        #endregion

        #region Instance Members

        public override BerEncodingId GetBerEncodingId()
        {
            return BerEncodingId;
        }

        public override Tag GetTag()
        {
            return Tag;
        }

        public override ushort GetValueByteCount(BerCodec codec)
        {
            return codec.GetByteCount(GetBerEncodingId(), _Value);
        }

        #endregion

        #region Serialization

        public static MockDataObjectList Decode(ReadOnlyMemory<byte> value)
        {
            return Decode(value.Span);
        }

        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="BerException"></exception>
        public static MockDataObjectList Decode(ReadOnlySpan<byte> value)
        {
            return new MockDataObjectList(value.ToArray());
        }

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

        public int GetHashCode(MockDataObjectList obj)
        {
            return obj.GetHashCode();
        }

        #endregion
    }
}