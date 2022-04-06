using System;

using Play.Ber.DataObjects;
using Play.Core.Extensions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Icc.Exceptions;

namespace Play.Emv.Kernel2.StateMachine._Temp
{
    public partial class _S910
    {
        private class ResponseHandler
        {
            #region Instance Values

            private readonly KernelDatabase _Database;
            private readonly DataExchangeKernelService _DataExchangeKernelService;
            private readonly IKernelEndpoint _KernelEndpoint;
            private readonly IHandlePcdRequests _PcdEndpoint;

            #endregion

            #region Invalid Responses

            #region S910.7.1 - S910.7.2

            public ResponseHandler(
                KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IKernelEndpoint kernelEndpoint,
                IHandlePcdRequests pcdEndpoint)
            {
                _Database = database;
                _DataExchangeKernelService = dataExchangeKernelService;
                _KernelEndpoint = kernelEndpoint;
                _PcdEndpoint = pcdEndpoint;
            }

            /// <remarks>EMV Book C-2 Section S910.7.1 - S910.7.2</remarks>
            /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
            public void HandleCamFailed(KernelSessionId sessionId)
            {
                _Database.Update(Level2Error.CryptographicAuthenticationMethodFailed);
                _Database.Set(TerminalVerificationResultCodes.CombinationDataAuthenticationFailed);

                ProcessInvalidDataResponse(sessionId);
            }

            #endregion

            #region S910.51 - S910.52

            /// <remarks>EMV Book C-2 Section S910.51 - S910.52</remarks>
            /// <exception cref="TerminalDataException"></exception>
            public void ProcessInvalidDataResponse(KernelSessionId sessionId)
            {
                if (!_Database.IsIdsAndTtrImplemented())
                {
                    HandleInvalidOutcome(sessionId);

                    return;
                }

                if (!_Database.TryGet(IntegratedDataStorageStatus.Tag, out IntegratedDataStorageStatus? idsStatus))
                {
                    HandleInvalidOutcome(sessionId);

                    return;
                }

                if (!idsStatus!.IsWriteSet())
                {
                    HandleInvalidOutcome(sessionId);

                    return;
                }

                HandleOutcomeWithIdsWriteFlag(sessionId);
            }

            #endregion

            #region S910.61 - S910.62

            /// <remarks>EMV Book C-2 Section S910.50 - S910.53</remarks>
            /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
            public void ProcessInvalidWriteResponse(KernelSessionId sessionId)
            {
                SetDisplayMessage();

                HandleInvalidOutcome(sessionId);
            }

            #endregion

            #region S910.50

            /// <remarks>EMV Book C-2 Section S910.50</remarks>
            /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
            private void SetDisplayMessage()
            {
                _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
                _Database.Update(Status.NotReady);
                _Database.Update(_Database.Get<MessageHoldTime>(MessageHoldTime.Tag));
            }

            #endregion

            #region S910.52

            /// <remarks>EMV Book C-2 Section S910.52</remarks>
            private void HandleOutcomeWithIdsWriteFlag(KernelSessionId sessionId)
            {
                try
                {
                    _Database.Update(StatusOutcome.EndApplication);
                    _Database.Update(MessageOnErrorIdentifiers.ErrorUseAnotherCard);
                    _Database.SetIsDataRecordPresent(true);
                    _Database.CreateEmvDataRecord(_DataExchangeKernelService);
                    _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);
                    _Database.SetUiRequestOnRestartPresent(true);
                }
                catch (TerminalDataException)
                {
                    // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
                }
                catch (Exception)
                {
                    // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
                }
                finally
                {
                    _KernelEndpoint.Request(new StopKernelRequest(sessionId));
                }
            }

            #endregion

            #region S910.53

            /// <remarks>EMV Book C-2 Section S910.53</remarks>
            private void HandleInvalidOutcome(KernelSessionId sessionId)
            {
                try
                {
                    _Database.Update(StatusOutcome.EndApplication);
                    _Database.Update(MessageOnErrorIdentifiers.ErrorUseAnotherCard);
                    _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);
                    _Database.SetUiRequestOnRestartPresent(true);
                }
                catch (TerminalDataException)
                {
                    // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
                }
                catch (Exception)
                {
                    // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
                }
                finally
                {
                    _KernelEndpoint.Request(new StopKernelRequest(sessionId));
                }
            }

            #endregion

            #endregion

            #region Valid Responses

            /// <exception cref="TerminalDataException"></exception>
            /// <exception cref="DataElementParsingException"></exception>
            /// <exception cref="IccProtocolException"></exception>
            public StateId HandleValidResponse(IGetKernelStateId currentStateIdRetriever, KernelSessionId sessionId)
            {
                // S910.70
                BuildDataRecord();

                // S910.71 - S910.75
                if (!TryPreparingOutcomeForSecondTap())
                    PrepareOutcomeParameterSetForCid();

                if (TryWaitingForPutDataResponseAfterGeneratingAc(sessionId))
                    return WaitingForPutDataResponseAfterGenerateAc.StateId;

                HandleOutMessage();

                return currentStateIdRetriever.GetStateId();
            }

            #region S910.70

            /// <exception cref="TerminalDataException"></exception>
            private void BuildDataRecord()
            {
                _Database.SetIsDataRecordPresent(true);
                _Database.CreateEmvDataRecord(_DataExchangeKernelService);
            }

            #endregion

            #region S910.71

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

            /// <exception cref="TerminalDataException"></exception>
            private void PrepareOutcomeParameterSetForPcii()
            {
                _Database.Update(StatusOutcome.EndApplication);
                _Database.Update(StartOutcome.B);
            }

            #endregion

            #region S910.73

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

            /// <exception cref="DataElementParsingException"></exception>
            /// <exception cref="TerminalDataException"></exception>
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

            private void HandleOutMessage()
            { }

            #endregion

            #endregion
        }
    }
}