using System;

using Play.Codecs;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Randoms;

namespace Play.Emv.Kernel.Services;

internal class UnpredictableNumberGenerator : IGenerateUnpredictableNumber
{
    #region Instance Members

    // WARNING: This should be generated using a device that can ensure total uniqueness, like a DUKPT compliant machine
    public UnpredictableNumber GenerateUnpredictableNumber() => new(Randomize.Numeric.UInt());

    // WARNING: This should be generated using a device that can ensure total uniqueness, like a DUKPT compliant machine
    public UnpredictableNumber GenerateUnpredictableNumber(NumberOfNonZeroBits nun)
    {
        UnpredictableNumber offset = GenerateUnpredictableNumber();
        uint rightPaddedZeroDigitCount = (uint) (8 - nun);
        uint rightPaddedResult = (uint) ((uint) ((uint) offset / Math.Pow(10, rightPaddedZeroDigitCount))
            * Math.Pow(10, rightPaddedZeroDigitCount));

        return UnpredictableNumber.Decode(PlayCodec.CompressedNumericCodec.Encode(rightPaddedResult).AsSpan());
    }

    #endregion
}