using System;
using System.Threading.Tasks;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.DataElements;
using Play.Emv.Terminal.Contracts.Messages.Commands;

namespace Play.Emv.Terminal.Services;

internal class TerminalQueryResolver
{
    #region Instance Values

    private readonly IPerformTerminalActionAnalysis _TerminalActionAnalysisService;
    private readonly IManageTerminalRisk _TerminalRiskManager;

    #endregion

    #region Instance Members

    public TagLengthValue Resolve(TerminalStateMachine.TerminalSessionLock sessionLock, Tag tag)
    {
        if (tag == PunatcTrack2.Tag)
            return GetPunatcTrack2(sessionLock);

        if (tag == AmountAuthorizedNumeric.Tag)
            return GetAmountAuthorizedNumeric(sessionLock);

        if (tag == AmountOtherNumeric.Tag)
            return GetAmountOtherNumeric(sessionLock);

        if (tag == TerminalCountryCode.Tag)
            return GetTerminalCountryCode(sessionLock);

        if (tag == TerminalVerificationResults.Tag)
            return GetTerminalVerificationResults(sessionLock);

        if (tag == TransactionCurrencyCode.Tag)
            return GetTransactionCurrencyCode(sessionLock);

        if (tag == TransactionDate.Tag)
            return GetTransactionDate(sessionLock);

        if (tag == TransactionType.Tag)
            return GetTransactionType(sessionLock);

        if (tag == UnpredictableNumber.Tag)
            return GetUnpredictableNumber(sessionLock);

        if (tag == MerchantNameAndLocation.Tag)
            return GetMerchantNameAndLocation(sessionLock);

        return new TagLengthValue(tag, ReadOnlySpan<byte>.Empty);
    }

    private TagLengthValue GetPunatcTrack2(TerminalStateMachine.TerminalSessionLock sessionLock)
    {
        // HACK: Resolve this before implementing Magstripe
        return new TagLengthValue(PunatcTrack2.Tag, ReadOnlySpan<byte>.Empty);
    }

    private TagLengthValue GetAmountAuthorizedNumeric(TerminalStateMachine.TerminalSessionLock sessionLock)
    {
        return sessionLock.Session!.Transaction.GetAmountAuthorizedNumeric().AsTagLengthValue();
    }

    private TagLengthValue GetAmountOtherNumeric(TerminalStateMachine.TerminalSessionLock sessionLock)
    {
        return sessionLock.Session!.Transaction.GetAmountOtherNumeric().AsTagLengthValue();
    }

    private TagLengthValue GetTerminalCountryCode(TerminalStateMachine.TerminalSessionLock sessionLock)
    {
        return sessionLock.Session!.Transaction.GetTerminalCountryCode().AsTagLengthValue();
    }

    private TagLengthValue GetTerminalVerificationResults(TerminalStateMachine.TerminalSessionLock sessionLock)
    {
        // BUG: This is wrong. Validate this logic when implementing C-2

        var transaction = sessionLock.Session!.Transaction;
        var culture = transaction.GetCultureProfile();
        Task<TerminalRiskManagementResponse> a =
            _TerminalRiskManager.Process(new TerminalRiskManagementCommand(default, culture, transaction.GetAmountAuthorizedNumeric(),
                                                                           sessionLock.Session.TerminalConfiguration
                                                                               .GetTerminalRiskConfiguration(culture)));
        transaction.Update(a.Result.GetTerminalVerificationResult());

        throw new NotImplementedException();

        return sessionLock.Session!.Transaction.GetTerminalVerificationResults().AsTagLengthValue();
    }

    private TagLengthValue GetTransactionCurrencyCode(TerminalStateMachine.TerminalSessionLock sessionLock)
    {
        return sessionLock.Session!.Transaction.GetTransactionCurrencyCode().AsTagLengthValue();
    }

    private TagLengthValue GetTransactionDate(TerminalStateMachine.TerminalSessionLock sessionLock)
    {
        return sessionLock.Session!.Transaction.GetTransactionDate().AsTagLengthValue();
    }

    private TagLengthValue GetTransactionType(TerminalStateMachine.TerminalSessionLock sessionLock)
    {
        return sessionLock.Session!.Transaction.GetTransactionType().AsTagLengthValue();
    }

    private TagLengthValue GetUnpredictableNumber(TerminalStateMachine.TerminalSessionLock sessionLock)
    {
        return new UnpredictableNumber().AsTagLengthValue();
    }

    private TagLengthValue GetMerchantNameAndLocation(TerminalStateMachine.TerminalSessionLock sessionLock)
    {
        return sessionLock.Session!.TerminalConfiguration.GetMerchantNameAndLocation();
    }

    #endregion
}