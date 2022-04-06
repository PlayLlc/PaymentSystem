using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Exceptions;
using Play.Emv.DataExchange;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Messaging;

using MessageIdentifier = Play.Emv.Ber.MessageIdentifier;

namespace Play.Emv.Kernel2.StateMachine
{
    public partial class S910
    {
        private class InvalidResponseHandler
        {
            #region Instance Values

            private readonly KernelDatabase _Database;
            private readonly DataExchangeKernelService _DataExchangeKernelService;
            private readonly IKernelEndpoint _KernelEndpoint;

            #endregion

            #region Constructor

            public InvalidResponseHandler(
                KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IKernelEndpoint kernelEndpoint)
            {
                _Database = database;
                _DataExchangeKernelService = dataExchangeKernelService;
                _KernelEndpoint = kernelEndpoint;
            }

            #endregion

            #region S910.50 - S910.53

            /// <exception cref="TerminalDataException"></exception>
            public StateId ProcessInvalidResponse1(IGetKernelStateId currentStateIdRetriever, KernelSessionId sessionId)
            {
                SetInvalidDisplayMessage();

                HandleInvalidResponse(sessionId);

                return currentStateIdRetriever.GetStateId();
            }

            #endregion

            #region S910.50, S910.61

            /// <exception cref="TerminalDataException"></exception>
            private void SetInvalidDisplayMessage()
            {
                _Database.Update(MessageIdentifier.ErrorUseAnotherCard);
                _Database.Update(Status.NotReady);
                _Database.Update(_Database.Get<MessageHoldTime>(MessageHoldTime.Tag));
            }

            #endregion

            #region S910.51 - S910.52

            /// <exception cref="TerminalDataException"></exception>
            private void HandleInvalidResponse(KernelSessionId sessionId)
            {
                if (!_Database.IsIdsAndTtrImplemented())
                {
                    HandleInvalidResponseOutcome(sessionId);

                    return;
                }

                if (!_Database.TryGet(IntegratedDataStorageStatus.Tag, out IntegratedDataStorageStatus? idsStatus))
                {
                    HandleInvalidResponseOutcome(sessionId);

                    return;
                }

                if (!idsStatus!.IsWriteSet())
                {
                    HandleInvalidResponseOutcome(sessionId);

                    return;
                }

                HandleOutcomeWithIdsWriteFlag(sessionId);
            }

            #endregion

            #region S910.52

            private void HandleOutcomeWithIdsWriteFlag(KernelSessionId sessionId)
            {
                try
                {
                    _Database.Update(StatusOutcome.EndApplication);
                    _Database.Update(MessageOnErrorIdentifier.ErrorUseAnotherCard);
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

            private void HandleInvalidResponseOutcome(KernelSessionId sessionId)
            {
                try
                {
                    _Database.Update(StatusOutcome.EndApplication);
                    _Database.Update(MessageOnErrorIdentifier.ErrorUseAnotherCard);
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

            #region S910.61 - S910.62

            /// <exception cref="TerminalDataException"></exception>
            public StateId ProcessInvalidResponse2(IGetKernelStateId currentStateIdRetriever, KernelSessionId sessionId)
            {
                SetInvalidDisplayMessage();

                HandleInvalidResponseOutcome(sessionId);

                return currentStateIdRetriever.GetStateId();
            }

            #endregion
        }
    }
}