using Play.Ber.Codecs;
using Play.Ber.Emv.Codecs;
using Play.Ber.Emv.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements
{
    /// <summary>
    ///     PUNATC(Track1) indicates to the Kernel the positions in the discretionary data field of Track 1 Data where the
    ///     Unpredictable Number (Numeric) digits and Application Transaction Counter digits have to be copied.
    /// </summary>
    public record PunatcTrack1 : DataElement<ulong>
    {
        #region Static Metadata

        public static readonly BerEncodingId BerEncodingId = UnsignedBinaryCodec.Identifier;

        public static readonly Tag Tag = 0x9F63;
        private const byte _ByteLength = 6;

        #endregion

        #region Constructor

        public PunatcTrack1(ulong value) : base(value)
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

        #endregion

        #region Serialization

        public static PunatcTrack1 Decode(ReadOnlyMemory<byte> value)
        {
            return Decode(value.Span);
        }

        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="BerException"></exception>
        public static PunatcTrack1 Decode(ReadOnlySpan<byte> value)
        {
            Check.Primitive.ForMaximumLength(value, _ByteLength, Tag);

            DecodedResult<ulong> result = _Codec.Decode(BerEncodingId, value).ToUInt64Result()
                ?? throw new DataElementNullException(BerEncodingId);

            return new PunatcTrack1(result.Value);
        }

        public new byte[] EncodeValue()
        {
            return EncodeValue(_ByteLength);
        }

        #endregion
    }
}