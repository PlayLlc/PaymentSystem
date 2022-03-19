using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.DataElements;
using Play.Emv.Exceptions;

namespace Play.Emv.Ber.DataElements
{
    /// <summary>
    /// Version number assigned by the payment system for the specific mag-stripe mode functionality of the Kernel.
    /// </summary>
    public record MagstripeApplicationVersionNumberReader : DataElement<RelaySeconds>
    {
        #region Static Metadata

        public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
        public static readonly Tag Tag = 0xDF8305;
        private const byte _ByteLength = 2;

        #endregion

        #region Constructor

        public MagstripeApplicationVersionNumberReader(RelaySeconds value) : base(value)
        { }

        #endregion

        #region Instance Members

        public override PlayEncodingId GetEncodingId() => EncodingId;
        public override Tag GetTag() => Tag;

        #endregion

        #region Serialization

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
        public static MagstripeApplicationVersionNumberReader Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
        public static MagstripeApplicationVersionNumberReader Decode(ReadOnlySpan<byte> value)
        {
            Check.Primitive.ForExactLength(value, _ByteLength, Tag);

            ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

            return new MagstripeApplicationVersionNumberReader(result);
        }

        public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
        public new byte[] EncodeValue(int length) => EncodeValue();

        #endregion

        #region Operator Overrides

        public static implicit operator RelaySeconds(MagstripeApplicationVersionNumberReader value) => value._Value;

        #endregion
    }
}
