using System;

using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements
{
    /// <summary>
    ///     The Minimum Relay Resistance Grace Period and Maximum Relay Resistance Grace Period represent how far outside the
    ///     window defined by the Card that the measured time may be and yet still be considered acceptable. The Minimum Relay
    ///     Resistance Grace Period is expressed in units of hundreds of microseconds.
    /// </summary>
    public record MinimumRelayResistanceGracePeriod : DataElement<ushort>
    {
        #region Static Metadata

        public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
        public static readonly Tag Tag = 0xDF8132;
        private const byte _ByteLength = 2;

        #endregion

        #region Constructor

        public MinimumRelayResistanceGracePeriod(ushort value) : base(value)
        { }

        #endregion

        #region Instance Members

        public override PlayEncodingId GetEncodingId() => EncodingId;
        public override Tag GetTag() => Tag;

        #endregion

        #region Serialization

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
        public static MinimumRelayResistanceGracePeriod Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
        public static MinimumRelayResistanceGracePeriod Decode(ReadOnlySpan<byte> value)
        {
            Check.Primitive.ForExactLength(value, _ByteLength, Tag);

            ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

            return new MinimumRelayResistanceGracePeriod(result);
        }

        public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
        public new byte[] EncodeValue(int length) => EncodeValue();

        #endregion
    }
}