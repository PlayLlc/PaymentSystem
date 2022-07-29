using Play.Emv.Configuration;
using Play.Emv.Security;

namespace Play.Emv.Terminal.Configuration.ApplicationDependent;

public class ApplicationDependentConfiguration
{
    #region Instance Values

    private readonly ApplicationInformation _ApplicationInformation;
    private readonly MerchantInformation _MerchantInformation;
    private readonly SecurityConfiguration _SecurityConfiguration;
    private readonly TerminalActionAnalysisConfiguration _TerminalActionAnalysisConfiguration;
    private readonly TerminalRiskManagementConfiguration _TerminalRiskManagementConfiguration;
    private readonly TransactionConfiguration _TransactionConfiguration;

    #endregion

    #region Constructor

    public ApplicationDependentConfiguration(
        ApplicationInformation applicationInformation, MerchantInformation merchantInformation, SecurityConfiguration securityConfiguration,
        TerminalRiskManagementConfiguration terminalRiskManagementConfiguration, TerminalActionAnalysisConfiguration terminalActionAnalysisConfiguration,
        TransactionConfiguration transactionConfiguration)
    {
        _ApplicationInformation = applicationInformation;
        _MerchantInformation = merchantInformation;
        _SecurityConfiguration = securityConfiguration;
        _TerminalRiskManagementConfiguration = terminalRiskManagementConfiguration;
        _TerminalActionAnalysisConfiguration = terminalActionAnalysisConfiguration;
        _TransactionConfiguration = transactionConfiguration;
    }

    #endregion
}