using Play.Ber.DataObjects;
using Play.Emv.Ber;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.Outcomes;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Selection.Contracts;
using Play.Emv.Sessions;
using Play.Emv.Templates;
using Play.Globalization.Time;
using Play.Globalization.Time.Seconds;
using Play.Icc.FileSystem.DedicatedFiles;
using Play.Icc.Messaging.Apdu;
using Play.Messaging;

using MessageIdentifier = Play.Emv.DataElements.MessageIdentifier;

namespace Play.Emv.Selection.Start;

/// <summary>
///     Entry Point Start C - Candidate Selection. The process that negotiates mutually acceptable PICC Applications and
///     the Kernel to run them on for a given transaction. This process will rank Applications in priority order and
///     attempt to process the Application that is preferred by both the PICC and the Terminal
/// </summary>
public class CombinationSelector
{
    #region Instance Values

    private readonly IHandlePcdRequests _PcdEndpoint;
    private readonly EmvCodec _Codec;
    private readonly PoiInformation _PoiInformation;

    #endregion

    #region Constructor

    // HACK: I don't think this should be injecting POI information directly into this service. Check the specifications and see how we're supposed to handle POI Information
    public CombinationSelector(PoiInformation poiInformation, IHandlePcdRequests pcdEndpoint)
    {
        _PoiInformation = poiInformation;
        _PcdEndpoint = pcdEndpoint;
        _Codec = EmvCodec.GetBerCodec();
    }

    #endregion

    #region Instance Members

    public void Start(
        TransactionSessionId transactionSessionId,
        in CandidateList candidateList,
        in PreProcessingIndicators preProcessingIndicators,
        in Outcome outcome,
        in TransactionType transactionType)
    {
        if (outcome.GetStartOutcome() == StartOutcome.B)
        {
            // BUG: It looks like I added this here because some state logic wasn't implemented. Check the specification and correct this part of the flow
            if (1 == 2)
            {
                ProcessStep3(transactionSessionId, candidateList, outcome);

                return;
            }
        }

        ProcessStep1(transactionSessionId);
    }

    /// <exception cref="InvalidOperationException">Ignore.</exception>
    private Combination CreateCombination(
        in PreProcessingIndicators preProcessingIndicators,
        CombinationCompositeKey compositeKey,
        DirectoryEntry directoryEntry)
    {
        if (preProcessingIndicators.TryGetValue(compositeKey, out PreProcessingIndicator? result))
        {
            throw new
                InvalidOperationException($"A {nameof(PreProcessingIndicator)} was expected to be found for the {nameof(CombinationCompositeKey)} but none was found");
        }

        return Combination.Create(directoryEntry, result!);
    }

    public void ProcessValidApplet(
        TransactionSessionId transactionSessionId,
        CorrelationId correlationId,
        Transaction transaction,
        CombinationOutcome combinationOutcome,
        PreProcessingIndicatorResult preProcessingIndicatorResult,
        SelectApplicationDefinitionFileInfoResponse appletFci,
        Action<OutSelectionResponse> callback)
    {
        OutSelectionResponse outSelectionResponse = new(correlationId, transaction,
                                                        combinationOutcome.Combination.GetCombinationCompositeKey(),
                                                        preProcessingIndicatorResult.GetTerminalTransactionQualifiers(), appletFci);

        callback.Invoke(outSelectionResponse);
    }

    public bool TrySelectApplet(
        TransactionSessionId transactionSessionId,
        CandidateList candidateList,
        Outcome outcome,
        Combination combination,
        SelectApplicationDefinitionFileInfoResponse applicationFciResponse,
        out CombinationOutcome? result)
    {
        if (applicationFciResponse.GetStatusWords() == StatusWords._9000)
        {
            result = new CombinationOutcome(applicationFciResponse, combination);

            return true;
        }

        // BUG: It looks like I added this here because part of the Issuer Script processing wasn't yet implemented. Check the specification and correct this part of the flow
        if (1 == 2) // if(IssuerAuthenticationData is present || IssuerScript is present)
        {
            OutcomeParameterSet.Builder builder = OutcomeParameterSet.GetBuilder();
            UserInterfaceRequestData.Builder? userInterfaceRequestDataBuilder = UserInterfaceRequestData.GetBuilder();

            builder.Set(StatusOutcome.EndApplication);
            builder.SetIsUiRequestOnOutcomePresent(true);

            userInterfaceRequestDataBuilder.Set(MessageIdentifier.InsertSwipeOrTryAnotherCard);
            userInterfaceRequestDataBuilder.Set(Status.ReadyToRead);
            userInterfaceRequestDataBuilder.Set(new MessageHoldTime(Milliseconds.Zero));
        }

        result = null;

        return false;
    }

    public void ProcessInvalidAppletResponse(TransactionSessionId transactionSessionId, CandidateList candidateList, Outcome outcome)
    {
        ProcessStep3(transactionSessionId, candidateList, outcome);
    }

