using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements
{
    /// <summary>
    ///     Issuer-specified preference for the minimum number of consecutive offline transactions for this ICC application
    ///     allowed in a terminal with online capability
    /// </summary>
    public record LowerConsecutiveOfflineLimit : DataElement<byte>
    {
        #region Static Metadata

        public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
        public static readonly Tag Tag = 0x9F14;
        private const byte _ByteLength = 1;

        #endregion

        #region Constructor

        public LowerConsecutiveOfflineLimit(byte value) : base(value)
        { }

        #endregion

        #region Instance Members

        public override PlayEncodingId GetEncodingId() => EncodingId;
        public override Tag GetTag() => Tag;

        #endregion

        #region Serialization

        public static LowerConsecutiveOfflineLimit Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
        public override LowerConsecutiveOfflineLimit Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="BerParsingException"></exception>
        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="CodecParsingException"></exception>
        public static LowerConsecutiveOfflineLimit Decode(ReadOnlySpan<byte> value)
        {
            Check.Primitive.ForExactLength(value, _ByteLength, Tag);

            return new LowerConsecutiveOfflineLimit(value[0]);
        }

        public override byte[] EncodeValue() => PlayCodec.BinaryCodec.Encode(_Value);

        #endregion

        #region Operator Overrides

        public static implicit operator byte(LowerConsecutiveOfflineLimit value) => value._Value;

        #endregion
    }
}