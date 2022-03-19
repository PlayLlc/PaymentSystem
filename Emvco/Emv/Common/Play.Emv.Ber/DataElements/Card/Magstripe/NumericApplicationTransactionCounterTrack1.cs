using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements
{
    /// <summary>
    /// The value of NATC(Track1) represents the number of digits of the Application Transaction Counter to be included in the discretionary data field of Track 1 Data. 
    /// </summary>
    public record NumericApplicationTransactionCounterTrack1 : DataElement<ulong>
    {
        #region Static Metadata

        public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
        public static readonly Tag Tag = 0x9F64;
        private const byte _ByteLength = 1;

        #endregion

        #region Constructor

        public NumericApplicationTransactionCounterTrack1(ulong value) : base(value)
        { }

        #endregion

        #region Instance Members

        public override PlayEncodingId GetEncodingId() => EncodingId;
        public override Tag GetTag() => Tag;

        #endregion

        #region Serialization

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
        public static NumericApplicationTransactionCounterTrack1 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
        public static NumericApplicationTransactionCounterTrack1 Decode(ReadOnlySpan<byte> value)
        {
            Check.Primitive.ForExactLength(value, _ByteLength, Tag);

            ulong result = PlayCodec.BinaryCodec.DecodeToUInt64(value);

            return new NumericApplicationTransactionCounterTrack1(result);
        }

        public new byte[] EncodeValue() => EncodeValue(_ByteLength);

        #endregion
    }
}