    public void ProcessPointOfInteractionResponse(
        TransactionSessionId transactionSessionId,
        CandidateList candidateList,
        PreProcessingIndicators preProcessingIndicators,
        Outcome outcome,
        TransactionType transactionType,
        SendPoiInformationResponse sendPoiInformationResponse)
    {
        if (sendPoiInformationResponse.GetStatusWords() == StatusWords._9000)
        {
            ProcessStep2(transactionSessionId, candidateList, preProcessingIndicators, outcome, transactionType,
                         FileControlInformationPpse.Decode(sendPoiInformationResponse.GetData()));
        }
        else
            ProcessStep3(transactionSessionId, candidateList, outcome);
    }

    private bool IsMatchingAid(DedicatedFileName applicationDedicatedFileName, PreProcessingIndicators preProcessingIndicators) =>
        preProcessingIndicators.IsMatchingAid(applicationDedicatedFileName);

    private bool IsMatchingDomesticKernelIdentifier(PreProcessingIndicators preProcessingIndicators, KernelIdentifier kernelIdentifier)
    {
        // Book B Section 3.3.2.5 Section C:
        // If the Short Kernel ID is equal to 000000b,then the determination of the Requested Kernel ID is out of scope of this specification.
        if (kernelIdentifier.GetShortKernelId() == 0)
            return false;

        return preProcessingIndicators.IsMatchingKernel(kernelIdentifier);
    }

    private bool IsMatchingInternationalKernelIdentifier(PreProcessingIndicators preProcessingIndicators, KernelIdentifier kernelIdentifier)
    {
        if (kernelIdentifier.GetValueByteCount(_Codec) < 3)
            return false;

        return preProcessingIndicators.IsMatchingKernel(kernelIdentifier);
    }

    private bool IsMatchingKernelIdentifier(PreProcessingIndicators preProcessingIndicators, DirectoryEntry directoryEntry)
    {
        if (!directoryEntry.TryGetKernelIdentifier(out KernelIdentifier? result))
            return false;

        if (result!.IsDomesticKernel())
            return IsMatchingDomesticKernelIdentifier(preProcessingIndicators, result);

        return IsMatchingInternationalKernelIdentifier(preProcessingIndicators, result);
    }

    private void PopulateCandidateList(
        PreProcessingIndicators preProcessingIndicators,
        TransactionType transactionType,
        FileControlInformationPpse fileControlInformationTemplatePpse)
    {
        List<DirectoryEntry> directoryEntries = fileControlInformationTemplatePpse.GetDirectoryEntries();
        HashSet<Combination> combinations = new();

        foreach (DirectoryEntry directoryEntry in directoryEntries)
        {
            if (!IsMatchingAid(directoryEntry.GetApplicationDedicatedFileName().AsDedicatedFileName(), preProcessingIndicators))
            {
                directoryEntries.Remove(directoryEntry);

                continue;
            }

            if (!directoryEntry.TryGetKernelIdentifier(out KernelIdentifier? kernelIdentifier))
            {
                directoryEntries.Remove(directoryEntry);

                continue;
            }

            if (!IsMatchingKernelIdentifier(preProcessingIndicators, directoryEntry))
            {
                directoryEntries.Remove(directoryEntry);

                continue;
            }

            CombinationCompositeKey combinationKey = new(directoryEntry.GetApplicationDedicatedFileName().AsDedicatedFileName(),
                                                         kernelIdentifier!.AsKernelId(), transactionType);

            combinations.Add(CreateCombination(preProcessingIndicators, combinationKey, directoryEntry));
        }
    }

    private void ProcessEmptyCandidateList(Outcome outcome)
    {
        UserInterfaceRequestData.Builder? userInterfaceRequestDataBuilder = UserInterfaceRequestData.GetBuilder();
        userInterfaceRequestDataBuilder.Set(MessageIdentifier.InsertSwipeOrTryAnotherCard);
        userInterfaceRequestDataBuilder.Set(Status.ReadyToRead);

        OutcomeParameterSet.Builder? outcomeParameterSetBuilder = OutcomeParameterSet.GetBuilder();
        outcomeParameterSetBuilder.SetIsUiRequestOnOutcomePresent(true);
        outcomeParameterSetBuilder.Set(new Milliseconds(0));

        outcome.Update(userInterfaceRequestDataBuilder);
        outcome.Update(outcomeParameterSetBuilder);
    }

    private void ProcessIfDirectoryEntryListIsEmpty(TransactionSessionId transactionSessionId, CandidateList candidateList, Outcome outcome)
    {
        ProcessStep3(transactionSessionId, candidateList, outcome);
    }

