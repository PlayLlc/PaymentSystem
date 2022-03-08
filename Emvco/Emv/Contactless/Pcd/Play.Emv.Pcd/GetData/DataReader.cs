using System.Threading.Tasks;

using Play.Emv.Icc.GetData;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd.GetData;

/// <summary>
///     This object reads data that is not specific to the currently selected application. The
///     <see cref="ApplicationDataReader" /> is used to retrieve application specific data from the ApplicationFileLocator
///     provided by the application
/// </summary>
public class DataReader : IReadIccData
{
    #region Instance Values

    private readonly IPcdTransceiver _PcdTransceiver;

    #endregion

    #region Constructor

    public DataReader(IPcdTransceiver pcdTransceiver)
    {
        _PcdTransceiver = pcdTransceiver;
    }

    #endregion

    #region Instance Members

    public async Task<GetDataResponse> Transceive(GetDataRequest command)
    {
        byte[] rapdu = await _PcdTransceiver.Transceive(command.Serialize()).ConfigureAwait(false);

        return new GetDataResponse(command.GetCorrelationId(), command.GetTransactionSessionId(), new GetDataRApduSignal(rapdu));
    }

    #endregion
}