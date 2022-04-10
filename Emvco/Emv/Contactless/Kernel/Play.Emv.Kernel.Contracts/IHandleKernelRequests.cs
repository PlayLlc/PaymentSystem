namespace Play.Emv.Kernel.Contracts;

public interface IHandleKernelRequests : IHandleKernelStopRequests
{
    #region Instance Members

    /// <summary>
    ///     An ACT DataExchangeSignal. Through its interaction with the Card, it creates a transaction record for authorization
    ///     and/or clearing.authorization and/or clearing.
    /// </summary>
    /// <param name="message"></param>
    public void Request(ActivateKernelRequest message);

    /// <summary>
    ///     It performs house-keeping by removing torn transactions from the Torn Transaction Log that have aged off without
    ///     having been recovered.The Torn Transaction Log is the repository in which the Kernel stores information on torn
    ///     transactions. More information on torn transactions and the Torn Transaction Log is provided in section 3.8 of Book
    ///     C-2
    /// </summary>
    public void Request(CleanKernelRequest message);

    public void Request(QueryKernelRequest message);
    public void Request(UpdateKernelRequest message);

    #endregion
}