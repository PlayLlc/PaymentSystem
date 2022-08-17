using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGenerateAcResponse2
{
    private partial class ResponseHandler
    {
        #region Instance Members

        #region Cam Failed Response - S11.46 - S11.46.1

        /// <remarks>EMV Book C-2 Section S11.46 - S11.46.1</remarks>
        /// <exception cref="TerminalDataException"></exception>
        public void ProcessCamFailedResponse(KernelSessionId sessionId, TornRecord tempTornRecord)
        {
            _Database.Update(Level2Error.CryptographicAuthenticationMethodFailed);
            _Database.Set(TerminalVerificationResultCodes.CombinationDataAuthenticationFailed);

            ProcessInvalidDataResponse(sessionId, tempTornRecord);
        }

        #endregion

        #region Invalid Data Response - S11.90 - S11.95

        /// <remarks>EMV Book C-2 S11.90 - S11.95</remarks>
        /// <exception cref="TerminalDataException"></exception>
        public void ProcessInvalidDataResponse(KernelSessionId sessionId, TornRecord tempTornRecord)
        {
            // S11.90
            SetDisplayMessage();

            // S11.91 - S11.92
            HandleTornTempRecord(tempTornRecord);

            // S11.93 - S11.94
            if (TryHandleOutcomeWithIdsWriteFlag(sessionId))
                return;

            // S11.95
            HandleInvalidOutcome(sessionId);
        }

        #endregion

        #region Invalid Write Response - S11.101 - S11.102

        /// <remarks>EMV Book C-2 S11.101 - S11.102</remarks>
        /// <exception cref="TerminalDataException"></exception>
        public void ProcessInvalidWriteResponse(KernelSessionId sessionId)
        {
            SetDisplayMessage();

            HandleInvalidOutcome(sessionId);
        }

        #endregion

        #region S11.91 - S11.92

        /// <remarks>EMV Book C-2 S11.91 - S11.92</remarks>
        private void HandleTornTempRecord(TornRecord tempTornRecord)
        {
            if (tempTornRecord.TryGetRecordItem(IntegratedDataStorageStatus.Tag, out PrimitiveValue? tornIdsStatus))
            {
                throw new TerminalDataException(
                    $"The {nameof(ResponseHandler)} could not retrieve the expected {nameof(IntegratedDataStorageStatus)} from the temporary {nameof(TornRecord)}");
            }

            _Database.Update(tempTornRecord);
        }

        #endregion

        #region S11.90, S11.101

        /// <remarks>EMV Book C-2 S11.90, S11.101</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private void SetDisplayMessage()
        {
            _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
            _Database.Update(Statuses.NotReady);
            _Database.Update(_Database.Get<MessageHoldTime>(MessageHoldTime.Tag));
        }

        #endregion

        #region S11.93 - S11.94

        /// <remarks>EMV Book C-2 S11.93 - S11.94</remarks>
        private bool TryHandleOutcomeWithIdsWriteFlag(KernelSessionId sessionId)
        {
            try
            {
                if (!_Database.Get<IntegratedDataStorageStatus>(IntegratedDataStorageStatus.Tag).IsWriteSet())
                    return false;

                _Database.Update(StatusOutcomes.EndApplication);
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
                _EndpointClient.Send(new StopKernelRequest(sessionId));
            }

            return true;
        }

        #endregion

        #region S11.95, S11.102

        /// <remarks>EMV Book C-2 S11.95</remarks>
        private void HandleInvalidOutcome(KernelSessionId sessionId)
        {
            try
            {
                _Database.Update(StatusOutcomes.EndApplication);
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
                _EndpointClient.Send(new StopKernelRequest(sessionId));
            }
        }

        #endregion

        #endregion
    }
}