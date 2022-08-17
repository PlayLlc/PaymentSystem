using System.Globalization;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.Templates;
using Play.Emv.Identifiers;
using Play.Emv.Outcomes;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Selection.Contracts;
using Play.Globalization.Time;
using Play.Icc.FileSystem.DedicatedFiles;
using Play.Icc.Messaging.Apdu;
using Play.Messaging;

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
    private readonly HashSet<Combination> _Combinations = new();

    #endregion

    #region Constructor

    // HACK: I don't think this should be injecting POI information directly into this service. Check the specifications and see how we're supposed to handle POI Information
    public CombinationSelector(PoiInformation poiInformation, IHandlePcdRequests pcdEndpoint)
    {
        _PoiInformation = poiInformation;
        _PcdEndpoint = pcdEndpoint;
        _Codec = EmvCodec.GetCodec();
    }

    #endregion

    /// <remarks>EMV Book B Section 3.3.2.1</remarks>
    public void Start(
        TransactionSessionId transactionSessionId, CandidateList candidateList, Outcome outcome, IssuerAuthenticationData? issuerAuthenticationData = null,
        IssuerScriptTemplate1? issuerScriptTemplate1 = null)
    {
        if (outcome.GetStartOutcome() == StartOutcomes.B)
        {
            if ((issuerAuthenticationData != null) || (issuerScriptTemplate1 != null))
            {
                // Continue at 3.3.3.3 Final Combination Selection with the combination that was selected during the previous Final Combination Selection
                return;
            }

            ProcessStep1(transactionSessionId);
        }

        if (outcome.GetStartOutcome() == StartOutcomes.C)
            ProcessStep3(transactionSessionId, candidateList, outcome);
    }

    #region Step 1

    /// <remarks>Emv Book B Section 3.3.2.2</remarks>
    private void ProcessStep1(TransactionSessionId transactionSessionId)
    {
        SelectPpse(transactionSessionId);
    }

    /// <remarks>EMV Book B Section 3.3.2.2</remarks>
    private void SelectPpse(TransactionSessionId transactionSessionId)
    {
        _PcdEndpoint.Request(SelectProximityPaymentSystemEnvironmentRequest.Create(transactionSessionId));
    }

    /// <remarks>Emv Book B Section 3.3.2.3</remarks>
    /// <exception cref="DataElementParsingException"></exception>
    public void ProcessPpseResponse(
        TransactionSessionId transactionSessionId, CandidateList candidateList, PreProcessingIndicators preProcessingIndicators, Outcome outcome,
        TransactionType transactionType, SelectProximityPaymentSystemEnvironmentResponse response)
    {
        if (!response.TryGetFileControlInformation(out FileControlInformationPpse? ppseFileControlInformation))
        {
            // HACK: We're skipping processing of Step 1A for now. We're not sending Send POI Information CAPDU. We need to refactor how we're retrieving the command template
            //if (ppseFileControlInformation!.IsPointOfInteractionApduCommandRequested())
            //    ProcessStep1A(transactionSessionId, ppseFileControlInformation);

            ProcessStep2(transactionSessionId, candidateList, preProcessingIndicators, outcome, transactionType, ppseFileControlInformation);

            return;
        }

        ProcessStep3(transactionSessionId, candidateList, outcome);
    }

    // HACK: We're skipping processing of Step 1A for now. We're not sending Send POI Information CAPDU. We need to refactor how we're retrieving the command template
    /// <remarks>Emv Book B Section 3.3.2.3a</remarks>
    private void ProcessStep1A(TransactionSessionId transactionSessionId, FileControlInformationPpse fileControlInformationTemplatePpse)
    {
        throw new NotImplementedException(
            $"The {nameof(CombinationSelector)} has not been implemented yet. The Kernel TLV Database has not been initialized at this point. Figure out how you're going to implement this section for the SEND POI information");

        _PcdEndpoint.Request(SendPoiInformationRequest.Create(transactionSessionId, fileControlInformationTemplatePpse.AsCommandTemplate(null, null)));
    }

    #endregion

    #region Step 2

    /// <remarks>Emv Book B Section 3.3.2.4 - 3.3.2.5</remarks>
    /// <exception cref="DataElementParsingException"></exception>
    private void ProcessStep2(
        TransactionSessionId transactionSessionId, CandidateList candidateList, PreProcessingIndicators preProcessingIndicators, Outcome outcome,
        TransactionType transactionType, FileControlInformationPpse fileControlInformationPpse)
    {
        // Emv Book B Section 3.3.2.4
        if (fileControlInformationPpse.IsDirectoryEntryListEmpty())
        {
            ProcessStep3(transactionSessionId, candidateList, outcome);

            return;
        }

        // Emv Book B Section 3.3.2.5
        PopulateCandidateList(preProcessingIndicators, transactionType, fileControlInformationPpse);

        ProcessStep3(transactionSessionId, candidateList, outcome);
    }

    /// <remarks>Emv Book B Section 3.3.2.5</remarks>
    /// <exception cref="DataElementParsingException"></exception>
    private void PopulateCandidateList(
        PreProcessingIndicators preProcessingIndicators, TransactionType transactionType, FileControlInformationPpse fileControlInformationTemplatePpse)
    {
        List<DirectoryEntry> directoryEntries = fileControlInformationTemplatePpse.GetDirectoryEntries();
        _Combinations.Clear();

        foreach (DirectoryEntry directoryEntry in directoryEntries)
        {
            // Section 3.3.2.5.B
            if (!IsMatchingAid(directoryEntry.GetApplicationDedicatedFileName().AsDedicatedFileName(), preProcessingIndicators))
            {
                directoryEntries.Remove(directoryEntry);

                continue;
            }

            // Section 3.3.2.5.C
            if (!directoryEntry.TryGetKernelIdentifier(out KernelIdentifier? kernelIdentifier))
            {
                directoryEntries.Remove(directoryEntry);

                continue;
            }

            if (!IsMatchingKernelIdentifier(preProcessingIndicators, directoryEntry, kernelIdentifier!))
            {
                directoryEntries.Remove(directoryEntry);

                continue;
            }

            CombinationCompositeKey combinationKey = new(directoryEntry.GetApplicationDedicatedFileName().AsDedicatedFileName(), kernelIdentifier!.AsKernelId(),
                transactionType);

            _Combinations.Add(CreateCombination(preProcessingIndicators, combinationKey, directoryEntry));
        }
    }

    private bool IsMatchingAid(DedicatedFileName applicationDedicatedFileName, PreProcessingIndicators preProcessingIndicators) =>
        preProcessingIndicators.IsMatchingAid(applicationDedicatedFileName);

    /// <exception cref="InvalidOperationException">Ignore.</exception>
    private Combination CreateCombination(
        in PreProcessingIndicators preProcessingIndicators, CombinationCompositeKey compositeKey, DirectoryEntry directoryEntry)
    {
        if (preProcessingIndicators.TryGetValue(compositeKey, out PreProcessingIndicator? result))
        {
            throw new InvalidOperationException(
                $"A {nameof(PreProcessingIndicator)} was expected to be found for the {nameof(CombinationCompositeKey)} but none was found");
        }

        return Combination.Create(directoryEntry, result!);
    }

    /// <exception cref="DataElementParsingException"></exception>
    private bool IsMatchingKernelIdentifier(PreProcessingIndicators preProcessingIndicators, DirectoryEntry directoryEntry, KernelIdentifier kernelIdentifier)
    {
        if (kernelIdentifier!.IsDomesticKernel())
            return IsMatchingDomesticKernelIdentifier(preProcessingIndicators, kernelIdentifier);

        return IsMatchingInternationalKernelIdentifier(preProcessingIndicators, kernelIdentifier);
    }

    /// <exception cref="DataElementParsingException"></exception>
    private bool IsMatchingDomesticKernelIdentifier(PreProcessingIndicators preProcessingIndicators, KernelIdentifier kernelIdentifier)
    {
        // Book B Section 3.3.2.5 Section C:
        // If the Short Kernel ID is equal to 000000b,then the determination of the Requested Kernel ID is out of scope of this specification.
        if (kernelIdentifier.GetShortKernelId() == 0)
            return false;

        return preProcessingIndicators.IsMatchingKernel(kernelIdentifier);
    }

    #endregion

    #region Step 3

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

    private void ProcessEmptyCandidateList(Outcome outcome)
    {
        UserInterfaceRequestData.Builder? userInterfaceRequestDataBuilder = UserInterfaceRequestData.GetBuilder();
        userInterfaceRequestDataBuilder.Set(MessageIdentifiers.ErrorUseAnotherCard);
        userInterfaceRequestDataBuilder.Set(Statuses.ReadyToRead);

        OutcomeParameterSet.Builder? outcomeParameterSetBuilder = OutcomeParameterSet.GetBuilder();
        outcomeParameterSetBuilder.SetIsUiRequestOnOutcomePresent(true);
        outcomeParameterSetBuilder.Set(new Milliseconds(0));

        outcome.Update(userInterfaceRequestDataBuilder);
        outcome.Update(outcomeParameterSetBuilder);
    }

    /// <exception cref="InvalidOperationException">Ignore.</exception>
    private Combination SelectCombination(CandidateList candidateList)
    {
        if (candidateList.Count == 0)
            throw new InvalidOperationException();

        return candidateList.ElementAt(0);
    }

    /// <remarks>
    ///     When the Proximity Coupling Device sends the <see cref="SelectApplicationDefinitionFileInfoResponse" />
    ///     callback, the processing will continue at <see cref="ProcessValidApplet" />
    /// </remarks>
    private void SelectApplicationFileControlInformation(TransactionSessionId transactionSessionId, Combination combination)
    {
        _PcdEndpoint.Request(SelectApplicationDefinitionFileInfoRequest.Create(transactionSessionId, combination.GetApplicationIdentifier()));
    }

    #endregion

    public bool TrySelectApplet(Combination combination, SelectApplicationDefinitionFileInfoResponse applicationFciResponse, out CombinationOutcome? result)
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

            builder.Set(StatusOutcomes.EndApplication);
            builder.SetIsUiRequestOnOutcomePresent(true);

            userInterfaceRequestDataBuilder.Set(MessageIdentifiers.ErrorUseAnotherCard);
            userInterfaceRequestDataBuilder.Set(Statuses.ReadyToRead);
            userInterfaceRequestDataBuilder.Set(new MessageHoldTime(Milliseconds.Zero));
        }

        result = null;

        return false;
    }

    public void ProcessValidApplet(
        TransactionSessionId transactionSessionId, CorrelationId correlationId, Transaction transaction, CombinationOutcome combinationOutcome,
        PreProcessingIndicatorResult preProcessingIndicatorResult, SelectApplicationDefinitionFileInfoResponse appletFci, Action<OutSelectionResponse> callback)
    {
        OutSelectionResponse outSelectionResponse = new(correlationId, transaction, combinationOutcome.Combination.GetCombinationCompositeKey(),
            preProcessingIndicatorResult.GetTerminalTransactionQualifiers(), appletFci);

        callback.Invoke(outSelectionResponse);
    }

    public void ProcessInvalidAppletResponse(TransactionSessionId transactionSessionId, CandidateList candidateList, Outcome outcome)
    {
        ProcessStep3(transactionSessionId, candidateList, outcome);
    }

    /// <summary>
    ///     ProcessPointOfInteractionResponse
    /// </summary>
    /// <param name="transactionSessionId"></param>
    /// <param name="candidateList"></param>
    /// <param name="preProcessingIndicators"></param>
    /// <param name="outcome"></param>
    /// <param name="transactionType"></param>
    /// <param name="sendPoiInformationResponse"></param>
    /// <exception cref="DataElementParsingException"></exception>
    public void ProcessPointOfInteractionResponse(
        TransactionSessionId transactionSessionId, CandidateList candidateList, PreProcessingIndicators preProcessingIndicators, Outcome outcome,
        TransactionType transactionType, SendPoiInformationResponse sendPoiInformationResponse)
    {
        if (sendPoiInformationResponse.GetStatusWords() == StatusWords._9000)
        {
            ProcessStep2(transactionSessionId, candidateList, preProcessingIndicators, outcome, transactionType,
                FileControlInformationPpse.Decode(sendPoiInformationResponse.GetData()));
        }
        else
            ProcessStep3(transactionSessionId, candidateList, outcome);
    }

    private bool IsMatchingInternationalKernelIdentifier(PreProcessingIndicators preProcessingIndicators, KernelIdentifier kernelIdentifier)
    {
        if (kernelIdentifier.GetValueByteCount(_Codec) < 3)
            return false;

        return preProcessingIndicators.IsMatchingKernel(kernelIdentifier);
    }

    private void ValidateVisaRequirement(
        TransactionSessionId transactionSessionId, CandidateList candidateList, Outcome outcome, Combination combination,
        FileControlInformationAdf fileControlInformationTemplate)
    {
        if (!fileControlInformationTemplate.IsNetworkOf(RegisteredApplicationProviderIndicators.VisaInternational))
            ProcessStep3(transactionSessionId, candidateList, outcome);

        if (combination.GetCombinationCompositeKey().GetKernelId() != ShortKernelIdTypes.Kernel3)
            ProcessStep3(transactionSessionId, candidateList, outcome);

        if (fileControlInformationTemplate.IsDataObjectRequested(TerminalTransactionQualifiers.Tag))
            ProcessStep3(transactionSessionId, candidateList, outcome);
    }
}