using System.Threading.Tasks;

namespace Play.Emv.Pcd.Contracts;

public interface ITransceiveDataBatches<in T, TK> where TK : QueryPcdResponse where T : QueryPcdRequest
{
    #region Instance Members

    /// <summary>
    ///     CA(C-APDU) DataExchangeSignal. Send a C-APDU to the Card and return either an R-APDU or an error indication. The
    ///     parameter to the DataExchangeSignal
    ///     is the command to be sent to the Card
    /// </summary>
    public Task<TK> Transceive(T command);

    #endregion
}