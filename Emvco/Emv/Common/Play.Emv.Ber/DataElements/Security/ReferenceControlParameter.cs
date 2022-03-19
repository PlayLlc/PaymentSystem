using Play.Ber.Codecs;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements
{
    /// <summary>
    ///     Working variable to store the reference control parameter of the GENERATE AC command.
    /// </summary>
    public record ReferenceControlParameter : DataElement<byte>, IEqualityComparer<ReferenceControlParameter>
    {
        #region Static Metadata

        public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
        public static readonly Tag Tag = 0xDF8114;
        private const byte _ByteLength = 1;

        #endregion

        #region Constructor

        /// <exception cref="CardDataException"></exception>
        public ReferenceControlParameter(byte value) : base(value)
        {
            if (!CryptogramTypes.IsValid(value))
                throw new CardDataException($"The argument {nameof(value)} was not recognized as a valid {nameof(CryptogramTypes)}");
        }

        #endregion

        #region Instance Members

        private static byte Create(CryptogramTypes cryptogramTypes, bool isCombinedDataAuthenticationSupported)
        {
            if (isCombinedDataAuthenticationSupported)
                return (byte) (cryptogramTypes | (byte) Bits.Five);

            return (byte) cryptogramTypes;
        }

        /// <summary>
        ///     GetCryptogramType
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="DataElementParsingException"></exception>
        public CryptogramTypes GetCryptogramType()
        {
            if (!CryptogramTypes.TryGet(_Value, out CryptogramTypes? result))
            {
                throw new
                    DataElementParsingException($"The {nameof(CryptogramInformationData)} expected a {nameof(CryptogramTypes)} but none could be found");
            }

            return result!;
        }

        public override PlayEncodingId GetEncodingId() => EncodingId;
        public override Tag GetTag() => Tag;
        public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

        #endregion

        #region Serialization

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
        public static ReferenceControlParameter Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
        /// <exception cref="CardDataException"></exception>
        public static ReferenceControlParameter Decode(ReadOnlySpan<byte> value)
        {
            Check.Primitive.ForExactLength(value, _ByteLength, Tag);

            byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

            return new ReferenceControlParameter(result);
        }

        public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
        public new byte[] EncodeValue(int length) => EncodeValue();

        #endregion

        #region Equality

        public bool Equals(ReferenceControlParameter? x, ReferenceControlParameter? y)
        {
            if (x is null)
                return y is null;

            if (y is null)
                return false;

            return x.Equals(y);
        }

        public int GetHashCode(ReferenceControlParameter obj) => obj.GetHashCode();

        #endregion
    }
}