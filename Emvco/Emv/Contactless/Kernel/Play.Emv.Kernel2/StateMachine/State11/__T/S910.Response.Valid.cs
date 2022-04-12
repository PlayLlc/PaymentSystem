using System;

using Play.Ber.DataObjects;
using Play.Core.Extensions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Icc.Exceptions;

namespace Play.Emv.Kernel2.StateMachine.__T;

public partial class S910
{
    private partial class ResponseHandler
    {
        #region Instance Members

        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="IccProtocolException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public StateId HandleValidResponse(IGetKernelStateId currentStateIdRetriever, Kernel2Session session)
        {
            // S910.70
            BuildDataRecord();

            // S910.71 - S910.75
            if (!TryPreparingOutcomeForSecondTap())
                PrepareOutcomeParameterSetForCid();

            if (TryWaitingForPutDataResponseAfterGeneratingAc(session.GetKernelSessionId()))
                return WaitingForPutDataResponseAfterGenerateAc.StateId;

            HandleOutMessage(session);

            return Idle.StateId;
        }

        #region S910.70

        /// <remarks>EMV Book C-2 Section S910.70</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private void BuildDataRecord()
        {
            _Database.SetIsDataRecordPresent(true);
            _Database.CreateEmvDataRecord(_DataExchangeKernelService);
        }

        #endregion

        #region S910.71

        /// <remarks>EMV Book C-2 Section S910.71</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private bool TryPreparingOutcomeForSecondTap()
        {
            PosCardholderInteractionInformation pcii =
                _Database.Get<PosCardholderInteractionInformation>(PosCardholderInteractionInformation.Tag);

            if (!pcii.IsSecondTapNeeded())
                return false;

            PrepareOutcomeParameterSetForPcii();
            AttemptToSetPciiDisplayMessage();

            return true;
        }

        #endregion

        #region S910.72

        /// <remarks>EMV Book C-2 Section S910.72</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private void PrepareOutcomeParameterSetForPcii()
        {
            _Database.Update(StatusOutcome.EndApplication);
            _Database.Update(StartOutcome.B);
        }

        #endregion

        #region S910.73

        /// <remarks>EMV Book C-2 Section S910.73</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private void AttemptToSetPciiDisplayMessage()
        {
            PhoneMessageTable phoneMessageTable = _Database.Get<PhoneMessageTable>(PhoneMessageTable.Tag);
            PosCardholderInteractionInformation pcii =
                _Database.Get<PosCardholderInteractionInformation>(PosCardholderInteractionInformation.Tag);

            if (!phoneMessageTable.TryGetMatch(pcii, out MessageTableEntry? messageTableEntry))
                return;

            // TODO: I'm not sure if the below line is correct - Where do we get the MessageHoldTime from?
            _Database.Update(_Database.Get<MessageHoldTime>(MessageHoldTime.Tag));
            _Database.Update(messageTableEntry!.GetStatus());
        }

        #endregion

        #region S910.74 - S910.75

        /// <remarks>EMV Book C-2 Section S910.74 - S910.75</remarks>
        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private void PrepareOutcomeParameterSetForCid()
        {
            CryptogramInformationData cid = _Database.Get<CryptogramInformationData>(CryptogramInformationData.Tag);

            if (cid.GetCryptogramType() == CryptogramTypes.TransactionCryptogram)
                PrepareOutcomeForTransactionCryptogram();
            else if (cid.GetCryptogramType() == CryptogramTypes.AuthorizationRequestCryptogram)
                PrepareOutcomeForApplicationRequestCryptogram();
            else
                PrepareOutcomeForApplicationAuthenticationCryptogram();
        }

        #endregion

        #region S910.74 - S910.75 Shared

        /// <remarks>EMV Book C-2 Section S910.74 - S910.75 Shared</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private bool IsDeclined()
        {
            if (!_Database.IsPurchaseTransaction() && !_Database.IsCashTransaction())
                return false;

            if (!_Database.IsIcWithContactsSupported())
                return true;

            if (!_Database.TryGet(ThirdPartyData.Tag, out ThirdPartyData? thirdPartyData))
                return false;

            if (thirdPartyData!.GetUniqueIdentifier().AreAnyBitsSet(0x8000))
                return false;

            if (thirdPartyData!.TryGetDeviceType(out ushort? deviceType))
                return false;

            if (deviceType == 0x3030)
                return false;

            return true;
        }

        #endregion

        #region S910.74 - S910.75 Transaction Cryptogram

