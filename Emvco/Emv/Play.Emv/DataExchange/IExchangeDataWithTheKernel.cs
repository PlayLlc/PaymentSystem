﻿namespace Play.Emv.DataExchange;

/// <summary>
///     Request and Response pairs implement this interface when a data exchange request originates from the Kernel
/// </summary>
public interface IExchangeDataWithTheKernel
{
    public DataExchangeKernelId GetDataExchangeKernelId();
}