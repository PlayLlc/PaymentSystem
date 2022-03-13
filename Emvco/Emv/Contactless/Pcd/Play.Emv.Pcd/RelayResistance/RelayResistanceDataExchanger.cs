using System.Threading.Tasks;

using Play.Emv.Icc.ComputeCryptographicChddecksum;
using Play.Emv.Pcd.Contracts.SignalIn.Quereies;
using Play.Emv.Pcd.Contracts.SignalOut.Queddries;

namespace Play.Emv.Pcd;

public class RelayResistanceDataExchanger : IExchangeRelayResistanceData
{
    #region Instance Values

    private readonly IPcdTransceiver _PcdTransceiver;

    #endregion

    #region Constructor

    public RelayResistanceDataExchanger(IPcdTransceiver pcdTransceiver)
    {
        _PcdTransceiver = pcdTransceiver;
    }

    #endregion

    #region Instance Members

    /// <exception cref="Emv.Exceptions.DataElementParsingException"></exception>
    /// <exception cref="Play.Icc.Exceptions.IccProtocolException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public async Task<ExchangeRelayResistanceDataResponse> Transceive(ExchangeRelayResistanceDataRequest command)
    {
        ExchangeRelayResistanceDataRApduSignal response = new(await _PcdTransceiver.Transceive(command.Serialize()).ConfigureAwait(false));

        // TODO Handle for Status  Words

        return new ExchangeRelayResistanceDataResponse(command.GetCorrelationId(), command.GetTransactionSessionId(), response);
    }

    #endregion
}