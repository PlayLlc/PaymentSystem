﻿using Play.Emv.DataElements;

namespace Play.Emv.Kernel.Services;

public interface IGenerateUnpredictableNumber
{
    public UnpredictableNumber GenerateUnpredictableNumber();
}