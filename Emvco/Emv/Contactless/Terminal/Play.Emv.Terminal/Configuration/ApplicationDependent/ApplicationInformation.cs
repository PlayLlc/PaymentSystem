using Play.Emv.DataElements;
using Play.Emv.DataElements.Emv;

namespace Play.Emv.Terminal.Configuration.ApplicationDependent;

public class ApplicationInformation
{
    #region Instance Values

    private readonly AcquirerIdentifier _AcquirerIdentifier;
    private readonly ApplicationIdentifier _ApplicationIdentifier;
    private readonly ApplicationVersionNumberTerminal _ApplicationVersionNumber;

    #endregion

    #region Constructor

    public ApplicationInformation(
        AcquirerIdentifier acquirerIdentifier,
        ApplicationIdentifier applicationIdentifier,
        ApplicationVersionNumberTerminal applicationVersionNumber)
    {
        _AcquirerIdentifier = acquirerIdentifier;
        _ApplicationIdentifier = applicationIdentifier;
        _ApplicationVersionNumber = applicationVersionNumber;
    }

    #endregion
}