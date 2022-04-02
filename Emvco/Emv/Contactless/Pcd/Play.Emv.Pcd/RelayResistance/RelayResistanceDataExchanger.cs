using System;
using System.Threading.Tasks;

using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Icc;
using Play.Emv.Pcd.Contracts;

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

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Play.Icc.Exceptions.IccProtocolException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public async Task<ExchangeRelayResistanceDataResponse> Transceive(ExchangeRelayResistanceDataRequest command)
    {
        try
        {
            ExchangeRelayResistanceDataRApduSignal response =
                new(await _PcdTransceiver.Transceive(command.Serialize()).ConfigureAwait(false));

            // TODO Handle for Status  Words

            return new ExchangeRelayResistanceDataResponse(command.GetCorrelationId(), command.GetTransactionSessionId(), response);
        }
        catch (PcdProtocolException)
        {
            // TODO: Logging
            return new ExchangeRelayResistanceDataResponse(command.GetCorrelationId(), command.GetTransactionSessionId(),
                                                           new ExchangeRelayResistanceDataRApduSignal(Array.Empty<byte>(),
                                                            Level1Error.ProtocolError));
        }
        catch (PcdTimeoutException)
        {
            // TODO: Logging
            return new ExchangeRelayResistanceDataResponse(command.GetCorrelationId(), command.GetTransactionSessionId(),
                                                           new ExchangeRelayResistanceDataRApduSignal(Array.Empty<byte>(),
                                                            Level1Error.TimeOutError));
        }
    }

    #endregion
}