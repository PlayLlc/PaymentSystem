using System.Threading.Tasks;

using Play.Emv.Ber.DataObjects;

namespace Play.Emv.Security.Cryptograms;

// TODO: Move the Generate Application Cryptogram CAPDU to the Play.Emv.Card layer
public interface IGenerateApplicationCryptogramResponse
{
    #region Instance Members

    public Task<GenerateAcResponseMessage> Generate(
        CryptogramTypes cryptogramTypes,
        bool isCdaRequested,
        DataObjectListResult cardRiskManagementDataObjectListResult);

    public Task<GenerateAcResponseMessage> Generate(
        CryptogramTypes cryptogramTypes,
        bool isCdaRequested,
        DataObjectListResult cardRiskManagementDataObjectListResult,
        DataObjectListResult dataStorageDataObjectListResult);

    #endregion
}