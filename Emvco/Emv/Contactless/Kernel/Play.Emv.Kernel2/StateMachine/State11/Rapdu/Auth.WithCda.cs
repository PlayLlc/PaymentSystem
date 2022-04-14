using System;

using Play.Ber.DataObjects;
using Play.Core.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Security;
using Play.Emv.Security.Exceptions;
using Play.Icc.Exceptions;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGenerateAcResponse2
{
    private partial class AuthHandler
    {
        #region Instance Members

        /// <exception cref="PlayInternalException"></exception>
        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="IccProtocolException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public StateId ProcessWithCda(Kernel2Session session, GenerateApplicationCryptogramResponse rapdu)
        {
            if (session.TryGetTornEntry(out TornEntry? result))
            {
                throw new TerminalDataException(
                    $"The {nameof(AuthHandler)} could not {nameof(ProcessWithCda)} because the expected {nameof(TornEntry)} could not be retrieved from the {nameof(Kernel2Session)}");
            }

            if (!_Database.TryGet(result!, out TornRecord? tempTornRecord))
            {
                throw new TerminalDataException(
                    $"The {nameof(AuthHandler)} could not {nameof(ProcessWithCda)} because the expected temporary {nameof(TornRecord)} could not be retrieved from the {nameof(TornTransactionLog)}");
            }

            // S11.40
            if (!TryRetrievePublicKeys(session, rapdu, session.GetStaticDataToBeAuthenticated()))
                return StateId;

            _Database.TryGet(IntegratedDataStorageStatus.Tag, out IntegratedDataStorageStatus? integratedDataStorageStatus);

            // S11.41 - S11.41.2
            if (TryHandlingIntegratedDataStorageError(session.GetKernelSessionId(), integratedDataStorageStatus))
                return StateId;

            // S11.41.1 - S11.43.1
            if (TryHandlingStandaloneDataStorageError(session.GetKernelSessionId()))
                return StateId;

            // S11.47
            if (IsIdsReadFlagSet(integratedDataStorageStatus))
                return _ResponseHandler.HandleValidResponse(session);

            // S11.47 - S11.49
            if (TryHandlingInvalidDataSummary1Equality(session.GetKernelSessionId(), tempTornRecord!))
                return StateId;

            // S11.50 - S11.51
            if (TryHandlingMissingDataSummary2(session.GetKernelSessionId()))
                return StateId;

            // S11.52 - S11.53
            if (TryHandlingInvalidDataStorageSummary1And2Equality(session.GetKernelSessionId()))
                return StateId;

            // S11.54
            SetSuccessfulRead();

            // S11.55
            if (TryHandleIdsWriteFlagNotSet(session, out StateId? successfulResponseStateId))
                return successfulResponseStateId!.Value;

            // S11.56 - S11.57
            if (TryHandlingMissingDataSummary3(session.GetKernelSessionId()))
                return StateId;

            // S11.58 - S11.59
            if (TryHandlingInvalidDataStorageSummary2And3Equality(session.GetKernelSessionId()))
                return StateId;

            // S11.60
            return TryHandlingStopIfWriteFailed(session) ? StateId : _ResponseHandler.HandleValidResponse(session);
        }

        #region S11.40

        /// <remarks>EMV Book C-2 Section S11.40</remarks>
        /// <summary>
        ///     This method Retrieves the required Public Keys, updates the database with the recovered data, and validates the
        ///     signature of the dynamic data
        /// </summary>
        /// <exception cref="TerminalDataException"></exception>
        private bool TryRetrievePublicKeys(
            Kernel2Session session, GenerateApplicationCryptogramResponse rapdu, StaticDataToBeAuthenticated staticDataToBeAuthenticated)
        {
            try
            {
                // Retrieves the Issuer PK, ICC PK, and validates the SDAD
                _AuthenticationService.AuthenticateFirstCda(_Database, _Database, rapdu, staticDataToBeAuthenticated);

                return true;
            }
            catch (CryptographicAuthenticationMethodFailedException)
            {
                // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
                _ResponseHandler.ProcessCamFailedResponse(session.GetKernelSessionId());

                return false;
            }
            catch (Exception)
            {
                // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
                _ResponseHandler.ProcessCamFailedResponse(session.GetKernelSessionId());

                return false;
            }
        }

        #endregion

        #region S11.41 - S11.41.2

        /// <remarks>EMV Book C-2 Section S11.41 - S11.41.2</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private bool TryHandlingIntegratedDataStorageError(
            KernelSessionId sessionId, IntegratedDataStorageStatus? integratedDataStorageStatus)
        {
            if (!IsIdsReadFlagSet(integratedDataStorageStatus))
                return false;

            // If RRP was not performed, then we will not fail due to data storage summary values
            if (_Database.IsSet(TerminalVerificationResultCodes.RelayResistancePerformed))
                return false;

            ApplicationCapabilitiesInformation applicationCapabilitiesInformation =
                _Database.Get<ApplicationCapabilitiesInformation>(ApplicationCapabilitiesInformation.Tag);

            if (!IsMandatoryRelayResistantDataPresent())
            {
                _ResponseHandler.ProcessCamFailedResponse(sessionId);

                return true;
            }

            if (applicationCapabilitiesInformation.GetDataStorageVersionNumber() != DataStorageVersionNumbers.Version2)
                return false;

            if (IsRelayResistantDataStorageVersion2DataValid())
                return false;

            _ResponseHandler.ProcessCamFailedResponse(sessionId);

            return true;
        }

        #endregion

        #region S11.41

        /// <remarks>EMV Book C-2 Section S11.41</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private bool IsIdsReadFlagSet(IntegratedDataStorageStatus? integratedDataStorageStatus)
        {
            if (!_Database.IsIdsAndTtrImplemented())
                return false;

            if (integratedDataStorageStatus is null)
                return false;

            return integratedDataStorageStatus!.IsReadSet();
        }

        #endregion

        #region S11.42.1, S11.43.1

        /// <remarks>EMV Book C-2 Section S11.42.1, S11.43.1</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private bool IsMandatoryRelayResistantDataPresent()
        {
            if (!_Database.IsPresentAndNotEmpty(TerminalRelayResistanceEntropy.Tag))
                return false;
            if (!_Database.IsPresentAndNotEmpty(DeviceRelayResistanceEntropy.Tag))
                return false;
            if (!_Database.IsPresentAndNotEmpty(MinTimeForProcessingRelayResistanceApdu.Tag))
                return false;
            if (!_Database.IsPresentAndNotEmpty(MaxTimeForProcessingRelayResistanceApdu.Tag))
                return false;
            if (!_Database.IsPresentAndNotEmpty(DeviceEstimatedTransmissionTimeForRelayResistanceRapdu.Tag))
                return false;

            return true;
        }

        #endregion

        #region S11.42.1

        /// <remarks>EMV Book C-2 Section S11.42.1</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private bool IsRelayResistantDataStorageVersion2DataValid()
        {
            if (!_Database.IsPresentAndNotEmpty(DataStorageSummary2.Tag))
                return false;
            if (!_Database.IsPresentAndNotEmpty(DataStorageSummary3.Tag))
                return false;

            DataStorageSummary2 summary2 = _Database.Get<DataStorageSummary2>(DataStorageSummary2.Tag);
            DataStorageSummary3 summary3 = _Database.Get<DataStorageSummary3>(DataStorageSummary3.Tag);

            if (summary2.GetValueByteCount() != 16)
                return false;

            return summary3.GetValueByteCount() == 16;
        }

        #endregion

        #region S11.41.1 - S11.43.1

        /// <remarks>EMV Book C-2 Section S11.41.1 - S11.43.1</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private bool TryHandlingStandaloneDataStorageError(KernelSessionId sessionId)
        {
            // Verify SDAD is accomplished at the same time as certification recover - S11.40

            if (!_Database.IsSet(TerminalVerificationResultCodes.RelayResistancePerformed))
                return false;

            if (IsMandatoryRelayResistantDataPresent())
                return false;

            _ResponseHandler.ProcessCamFailedResponse(sessionId);

            return true;
        }

        #endregion

        #region S11.47 - S11.49

        /// <remarks>EMV Book C-2 Section S11.47 - S11.49</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private bool TryHandlingInvalidDataSummary1Equality(KernelSessionId sessionId, TornRecord tempTornRecord)
        {
            DataStorageSummary1 dsSummary1 = _Database.Get<DataStorageSummary1>(DataStorageSummary1.Tag);

            if (tempTornRecord.TryGetRecordItem(IntegratedDataStorageStatus.Tag, out PrimitiveValue? tornIdsStatus))
            {
                throw new TerminalDataException(
                    $"The {nameof(AuthHandler)} could not retrieve the {nameof(IntegratedDataStorageStatus)} from the temporary {nameof(TornRecord)}");
            }

            if (!((IntegratedDataStorageStatus) tornIdsStatus!).IsWriteSet())
                return false;

            if (!tempTornRecord.TryGetRecordItem(DataStorageSummary1.Tag, out PrimitiveValue? tornDsSummary1))
            {
                throw new TerminalDataException(
                    $"The {nameof(AuthHandler)} could not retrieve the {nameof(DataStorageSummary1)} from the temporary {nameof(TornRecord)}");
            }

            if (dsSummary1 == tornDsSummary1)
                return false;

            HandleInvalidDataSummary1Equality(sessionId);

            return true;
        }

        #endregion

        #region S11.49

        /// <remarks>EMV Book C-2 Section S11.49</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private void HandleInvalidDataSummary1Equality(KernelSessionId sessionId)
        {
            _Database.Update(Level2Error.IdsReadError);
            _ResponseHandler.ProcessInvalidDataResponse(sessionId);
        }

        #endregion

        #region S11.50 - S11.51

        /// <remarks>EMV Book C-2 Section S11.50 - S11.51</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private bool TryHandlingMissingDataSummary2(KernelSessionId sessionId)
        {
            if (_Database.IsPresentAndNotEmpty(DataStorageSummary2.Tag))
                return false;

            _Database.Update(Level2Error.CardDataMissing);
            _ResponseHandler.ProcessInvalidDataResponse(sessionId);

            return true;
        }

        #endregion

        #region S11.52 - S11.53

        /// <remarks>EMV Book C-2 Section S11.52 - S11.53</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private bool TryHandlingInvalidDataStorageSummary1And2Equality(KernelSessionId sessionId)
        {
            DataStorageSummary1 dataStorageSummary1 = _Database.Get<DataStorageSummary1>(DataStorageSummary1.Tag);
            DataStorageSummary2 dataStorageSummary2 = _Database.Get<DataStorageSummary2>(DataStorageSummary2.Tag);

            if (dataStorageSummary1 == dataStorageSummary2)
                return false;

            _Database.Update(Level2Error.IdsReadError);
            _ResponseHandler.ProcessInvalidDataResponse(sessionId);

            return true;
        }

        #endregion

        #region S11.54

        /// <remarks>EMV Book C-2 Section S11.54</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private void SetSuccessfulRead()
        {
            _Database.SetReadIsSuccessful(true);
        }

        #endregion

        #region S11.55

        /// <remarks>EMV Book C-2 Section S11.55</remarks>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="IccProtocolException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private bool TryHandleIdsWriteFlagNotSet(Kernel2Session session, out StateId? successfulResponseStateId)
        {
            if (!_Database.IsIntegratedDataStorageWriteFlagSet())
            {
                successfulResponseStateId = _ResponseHandler.HandleValidResponse(session);

                return true;
            }

            successfulResponseStateId = null;

            return false;
        }

        #endregion

        #region S11.56 - S11.57

        /// <remarks>EMV Book C-2 Section S11.56 - S11.57</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private bool TryHandlingMissingDataSummary3(KernelSessionId sessionId)
        {
            if (_Database.IsPresentAndNotEmpty(DataStorageSummary3.Tag))
                return false;

            _Database.Update(Level2Error.CardDataMissing);
            _ResponseHandler.ProcessInvalidDataResponse(sessionId);

            return true;
        }

        #endregion

        #region S11.58 - S11.59

        /// <remarks>EMV Book C-2 Section S11.58 - S11.59</remarks>
        /// <exception cref="PlayInternalException"></exception>
        private bool TryHandlingInvalidDataStorageSummary2And3Equality(KernelSessionId sessionId)
        {
            DataStorageSummary2 dataStorageSummary2 = _Database.Get<DataStorageSummary2>(DataStorageSummary2.Tag);
            DataStorageSummary3 dataStorageSummary3 = _Database.Get<DataStorageSummary3>(DataStorageSummary3.Tag);

            if (dataStorageSummary2 != dataStorageSummary3)
                return false;

            if (!_Database.TryGet(DataStorageOperatorDataSetInfoForReader.Tag,
                out DataStorageOperatorDataSetInfoForReader? dataStorageOperatorDataSetInfoForReader))
                return false;

            if (!dataStorageOperatorDataSetInfoForReader!.IsStopIfWriteFailedSet())
                return false;

            _Database.Update(Level2Error.IdsWriterError);
            _ResponseHandler.ProcessInvalidWriteResponse(sessionId);

            return true;
        }

        #endregion

        #region S11.60

        /// <remarks>EMV Book C-2 Section S11.60</remarks>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="PlayInternalException"></exception>
        private bool TryHandlingStopIfWriteFailed(Kernel2Session session)
        {
            DataStorageOperatorDataSetInfoForReader dsInfo =
                _Database.Get<DataStorageOperatorDataSetInfoForReader>(DataStorageOperatorDataSetInfoForReader.Tag);

            if (!dsInfo.IsStopIfWriteFailedSet())
                return false;

            HandleStopIfWriteFailed(session.GetKernelSessionId());

            return true;
        }

        #endregion

        #region S11.61

        /// <remarks>EMV Book C-2 Section S11.61</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private void HandleStopIfWriteFailed(KernelSessionId sessionId)
        {
            _Database.Update(Level2Error.IdsWriterError);
            _ResponseHandler.ProcessInvalidWriteResponse(sessionId);
        }

        #endregion

        #endregion
    }
}