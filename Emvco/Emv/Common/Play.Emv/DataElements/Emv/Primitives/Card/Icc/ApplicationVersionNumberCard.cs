using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.Codecs;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements.Emv.Primitives.Card.Icccc
{
    /// <summary>
    ///     Version number assigned by the payment system for the application in the Card.
    /// </summary>
    public record ApplicationVersionNumberCard : DataElement<ushort>
    {
        #region Static Metadata

        public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
        public static readonly Tag Tag = 0x9F08;
        private const byte _ByteLength = 2;

        #endregion

        #region Constructor

        public ApplicationVersionNumberCard(ushort value) : base(value)
        { }

        #endregion

        #region Instance Members

        public override Tag GetTag() => Tag;
        public override PlayEncodingId GetEncodingId() => EncodingId;

        #endregion

        #region Serialization

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
        public static ApplicationVersionNumberCard Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
        public static ApplicationVersionNumberCard Decode(ReadOnlySpan<byte> value)
        {
            Check.Primitive.ForExactLength(value, _ByteLength, Tag);

            ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

            return new ApplicationVersionNumberCard(result);
        }

        public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
        public new byte[] EncodeValue(int length) => EncodeValue();

        #endregion

        #region Operator Overrides

        public static explicit operator ushort(ApplicationVersionNumberCard value) => value._Value;

        #endregion
    }
}