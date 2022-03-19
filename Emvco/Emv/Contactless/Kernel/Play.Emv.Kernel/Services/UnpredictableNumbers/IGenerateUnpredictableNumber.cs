using Play.Emv.Ber.DataElements;

namespace Play.Emv.Kernel.Services;

public interface IGenerateUnpredictableNumber
{
    public UnpredictableNumber GenerateUnpredictableNumber();
}