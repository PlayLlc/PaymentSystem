using Play.Ber.Emv.DataObjects;
using Play.Emv.DataElements;

namespace ___TEMP.Play.Emv.Security.Cryptograms;

// TODO: Move the Generate Application Cryptogram CAPDU to the Play.Emv.Card layer
public interface IGenerateApplicationCryptogramResponse
{
    #region Instance Members

    public Task<GenerateAcResponseMessage> Generate(
        CryptogramType cryptogramType,
        bool isCdaRequested,
        DataObjectListResult cardRiskManagementDataObjectListResult);

    public Task<GenerateAcResponseMessage> Generate(
        CryptogramType cryptogramType,
        bool isCdaRequested,
        DataObjectListResult cardRiskManagementDataObjectListResult,
        DataObjectListResult dataStorageDataObjectListResult);

    #endregion
}