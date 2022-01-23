using System.Threading.Tasks;

namespace Play.Emv.Pcd.Contracts;

// TODO: This needs to abstract the PCD layer. We're returning templates at this level, not RApdu
// TODO: Maybe instead of one card client to rule them all, we have individual services for each
// TODO: functionality the card exposes. That would allow us to return an abstract template
// TODO: but we would have to be able to have knowledge of the concrete template so that won't work.
// TODO: 
public interface ITransceiveData<in T, TK> where TK : QueryPcdResponse where T : QueryPcdRequest
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