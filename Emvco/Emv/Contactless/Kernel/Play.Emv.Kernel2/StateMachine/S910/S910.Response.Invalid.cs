using System;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;

namespace Play.Emv.Kernel2.StateMachine;

internal partial class S910
{
    private partial class ResponseHandler
    {
        #region Instance Members

        #region S910.7.1 - S910.7.2

        /// <remarks>EMV Book C-2 Section S910.7.1 - S910.7.2</remarks>
        /// <exception cref="TerminalDataException"></exception>
        public void ProcessCamFailedResponse(KernelSessionId sessionId)
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
        /// <exception cref="TerminalDataException"></exception>
        public void ProcessInvalidWriteResponse(KernelSessionId sessionId)
        {
            SetDisplayMessage();

            HandleInvalidOutcome(sessionId);
        }

        #endregion

        #region S910.50

        /// <remarks>EMV Book C-2 Section S910.50</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private void SetDisplayMessage()
        {
            _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
            _Database.Update(Statuses.NotReady);
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
                _Database.SetUiRequestOnOutcomePresent(true);
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
                _Database.SetUiRequestOnOutcomePresent(true);
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
    }
}