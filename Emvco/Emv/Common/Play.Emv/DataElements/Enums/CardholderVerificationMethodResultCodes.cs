using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.DataElements
{
    public record CvmResultCodes : EnumObject<byte>
    {
        #region Static Metadata

        /// <remarks>Hex: 0x00; Decimal: 0</remarks>
        public static readonly CvmResultCodes Unknown;

        /// <remarks>Hex: 0x01; Decimal: 1</remarks>
        public static readonly CvmResultCodes Failed;

        /// <remarks>Hex: 0x02; Decimal: 2</remarks>
        public static readonly CvmResultCodes Successful;

        #endregion

        #region Constructor

        static CvmResultCodes()
        {
            Unknown = new CvmResultCodes(0);
            Failed = new CvmResultCodes(1);
            Successful = new CvmResultCodes(2);
        }

        private CvmResultCodes(byte value) : base(value)
        { }

        #endregion
    }
}