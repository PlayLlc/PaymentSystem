using Play.Emv.DataElements;
using Play.Emv.Display.Contracts;
using Play.Emv.Display.Contracts.SignalIn;
using Play.Emv.Outcomes;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Sessions;

namespace Play.Emv.Selection.Start;

// BUG: Card Collision logic needs to be implemented here. We can inject a service like CardCollisionHandler from the PCD module but it needs to be handled
public class ProtocolActivator
{
    #region Instance Values

    private readonly IHandleDisplayRequests _DisplayProcess;
    private readonly IHandlePcdRequests _ProximityCouplingDevice;

    #endregion

    #region Constructor

    public ProtocolActivator(IHandlePcdRequests pcdClient, IHandleDisplayRequests displayClient)
    {
        _ProximityCouplingDevice = pcdClient;
        _DisplayProcess = displayClient;
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Protocol Activation is either the next step after Pre-Processing, or Entry Point may be started at Protocol
    ///     Activation
    ///     for new transactions with a fixed amount or as Start B after Outcome Processing.
    /// </summary>
    public void ActivateProtocol(
        TransactionSessionId transactionSessionId,
        in Outcome outcome,
        in PreProcessingIndicators preProcessingIndicators,
        in CandidateList candidateList)
    {
        ProcessIfActivationIsNotARestart(outcome, preProcessingIndicators, candidateList);
        ProcessIfActivationIsARestart(outcome);
        ActivateProximityCouplingDevice(transactionSessionId);
    }

    private void ActivateProximityCouplingDevice(TransactionSessionId transactionSessionId)
    {
        _ProximityCouplingDevice.Request(new ActivatePcdRequest(transactionSessionId));
    }

    private static DisplayMessageRequest GetReadyToReadDisplayMessage()
    {
        UserInterfaceRequestData.Builder? builder = UserInterfaceRequestData.GetBuilder();
        builder.Set(MessageIdentifier.PresentCard);
        builder.Set(Status.ReadyToRead);

        return new DisplayMessageRequest(builder.Complete());
    }

    /// <exception cref="InvalidOperationException">Ignore.</exception>
    private void ProcessIfActivationIsARestart(in Outcome outcome)
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
            _DisplayProcess.Request(GetReadyToReadDisplayMessage());
    }

    private void ProcessIfActivationIsNotARestart(
        in Outcome outcome,
        in PreProcessingIndicators preProcessingIndicators,
        in CandidateList candidateList)
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

    #endregion
}