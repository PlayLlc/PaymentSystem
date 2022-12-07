using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements
{
    /// <summary>
    ///     Issuer-specified preference for the maximum number of consecutive offline transactions for this ICC application
    ///     allowed in a terminal without online capability
    /// </summary>
    public record UpperConsecutiveOfflineLimit : DataElement<byte>
    {
        #region Static Metadata

        public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
        public static readonly Tag Tag = 0x9F23;
        private const byte _ByteLength = 1;

        #endregion

        #region Constructor

        public UpperConsecutiveOfflineLimit(byte value) : base(value)
        { }

        #endregion

        #region Instance Members

        public override PlayEncodingId GetEncodingId() => EncodingId;
        public override Tag GetTag() => Tag;

        #endregion

        #region Serialization

        public static UpperConsecutiveOfflineLimit Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
        public override UpperConsecutiveOfflineLimit Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="BerParsingException"></exception>
        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="CodecParsingException"></exception>
        public static UpperConsecutiveOfflineLimit Decode(ReadOnlySpan<byte> value)
        {
            Check.Primitive.ForExactLength(value, _ByteLength, Tag);

            return new UpperConsecutiveOfflineLimit(value[0]);
        }

        public override byte[] EncodeValue() => PlayCodec.BinaryCodec.Encode(_Value);

        #endregion

        #region Equality

        public bool Equals(UpperConsecutiveOfflineLimit? x, UpperConsecutiveOfflineLimit? y)
        {
            if (x is null)
                return y is null;

            if (y is null)
                return false;

            return x.Equals(y);
        }

        public int GetHashCode(UpperConsecutiveOfflineLimit obj) => obj.GetHashCode();

        #endregion

        #region Operator Overrides

        public static implicit operator byte(UpperConsecutiveOfflineLimit value) => value._Value;

        #endregion
    }
}