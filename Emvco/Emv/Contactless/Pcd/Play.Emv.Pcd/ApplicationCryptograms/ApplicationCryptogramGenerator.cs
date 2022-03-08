using System.Threading.Tasks;

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
    public async Task<GenerateApplicationCryptogramResponse> Transceive(GenerateApplicationCryptogramRequest request)
    {
        GenerateApplicationCryptogramRApduSignal response = new(await _PcdTransceiver
            .Transceive(request.Serialize()).ConfigureAwait(false));

        return new GenerateApplicationCryptogramResponse(request.GetCorrelationId(), request.GetTransactionSessionId(), response);
    }

    #endregion
}