namespace Play.Emv.Security.Services.Cryptograms;

// I'm doing this wrong. The DOL should be filled from the Kernel side via a DEK
//public class ApplicationCryptogramGenerator : IGenerateApplicationCryptogramResponse
//{
//    #region Instance Values

//    private readonly IPcdTransceiver _TransceiveData;

//    #endregion

//    #region Constructor

//    public ApplicationCryptogramGenerator(IPcdTransceiver transceiveData)
//    {
//        _TransceiveData = transceiveData;
//    }

//    #endregion

//    #region Instance Members

//    //public async Task<GenerateAcResponseMessage> Generate(CryptogramType cryptogramType, bool isCdaRequested,
//    //    DataObjectListResult cardRiskManagementDataObjectListResult)
//    //{
//    //    GenerateAcResponseMessage response = new(await _TransceiveData
//    //        .Transceive(GenerateApplicationCryptogramCApduSignal.Create(cryptogramType, isCdaRequested,
//    //            cardRiskManagementDataObjectListResult)).ConfigureAwait(false));

//    //    await _TransceiveData
//    //        .Transceive(GenerateApplicationCryptogramCApduSignal.Create(cryptogramType, isCdaRequested,
//    //            cardRiskManagementDataObjectListResult)).ConfigureAwait(false);

//    //}

//    //public async Task<GenerateAcResponseMessage> Generate(CryptogramType cryptogramType, bool isCdaRequested,
//    //    DataObjectListResult cardRiskManagementDataObjectListResult,
//    //    DataObjectListResult dataStorageDataObjectListResult)
//    //{
//    //    RApduSignal rApdu = await _TransceiveData
//    //        .Transceive(GenerateApplicationCryptogramCApduSignal.Create(cryptogramType, isCdaRequested,
//    //            cardRiskManagementDataObjectListResult, dataStorageDataObjectListResult)).ConfigureAwait(false);

//    //    if (rApdu.GetStatusWords() == StatusWords._6283) throw new Exception();

//    //    // publish a FatalTerminalEvent. throw an exception here so the calling thread will return control back to the Terminal?. 
//    //    // but exceptions take a long time to process.
//    //}

//    #endregion
//}