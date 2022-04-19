using System;
using System.Threading.Tasks;

using Play.Ber.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Icc;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd.ApplicationCryptograms;

public class ApplicationCryptogramGenerator : IGenerateApplicationCryptogram
{
    #region Instance Values

    private readonly IPcdTransceiver _PcdTransceiver;

    #endregion

    #region Constructor

    public ApplicationCryptogramGenerator(IPcdTransceiver pcdTransceiver)
    {
        _PcdTransceiver = pcdTransceiver;
    }

    #endregion

    #region Instance Members

    // HACK: We need to wrap the contents of this method in a try catch and capture any Level1Errors that occur while transceiving. This is where we need to be cognizant of timeouts happening
    /// <exception cref="BerParsingException"></exception>
    public async Task<GenerateApplicationCryptogramResponse> Transceive(GenerateApplicationCryptogramRequest command)
    {
        try
        {
            GenerateApplicationCryptogramRApduSignal response = new(await _PcdTransceiver.Transceive(command.Serialize()).ConfigureAwait(false));

            return new GenerateApplicationCryptogramResponse(command.GetCorrelationId(), command.GetTransactionSessionId(), response);
        }

        catch (PcdProtocolException)
        {
            // TODO: Logging

            return new GenerateApplicationCryptogramResponse(command.GetCorrelationId(), command.GetTransactionSessionId(),
                new GenerateApplicationCryptogramRApduSignal(Array.Empty<byte>(), Level1Error.ProtocolError));
        }
        catch (PcdTimeoutException)
        {
            // TODO: Logging
            return new GenerateApplicationCryptogramResponse(command.GetCorrelationId(), command.GetTransactionSessionId(),
                new GenerateApplicationCryptogramRApduSignal(Array.Empty<byte>(), Level1Error.TimeOutError));
        }
    }

    #endregion
}