    public void ProcessPpseResponse(
        TransactionSessionId transactionSessionId,
        CandidateList candidateList,
        PreProcessingIndicators preProcessingIndicators,
        Outcome outcome,
        TransactionType transactionType,
        SelectProximityPaymentSystemEnvironmentResponse response)
    {
        if (!response.TryGetFileControlInformation(out FileControlInformationPpse? ppseFileControlInformation))
        {
            if (ppseFileControlInformation!.IsPointOfInteractionApduCommandRequested())
                ProcessStep1A(transactionSessionId, ppseFileControlInformation);

            ProcessStep2(transactionSessionId, candidateList, preProcessingIndicators, outcome, transactionType,
                         ppseFileControlInformation);

            return;
        }

        ProcessStep3(transactionSessionId, candidateList, outcome);
    }

    private void ProcessStep1(TransactionSessionId transactionSessionId)
    {
        SelectPpse(transactionSessionId);
    }

    private void ProcessStep1A(TransactionSessionId transactionSessionId, FileControlInformationPpse fileControlInformationTemplatePpse)
    {
        SendPointOfInteractionApduCommand(transactionSessionId, fileControlInformationTemplatePpse);
    }

    private void ProcessStep2(
        TransactionSessionId transactionSessionId,
        CandidateList candidateList,
        PreProcessingIndicators preProcessingIndicators,
        Outcome outcome,
        TransactionType transactionType,
        FileControlInformationPpse fileControlInformationPpse)
    {
        if (fileControlInformationPpse.IsDirectoryEntryListEmpty())
            /*return*/
            ProcessIfDirectoryEntryListIsEmpty(transactionSessionId, candidateList, outcome);

        PopulateCandidateList(preProcessingIndicators, transactionType, fileControlInformationPpse);

        ProcessStep3(transactionSessionId, candidateList, outcome);
    }

    /// <summary>
    ///     Extended Selection is encapsulated in the <see cref="Combination" /> factory method
    /// </summary>
    /// <returns></returns>
    private void ProcessStep3(TransactionSessionId transactionSessionId, CandidateList candidateList, Outcome outcome)
    {
        if (candidateList.Count == 0)
            ProcessEmptyCandidateList(outcome);

        Combination combination = SelectCombination(candidateList);

        SelectApplicationFileControlInformation(transactionSessionId, combination);
    }

    /// <remarks>
    ///     When the Proximity Coupling Device sends the <see cref="SelectApplicationDefinitionFileInfoResponse" />
    ///     callback, the processing will continue at <see cref="ProcessAppletResponse" />
    /// </remarks>
    private void SelectApplicationFileControlInformation(TransactionSessionId transactionSessionId, Combination combination)
    {
        _PcdEndpoint.Request(SelectApplicationDefinitionFileInfoRequest.Create(transactionSessionId,
                                                                               combination.GetApplicationIdentifier()));
    }

    /// <exception cref="InvalidOperationException">Ignore.</exception>
    private Combination SelectCombination(CandidateList candidateList)
    {
        if (candidateList.Count == 0)
            throw new InvalidOperationException();

        return candidateList.ElementAt(0);
    }

    // TODO: Handle communications errors by calling HandleCommunicationsError for PCD Activate
    private void SelectPpse(TransactionSessionId transactionSessionId)
    {
        _PcdEndpoint.Request(SelectProximityPaymentSystemEnvironmentRequest.Create(transactionSessionId));
    }

    // TODO: ======================================================================================================
    // TODO: WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING WARNING
    // TODO: ======================================================================================================
    // TODO: We should not be passing an empty array to the fileControlInformationTemplatePpse. We need to be
    // TODO: using the Data Exchange mechanisms and sending this C-APDU once we have been notified that the values
    // TODO: requested 
    private void SendPointOfInteractionApduCommand(
        TransactionSessionId transactionSessionId,
        FileControlInformationPpse fileControlInformationTemplatePpse)
    {
        CommandTemplate? commandTemplate =
            fileControlInformationTemplatePpse.AsCommandTemplate(_Codec, _PoiInformation, Array.Empty<TagLengthValue>());

        _PcdEndpoint.Request(SendPoiInformationRequest.Create(transactionSessionId, commandTemplate));
    }

    private void ValidateVisaRequirement(
        TransactionSessionId transactionSessionId,
        CandidateList candidateList,
        Outcome outcome,
        Combination combination,
        FileControlInformationAdf fileControlInformationTemplate)
    {
        if (!fileControlInformationTemplate.IsNetworkOf(RegisteredApplicationProviderIndicators.VisaInternational))
            ProcessStep3(transactionSessionId, candidateList, outcome);

        if (combination.GetCombinationCompositeKey().GetKernelId() != ShortKernelIdTypes.Kernel3)
            ProcessStep3(transactionSessionId, candidateList, outcome);

        if (fileControlInformationTemplate.IsDataObjectRequested(TerminalTransactionQualifiers.Tag))
            ProcessStep3(transactionSessionId, candidateList, outcome);
    }

    #endregion

    //private readonly IProximityCouplingDeviceProcess _ProximityCouplingDevice;

    // This is taken care of by the default value of ApplicationPriorityIndicator

    // Usage of ASRPD (Application Selection Registered Proprietary Data) is optional and has not yet been implemented

    // SelectCombination();

    // Extended Selection is encapsulated in the Combination factory method
}