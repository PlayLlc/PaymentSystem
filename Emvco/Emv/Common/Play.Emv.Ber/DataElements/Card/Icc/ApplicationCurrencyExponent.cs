using Play.Ber.Codecs;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements
{
    /// <summary>
    ///     Indicates the implied position of the decimal point from the right of the amount represented in accordance with[ISO
    ///     4217]
    /// </summary>
    public record ApplicationCurrencyExponent : DataElement<byte>, IEqualityComparer<ApplicationCurrencyExponent>
    {
        #region Static Metadata

        public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
        public static readonly Tag Tag = 0x9F44;
        private const byte _ByteLength = 1;
        private const byte _CharLength = 1;

        #endregion

        #region Constructor

        public ApplicationCurrencyExponent(byte value) : base(value)
        { }

        #endregion

        #region Instance Members

        public override PlayEncodingId GetEncodingId() => EncodingId;
        public override Tag GetTag() => Tag;
        public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

        #endregion

        #region Serialization

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="CodecParsingException"></exception>
        public static ApplicationCurrencyExponent Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="CodecParsingException"></exception>
        public static ApplicationCurrencyExponent Decode(ReadOnlySpan<byte> value)
        {
            Check.Primitive.ForExactLength(value, _ByteLength, Tag);

            byte result = PlayCodec.NumericCodec.DecodeToByte(value);

            Check.Primitive.ForMaxCharLength(result.GetNumberOfDigits(), _CharLength, Tag);

            return new ApplicationCurrencyExponent(result);
        }

        public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
        public new byte[] EncodeValue(int length) => EncodeValue();

        #endregion

        #region Equality

        public bool Equals(ApplicationCurrencyExponent? x, ApplicationCurrencyExponent? y)
        {
            if (x is null)
                return y is null;

            if (y is null)
                return false;

            return x.Equals(y);
        }

        public int GetHashCode(ApplicationCurrencyExponent obj) => obj.GetHashCode();

        #endregion
    }
}