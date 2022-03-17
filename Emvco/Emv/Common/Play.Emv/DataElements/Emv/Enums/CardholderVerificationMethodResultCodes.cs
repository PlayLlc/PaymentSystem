using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.DataElements
{
    public record CardholderVerificationMethodResultCodes : EnumObject<byte>
    {
        #region Static Metadata

        /// <remarks>Hex: 0x00; Decimal: 0</remarks>
        public static readonly CardholderVerificationMethodResultCodes Unknown;

        /// <remarks>Hex: 0x01; Decimal: 1</remarks>
        public static readonly CardholderVerificationMethodResultCodes Failed;

        /// <remarks>Hex: 0x02; Decimal: 2</remarks>
        public static readonly CardholderVerificationMethodResultCodes Successful;

        #endregion

        #region Constructor

        static CardholderVerificationMethodResultCodes()
        {
            Unknown = new CardholderVerificationMethodResultCodes(0);
            Failed = new CardholderVerificationMethodResultCodes(1);
            Successful = new CardholderVerificationMethodResultCodes(2);
        }

        private CardholderVerificationMethodResultCodes(byte value) : base(value)
        { }

        #endregion
    }
}