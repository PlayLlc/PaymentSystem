using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Messaging;

using MessageIdentifier = Play.Emv.Ber.MessageIdentifier;

namespace Play.Emv.Kernel2.StateMachine
{
    public partial class S910
    {
        #region S910.50 - S910.53

        /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
        private StateId ProcessInvalidResponse1(IGetKernelStateId currentStateIdRetriever, KernelSessionId sessionId)
        {
            SetDisplayMessage();

            HandleInvalidResponse(sessionId);

            return currentStateIdRetriever.GetStateId();
        }

        #endregion

        #region S910.50

        /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
        private void SetDisplayMessage()
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
                HandleOutcome(sessionId);

                return;
            }

            if (!_Database.TryGet(IntegratedDataStorageStatus.Tag, out IntegratedDataStorageStatus? idsStatus))
            {
                HandleOutcome(sessionId);

                return;
            }

            if (!idsStatus!.IsWriteSet())
            {
                HandleOutcome(sessionId);

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

        private void HandleOutcome(KernelSessionId sessionId)
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
    }
}