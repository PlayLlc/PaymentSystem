using System.Threading.Tasks;

using Play.Emv.Icc;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd.GetData;

public class DataBatchReader : IGetData
{
    #region Instance Values

    private readonly IPcdTransceiver _Client;

    #endregion

    #region Constructor

    public DataBatchReader(IPcdTransceiver cardClient)
    {
        _Client = cardClient;
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Transceive
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="PcdProtocolException"></exception>
    /// <exception cref="PcdTimeoutException"></exception>
    public async Task<GetDataResponse> Transceive(GetDataRequest command)
    {
        GetDataRApduSignal response = new(await _Client.Transceive(command.Serialize()).ConfigureAwait(false));

        // TODO Handle for Status  Words

        return new GetDataResponse(command.GetCorrelationId(), command.GetTransactionSessionId(), response);
    }

    #endregion
}