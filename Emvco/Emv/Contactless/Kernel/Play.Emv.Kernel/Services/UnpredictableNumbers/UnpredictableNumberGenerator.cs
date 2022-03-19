using Play.Emv.Ber.DataElements;
using Play.Randoms;

namespace Play.Emv.Kernel.Services;

internal class UnpredictableNumberGenerator : IGenerateUnpredictableNumber
{
    #region Instance Members

    // WARNING: This should be generated using a device that can ensure total uniqueness, like a DUKPT compliant machine
    public UnpredictableNumber GenerateUnpredictableNumber() => new(Randomize.Numeric.UInt());

    #endregion
}