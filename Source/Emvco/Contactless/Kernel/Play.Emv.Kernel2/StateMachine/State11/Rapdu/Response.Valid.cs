using System;

using Play.Ber.DataObjects;
using Play.Core.Extensions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Display.Contracts;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Icc.Exceptions;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGenerateAcResponse2
{
    private partial class ResponseHandler
    {
        #region Instance Members

        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="IccProtocolException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public StateId HandleValidResponse(Kernel2Session session)
        {
            // S11.110
            BuildDataRecord();

            // S11.111 - S11.113
            if (!TryPreparingOutcomeForSecondTap())
                PrepareOutcomeParameterSetForCid(); // S11.114 - S11.115

            // S11.116 - S11.118
            if (TryWaitingForPutDataResponseAfterGeneratingAc(session.GetKernelSessionId()))
                return WaitingForPutDataResponseAfterGenerateAc.StateId;

            // S11.118.1 S11.121
            HandleOutMessage(session);

            return Idle.StateId;
        }

        #region S11.110

        /// <remarks>EMV Book C-2 Section S11.110</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private void BuildDataRecord()
        {
            _Database.SetIsDataRecordPresent(true);
            _Database.CreateEmvDataRecord(_DataExchangeKernelService);
        }

        #endregion

        #region S11.111 - S11.113

        /// <remarks>EMV Book C-2 Section S11.111 - S11.113</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private bool TryPreparingOutcomeForSecondTap()
        {
            PosCardholderInteractionInformation pcii = _Database.Get<PosCardholderInteractionInformation>(PosCardholderInteractionInformation.Tag);

            if (!pcii.IsSecondTapNeeded())
                return false;

            PrepareOutcomeParameterSetForPcii();
            AttemptToSetPciiDisplayMessage();

            return true;
        }

        #endregion

        #region S11.112

        /// <remarks>EMV Book C-2 Section S11.112</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private void PrepareOutcomeParameterSetForPcii()
        {
            _Database.Update(StatusOutcomes.EndApplication);
            _Database.Update(StartOutcomes.B);
        }

        #endregion

        #region S11.113

        /// <remarks>EMV Book C-2 Section S11.113</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private void AttemptToSetPciiDisplayMessage()
        {
            PhoneMessageTable phoneMessageTable = _Database.Get<PhoneMessageTable>(PhoneMessageTable.Tag);
            PosCardholderInteractionInformation pcii = _Database.Get<PosCardholderInteractionInformation>(PosCardholderInteractionInformation.Tag);

            if (!phoneMessageTable.TryGetMatch(pcii, out MessageTableEntry? messageTableEntry))
                return;

            // TODO: I'm not sure if the below line is correct - Where do we get the MessageHoldTime from?
            _Database.Update(_Database.Get<MessageHoldTime>(MessageHoldTime.Tag));
            _Database.Update(messageTableEntry!.GetStatus());
        }

        #endregion

        #region S11.114 - S11.115

        /// <remarks>EMV Book C-2 Section S11.114 - S11.115</remarks>
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

        #region S11.114 - S11.115 Transaction Cryptogram

        /// <remarks>EMV Book C-2 Section S11.114 - S11.115 Transaction Cryptogram</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private void PrepareOutcomeForTransactionCryptogram()
        {
            _Database.Update(StatusOutcomes.Approved);
            _Database.Update(_Database.Get<MessageHoldTime>(MessageHoldTime.Tag));

            if (_Database.TryGet(BalanceReadAfterGenAc.Tag, out BalanceReadAfterGenAc? balanceReadAfterGenAc))
            {
                _Database.Update(ValueQualifiers.Balance);
                _Database.Update(balanceReadAfterGenAc!);

                if (_Database.TryGet(ApplicationCurrencyCode.Tag, out ApplicationCurrencyCode? currencyCode))
                    _Database.Update(currencyCode!);
            }

            if (_Database.GetOutcomeParameterSet().GetCvmPerformed() == CvmPerformedOutcome.ObtainSignature)
                _Database.Update(DisplayMessageIdentifiers.ApprovedPleaseSign);
            else
                _Database.Update(DisplayMessageIdentifiers.Approved);
        }

        #endregion

        #region S11.114 - S11.115 Application Request Cryptogram

        /// <remarks>EMV Book C-2 Section S11.114 - S11.115 Application Request Cryptogram</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private void PrepareOutcomeForApplicationRequestCryptogram()
        {
            _Database.Update(StatusOutcomes.OnlineRequest);
            _Database.Update(MessageHoldTime.MinimumValue);
            _Database.Update(DisplayMessageIdentifiers.AuthorizingPleaseWait);
        }

        #endregion

        #region S11.114 - S11.115 Application Authentication Cryptogram

        /// <remarks>EMV Book C-2 Section S11.114 - S11.115 Application Authentication Cryptogram</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private void PrepareOutcomeForApplicationAuthenticationCryptogram()
        {
            if (!_Database.IsPurchaseTransaction() && !_Database.IsCashTransaction())
            {
                _Database.Update(MessageHoldTime.MinimumValue);
                _Database.Update(DisplayMessageIdentifiers.ClearDisplay);

                return;
            }

            _Database.Update(_Database.Get<MessageHoldTime>(MessageHoldTime.Tag));

            if (!_Database.IsIcWithContactsSupported())
            {
                _Database.Update(StatusOutcomes.Declined);

                return;
            }

            if (!_Database.TryGet(ThirdPartyData.Tag, out ThirdPartyData? thirdPartyData))
            {
                _Database.Update(StatusOutcomes.TryAnotherInterface);

                return;
            }

            if (!thirdPartyData!.GetUniqueIdentifier().IsBitSet(16))
            {
                _Database.Update(StatusOutcomes.TryAnotherInterface);

                return;
            }

            if (thirdPartyData.TryGetDeviceType(out ushort? deviceType))
            {
                _Database.Update(StatusOutcomes.TryAnotherInterface);

                return;
            }

            if (deviceType == DeviceTypes.Card)
            {
                _Database.Update(StatusOutcomes.TryAnotherInterface);

                return;
            }

            _Database.Update(StatusOutcomes.Declined);
        }

        #endregion

        #region S11.116 - S11.118

        /// <remarks>EMV Book C-2 Section S11.116 - S11.118</remarks>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="IccProtocolException"></exception>
        private bool TryWaitingForPutDataResponseAfterGeneratingAc(KernelSessionId sessionId)
        {
            if (!_DataExchangeKernelService.IsEmpty(DekResponseType.TagsToWriteAfterGenAc))
                return false;

            if (!_DataExchangeKernelService.TryPeek(DekResponseType.TagsToWriteBeforeGenAc, out PrimitiveValue? tagToWrite))
                return false;

            PutDataRequest capdu = PutDataRequest.Create(sessionId.GetTransactionSessionId(), tagToWrite!);
            _EndpointClient.Send(capdu);

            return true;
        }

        #endregion

        #region S11.118.1 S11.121

        /// <remarks>EMV Book C-2 Section S11.118.1 S11.121</remarks>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private void HandleOutMessage(Kernel2Session session)
        {
            if (!_Database.TryGet(PosCardholderInteractionInformation.Tag, out PosCardholderInteractionInformation? pcii))
            {
                HandleDisplayMessage(session);

                return;
            }

            if (!pcii!.IsSecondTapNeeded())
            {
                HandleDisplayMessage(session);

                return;
            }

            HandleDisplayMessageForSecondTapNeeded(session);
        }

        #endregion

        #region S11.119

        /// <remarks>EMV Book C-2 Section S11.119</remarks>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private void HandleDisplayMessage(Kernel2Session session)
        {
            _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);
            _Database.SetUiRequestOnRestartPresent(true);

            _EndpointClient.Send(new OutKernelResponse(session.GetCorrelationId(), session.GetKernelSessionId(), _Database.GetTransaction()));
        }

        #endregion

        #region S11.119 - S11.120

        /// <remarks>EMV Book C-2 Section S11.119 - S11.120</remarks>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private void HandleDisplayMessageForSecondTapNeeded(Kernel2Session session)
        {
            _EndpointClient.Send(new DisplayMessageRequest(_Database.GetUserInterfaceRequestData()));
            _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);
            _Database.SetUiRequestOnOutcomePresent(true);
            _Database.Update(DisplayStatuses.ReadyToRead);
            _Database.Update(MessageHoldTime.MinimumValue);

            _EndpointClient.Send(new OutKernelResponse(session.GetCorrelationId(), session.GetKernelSessionId(), _Database.GetTransaction()));
        }

        #endregion

        #endregion
    }
}