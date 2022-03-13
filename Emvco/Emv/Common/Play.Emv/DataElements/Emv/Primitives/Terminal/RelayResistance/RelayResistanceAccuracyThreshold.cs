using System;

using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements
{
    /// <summary>
    ///     Represents the threshold above which the Kernel considers the variation between Measured Relay Resistance
    ///     Processing Time and Min Time For Processing Relay Resistance APDU no longer acceptable. The Relay Resistance
    ///     Accuracy Threshold is expressed in units of hundreds of microseconds.
    /// </summary>
    public record RelayResistanceAccuracyThreshold : DataElement<ushort>
    {
        #region Static Metadata

        public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
        public static readonly Tag Tag = 0xDF8136;
        private const byte _ByteLength = 2;

        #endregion

        #region Constructor

        public RelayResistanceAccuracyThreshold(ushort value) : base(value)
        { }

        #endregion

        #region Instance Members

        public override PlayEncodingId GetEncodingId() => EncodingId;
        public override Tag GetTag() => Tag;

        #endregion

        #region Serialization

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
        public static RelayResistanceAccuracyThreshold Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
        public static RelayResistanceAccuracyThreshold Decode(ReadOnlySpan<byte> value)
        {
            Check.Primitive.ForExactLength(value, _ByteLength, Tag);

            ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

            return new RelayResistanceAccuracyThreshold(result);
        }

        public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
        public new byte[] EncodeValue(int length) => EncodeValue();

        #endregion
    }
}