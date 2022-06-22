using Play.Emv.Ber.DataElements;

namespace Play.Emv.Terminal.Configuration.ApplicationDependent;

public class ApplicationInformation
{
    #region Instance Values

    private readonly AcquirerIdentifier _AcquirerIdentifier;
    private readonly ApplicationIdentifier _ApplicationIdentifier;
    private readonly ApplicationVersionNumberReader _ApplicationVersionNumber;

    #endregion

    #region Constructor

    public ApplicationInformation(
        AcquirerIdentifier acquirerIdentifier, ApplicationIdentifier applicationIdentifier, ApplicationVersionNumberReader applicationVersionNumber)
    {
        _AcquirerIdentifier = acquirerIdentifier;
        _ApplicationIdentifier = applicationIdentifier;
        _ApplicationVersionNumber = applicationVersionNumber;
    }

    #endregion
}