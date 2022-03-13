using System;

using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Emv.ValueTypes;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements
{
    /// <summary>
    ///     Contains a Kernel challenge (random) to be used in the value field of the EXCHANGE RELAY RESISTANCE DATA command.
    /// </summary>
    public record TerminalRelayResistanceEntropy : DataElement<RelaySeconds>
    {
        #region Static Metadata

        public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
        public static readonly Tag Tag = 0xDF8301;
        private const byte _ByteLength = 4;

        #endregion

        #region Constructor

        public TerminalRelayResistanceEntropy(RelaySeconds value) : base(value)
        { }

        #endregion

        #region Instance Members

        public override PlayEncodingId GetEncodingId() => EncodingId;
        public override Tag GetTag() => Tag;

        #endregion

        #region Serialization

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
        public static TerminalRelayResistanceEntropy Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
        public static TerminalRelayResistanceEntropy Decode(ReadOnlySpan<byte> value)
        {
            Check.Primitive.ForExactLength(value, _ByteLength, Tag);

            ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

            return new TerminalRelayResistanceEntropy(result);
        }

        public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
        public new byte[] EncodeValue(int length) => EncodeValue();

        #endregion

        #region Operator Overrides

        public static explicit operator RelaySeconds(TerminalRelayResistanceEntropy value) => value._Value;

        #endregion
    }
}