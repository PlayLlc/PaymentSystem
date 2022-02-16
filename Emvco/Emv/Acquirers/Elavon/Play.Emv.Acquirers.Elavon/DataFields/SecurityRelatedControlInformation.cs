using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Codecs;
using Play.Core.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Codecs;

namespace Play.Emv.Acquirers.Elavon.DataFields;

public class DeleteMe
{ }

public record SecurityRelatedControlInformation
{
    #region Instance Members

    public static void Encode(KeyType keyType, KeyIdentifier keyIdentifier, Span<byte> buffer, ref int offset)
    {
        keyType.Encode(buffer, ref offset);
        keyIdentifier.Encode(buffer, ref offset);
    }

    #endregion

    public class KeyType
    {
        #region Static Metadata

        public static readonly KeyType Tpk = new(PlayEncoding.AlphaNumeric.GetBytes("01"));
        public static readonly KeyType Zpk = new(PlayEncoding.AlphaNumeric.GetBytes("02"));

        #endregion

        #region Instance Values

        private readonly byte[] _Value;

        #endregion

        #region Constructor

        private KeyType(byte[] value)
        {
            _Value = value;
        }

        #endregion

        #region Instance Members

        public void Encode(Span<byte> buffer, ref int offset)
        {
            _Value.CopyTo(buffer[offset..]);
        }

        #endregion
    }

    public readonly record struct KeyIdentifier
    {
        #region Static Metadata

        private const byte _MinValue = 3;
        private const byte _MaxValue = 32;

        #endregion

        #region Instance Values

        private readonly byte _Value;

        #endregion

        #region Constructor

        /// <summary>
        /// </summary>
        /// <param name="value">Minimum Value: 3; Maximum Value: 32</param>
        public KeyIdentifier(byte value)
        {
            CheckCore.ForRange(value, _MaxValue, _MinValue, nameof(KeyIdentifier));

            _Value = value;
        }

        #endregion

        #region Instance Members

        public void Encode(Span<byte> buffer, ref int offset)
        {
            buffer[offset++] = (byte) (_Value / 10);
            buffer[offset++] = (byte) (_Value % 10);
        }

        #endregion
    }
}