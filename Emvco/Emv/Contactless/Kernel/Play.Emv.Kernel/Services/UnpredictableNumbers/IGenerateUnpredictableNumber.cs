using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Kernel.Services;

public interface IGenerateUnpredictableNumber
{
    #region Instance Members

    public UnpredictableNumber GenerateUnpredictableNumber();
    public UnpredictableNumber GenerateUnpredictableNumber(NumberOfNonZeroBits nun);

    #endregion
}