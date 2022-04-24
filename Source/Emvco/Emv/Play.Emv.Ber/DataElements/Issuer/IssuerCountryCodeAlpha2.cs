using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Country;

namespace Play.Emv.Ber.DataElements
{
    /// <summary>
    ///     Indicates the country of the issuer as defined in ISO 3166 (using a 2 character alphabetic code)
    /// </summary>
    public record IssuerCountryCodeAlpha2 : DataElement<Alpha2CountryCode>, IEqualityComparer<IssuerCountryCodeAlpha2>
    {
        #region Static Metadata

        public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
        public static readonly Tag Tag = 0x5F55;
        private const byte _ByteLength = 2;

        #endregion

        #region Constructor

        public IssuerCountryCodeAlpha2(Alpha2CountryCode value) : base(value)
        { }

        #endregion

        #region Instance Members

        public override PlayEncodingId GetEncodingId() => EncodingId;
        public override Tag GetTag() => Tag;

        public static bool StaticEquals(IssuerCountryCodeAlpha2? x, IssuerCountryCodeAlpha2? y)
        {
            if (x is null)
                return y is null;

            if (y is null)
                return false;

            return x.Equals(y);
        }

        #endregion

        #region Serialization

        /// <exception cref="CodecParsingException"></exception>
        public static IssuerCountryCodeAlpha2 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

        public override IssuerCountryCodeAlpha2 Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

        /// <exception cref="CodecParsingException"></exception>
        public static IssuerCountryCodeAlpha2 Decode(ReadOnlySpan<byte> value)
        {
            Check.Primitive.ForExactLength(value, _ByteLength, Tag);

            return new IssuerCountryCodeAlpha2(new Alpha2CountryCode(PlayCodec.AlphabeticCodec.DecodeToChars(value)));
        }

        public override byte[] EncodeValue() => PlayCodec.NumericCodec.Encode(_Value, _ByteLength);
        public override byte[] EncodeValue(int length) => PlayCodec.NumericCodec.Encode(_Value, length);

        #endregion

        #region Equality

        public bool Equals(IssuerCountryCodeAlpha2? x, IssuerCountryCodeAlpha2? y)
        {
            if (x is null)
                return y is null;

            if (y is null)
                return false;

            return x.Equals(y);
        }

        public int GetHashCode(IssuerCountryCodeAlpha2 obj) => obj.GetHashCode();

        #endregion

        #region Operator Overrides

        public static implicit operator Alpha2CountryCode(IssuerCountryCodeAlpha2 value) => value._Value;

        #endregion
    }
}