using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements
{

    /// <summary>
    /// The Payment Account Reference is a data object associated with an Application PAN. It allows acquirers and merchants to link transactions, whether tokenised or not, that are associated to the same underlying Application PAN. Lower case alphabetic characters are not permitted for the Payment Account Reference, however the Kernel is not expected to check this. 
    /// </summary>
    public record PaymentAccountReference : DataElement<char[]>, IEqualityComparer<PaymentAccountReference>
    {
        #region Static Metadata

        public static readonly PlayEncodingId EncodingId = AlphaNumericCodec.EncodingId;
        public static readonly Tag Tag = 0x9F24;
        private const byte _ByteLength = 29;

        #endregion

        public static explicit operator ReadOnlySpan<char>(PaymentAccountReference value) => value._Value.AsSpan();
        #region Constructor

        public PaymentAccountReference(char[] value) : base(value)
        { }

        #endregion

        #region Instance Members

        public override PlayEncodingId GetEncodingId() => EncodingId;
        public override Tag GetTag() => Tag;
        public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(EncodingId, _Value);

        #endregion

        #region Serialization

        /// <exception cref="BerParsingException"></exception>
        public static PaymentAccountReference Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="BerParsingException"></exception>
        public static PaymentAccountReference Decode(ReadOnlySpan<byte> value)
        {
            Check.Primitive.ForExactLength(value, _ByteLength, Tag);

            char[] result = PlayCodec.AlphaNumericCodec.DecodeToChars(value);

            return new PaymentAccountReference(result);
        }

        public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(EncodingId, _Value);
        public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(EncodingId, _Value, length);

        #endregion

        #region Equality

        public bool Equals(PaymentAccountReference? x, PaymentAccountReference? y)
        {
            if (x is null)
                return y is null;

            if (y is null)
                return false;

            return x.Equals(y);
        }

        public int GetHashCode(PaymentAccountReference obj) => obj.GetHashCode();

        #endregion

        #region Operator Overrides


        #endregion
    }
}
