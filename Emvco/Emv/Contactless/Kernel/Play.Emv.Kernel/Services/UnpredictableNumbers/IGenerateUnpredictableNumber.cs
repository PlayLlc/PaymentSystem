using Play.Emv.DataElements;

namespace Play.Emv.Terminal.Contracts;

public interface IGenerateUnpredictableNumber
{
    public UnpredictableNumber GenerateUnpredictableNumber();
}