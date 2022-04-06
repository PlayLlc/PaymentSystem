using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Display.Contracts;
using Play.Emv.Identifiers;
using Play.Emv.Outcomes;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Selection.Start;

// BUG: Card Collision logic needs to be implemented here. We can inject a service like CardCollisionHandler from the PCD module but it needs to be handled
/// <summary>
/// </summary>
/// <remarks>
///     EMVco Contactless Book B Section 3.2.1
/// </remarks>
public class ProtocolActivator
{
    #region Instance Values

    private readonly IHandleDisplayRequests _DisplayProcess;
    private readonly IHandlePcdRequests _ProximityCouplingDeviceEndpoint;

    #endregion

    #region Constructor

    public ProtocolActivator(IHandlePcdRequests pcdClient, IHandleDisplayRequests displayClient)
    {
        _ProximityCouplingDeviceEndpoint = pcdClient;
        _DisplayProcess = displayClient;
    }

    #endregion

    #region Instance Members

    #region 3.2.1

    /// <summary>
    ///     Protocol Activation is either the next step after Pre-Processing, or Entry Point may be started at Protocol
    ///     Activation
    ///     for new transactions with a fixed amount or as Start B after Outcome Processing.
    /// </summary>
    public void ActivateProtocol(
        TransactionSessionId transactionSessionId, Outcome outcome, PreProcessingIndicators preProcessingIndicators,
        CandidateList candidateList)
    {
        ProcessIfActivationIsNotARestart(outcome, preProcessingIndicators, candidateList);
        ProcessIfActivationIsARestart(outcome);
        ActivateProximityCouplingDevice(transactionSessionId);
    }

    #endregion

    #region 3.2.1.3

    /// <remarks>EMVco Book B Section 3.2.1.3</remarks>
    private void ActivateProximityCouplingDevice(TransactionSessionId transactionSessionId)
    {
        _ProximityCouplingDeviceEndpoint.Request(new ActivatePcdRequest(transactionSessionId));
    }

    #endregion

    #endregion

    #region 3.2.1.1

    /// <param name="outcome"></param>
    /// <param name="preProcessingIndicators"></param>
    /// <param name="candidateList"></param>
    /// <remarks>EMVco Book B Section 3.2.1.1</remarks>
    private void ProcessIfActivationIsNotARestart(
        Outcome outcome, PreProcessingIndicators preProcessingIndicators, CandidateList candidateList)
    {
        if (outcome.IsRestart())
            return;

        if (!outcome.IsErrorPresent())
        {
            preProcessingIndicators.ResetPreprocessingIndicators();
            preProcessingIndicators.ResetTerminalTransactionQualifiers();
        }
        else
            candidateList.Clear();

        _DisplayProcess.Request(GetReadyToReadDisplayMessage());
    }

    private static DisplayMessageRequest GetReadyToReadDisplayMessage()
    {
        UserInterfaceRequestData.Builder? builder = UserInterfaceRequestData.GetBuilder();
        builder.Set(MessageIdentifiers.PresentCard);
        builder.Set(Status.ReadyToRead);

        return new DisplayMessageRequest(builder.Complete());
    }

    #endregion

    #region 3.2.1.2

    /// <exception cref="InvalidOperationException">Ignore.</exception>
    /// <remarks>EMVco Book B Section 3.2.1.2</remarks>
    private void ProcessIfActivationIsARestart(Outcome outcome)
    {
        if (!outcome.IsRestart())
            return;

        if (outcome.IsUiRequestOnRestartPresent())
        {
            if (!outcome.TryGetUserInterfaceRequestData(out UserInterfaceRequestData? requestData))
            {
                throw new
                    InvalidOperationException($"The {nameof(Outcome)} indicated that UI Request on Restart is true but no {nameof(UserInterfaceRequestData)} could be found");
            }

            _DisplayProcess.Request(new DisplayMessageRequest(requestData));
        }
        else
            _DisplayProcess.Request(GetReadyToReadDisplayMessage(outcome));
    }

    private static DisplayMessageRequest GetReadyToReadDisplayMessage(Outcome outcome)
    {
        UserInterfaceRequestData.Builder? builder = UserInterfaceRequestData.GetBuilder();
        builder.Set(MessageIdentifiers.PresentCard);
        builder.Set(Status.ReadyToRead);
        outcome.Update(builder);

        _ = outcome.TryGetUserInterfaceRequestData(out UserInterfaceRequestData? userInterfaceRequestData);

        return new DisplayMessageRequest(userInterfaceRequestData!);
    }

    #endregion
}