        /// <remarks>EMV Book C-2 Section S910.74 - S910.75 Transaction Cryptogram</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private void PrepareOutcomeForTransactionCryptogram()
        {
            _Database.Update(StatusOutcome.Approved);
            _Database.Update(_Database.Get<MessageHoldTime>(MessageHoldTime.Tag));

            if (_Database.TryGet(BalanceReadAfterGenAc.Tag, out BalanceReadAfterGenAc? balanceReadAfterGenAc))
            {
                _Database.Update(ValueQualifier.Balance);
                _Database.Update(balanceReadAfterGenAc!);

                if (_Database.TryGet(ApplicationCurrencyCode.Tag, out ApplicationCurrencyCode? currencyCode))
                    _Database.Update(currencyCode!);
            }

            if (_Database.GetOutcomeParameterSet().GetCvmPerformed() == CvmPerformedOutcome.ObtainSignature)
                _Database.Update(MessageIdentifiers.ApprovedPleaseSign);
            else
                _Database.Update(MessageIdentifiers.Approved);
        }

        #endregion

        #region S910.74 - S910.75 Application Request Cryptogram

        /// <remarks>EMV Book C-2 Section S910.74 - S910.75 Application Request Cryptogram</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private void PrepareOutcomeForApplicationRequestCryptogram()
        {
            _Database.Update(StatusOutcome.OnlineRequest);

            if (_Database.IsPurchaseTransaction() || _Database.IsCashTransaction())
                _Database.Update(_Database.Get<MessageHoldTime>(MessageHoldTime.Tag));

            if (IsDeclined())
                _Database.Update(MessageIdentifiers.Declined);
            else
                _Database.Update(MessageIdentifiers.PleaseInsertOrSwipeCard);
        }

        #endregion

        #region S910.74 - S910.75 Application Authentication Cryptogram

        /// <remarks>EMV Book C-2 Section S910.74 - S910.75 Application Authentication Cryptogram</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private void PrepareOutcomeForApplicationAuthenticationCryptogram()
        {
            _Database.Update(MessageHoldTime.MinimumValue);
            _Database.Update(MessageIdentifiers.ClearDisplay);

            if (!_Database.IsPurchaseTransaction() && !_Database.IsCashTransaction())
            {
                _Database.Update(StatusOutcome.EndApplication);

                return;
            }

            if (!IsDeclined())
                _Database.Update(StatusOutcome.TryAnotherInterface);
            else
                _Database.Update(StatusOutcome.Declined);
        }

        #endregion

        #region S910.76 - S910.78

        /// <remarks>EMV Book C-2 Section S910.76 - S910.78</remarks>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="IccProtocolException"></exception>
        private bool TryWaitingForPutDataResponseAfterGeneratingAc(KernelSessionId sessionId)
        {
            if (!_DataExchangeKernelService.IsEmpty(DekResponseType.TagsToWriteAfterGenAc))
                return false;

            if (!_DataExchangeKernelService.TryPeek(DekResponseType.TagsToWriteBeforeGenAc, out PrimitiveValue? tagToWrite))
                return false;

            PutDataRequest capdu = PutDataRequest.Create(sessionId.GetTransactionSessionId(), tagToWrite!);
            _PcdEndpoint.Request(capdu);

            return true;
        }

        #endregion

        #region S910.78.1 -S910.81

        /// <remarks>EMV Book C-2 Section S910.78.1 -S910.81</remarks>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private void HandleOutMessage(Kernel2Session session)
        {
            PosCardholderInteractionInformation pcii =
                _Database.Get<PosCardholderInteractionInformation>(PosCardholderInteractionInformation.Tag);

            if (pcii.IsSecondTapNeeded())
            {
                HandleDisplayMessageForSecondTapNeeded(session);

                return;
            }

            HandleDisplayMessage(session);
        }

        #endregion

        #region S910.79 - S910.80

        /// <remarks>EMV Book C-2 Section S910.79 - S910.80</remarks>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private void HandleDisplayMessageForSecondTapNeeded(Kernel2Session session)
        {
            _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);
            _Database.SetUiRequestOnRestartPresent(true);
            _Database.Update(Status.ReadyToRead);
            _Database.Update(MessageHoldTime.MinimumValue);

            _KernelEndpoint.Send(new OutKernelResponse(session.GetCorrelationId(), session.GetKernelSessionId(), _Database.GetOutcome()));
        }

        #endregion

        #region S910.81

        /// <remarks>EMV Book C-2 Section S910.81</remarks>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private void HandleDisplayMessage(Kernel2Session session)
        {
            _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);
            _Database.SetUiRequestOnRestartPresent(true);

            _KernelEndpoint.Send(new OutKernelResponse(session.GetCorrelationId(), session.GetKernelSessionId(), _Database.GetOutcome()));
        }

        #endregion

        #endregion
    }
}