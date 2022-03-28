using System;
using System.Threading.Tasks;

using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Icc;
using Play.Emv.Security.Cryptograms;
using Play.Icc.Messaging.Apdu;

namespace Play.Emv.Security;

//I'm doing this wrong. The DOL should be filled from the Kernel side via a DEK
public class ApplicationCryptogramGenerator : IGenerateApplicationCryptogramResponse
{
    #region Instance Values

    private readonly IPcdTransceiver _TransceiveData;

    #endregion

    #region Constructor

    public ApplicationCryptogramGenerator(IPcdTransceiver transceiveData)
    {
        _TransceiveData = transceiveData;
    }

    #endregion

    #region Instance Members

    public async Task<GenerateAcResponseMessage> Generate(
        CryptogramTypes cryptogramType, bool isCdaRequested, DataObjectListResult cardRiskManagementDataObjectListResult)
    {
        GenerateAcResponseMessage response = new(await _TransceiveData
                                                     .Transceive(GenerateApplicationCryptogramCApduSignal.Create(cryptogramType,
                                                                  isCdaRequested, cardRiskManagementDataObjectListResult))
                                                     .ConfigureAwait(false));

        await _TransceiveData
            .Transceive(GenerateApplicationCryptogramCApduSignal.Create(cryptogramType, isCdaRequested,
                                                                        cardRiskManagementDataObjectListResult)).ConfigureAwait(false);
    }

    public async Task<GenerateAcResponseMessage> Generate(
        CryptogramTypes cryptogramType, bool isCdaRequested, DataObjectListResult cardRiskManagementDataObjectListResult,
        DataObjectListResult dataStorageDataObjectListResult)
    {
        RApduSignal rApdu = await _TransceiveData
            .Transceive(GenerateApplicationCryptogramCApduSignal.Create(cryptogramType, isCdaRequested,
                                                                        cardRiskManagementDataObjectListResult,
                                                                        dataStorageDataObjectListResult)).ConfigureAwait(false);

        if (rApdu.GetStatusWords() == StatusWords._6283)
            throw new Exception();

        // publish a FatalTerminalEvent. throw an exception here so the calling thread will return control back to the Terminal?. 
        // but exceptions take a long time to process.
    }

    #endregion
}