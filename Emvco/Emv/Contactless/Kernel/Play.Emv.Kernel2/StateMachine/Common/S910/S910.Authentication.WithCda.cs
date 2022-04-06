using System;

using Play.Core.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Security;
using Play.Emv.Security.Exceptions;

namespace Play.Emv.Kernel2.StateMachine
{
    public partial class S910
    {
        private partial class AuthenticationHandler
        {
            /// <exception cref="PlayInternalException"></exception>
            /// <exception cref="Play.Core.Exceptions"></exception>
            public StateId ProcessWithCda(
                IGetKernelStateId currentStateIdRetriever, Kernel2Session session, GenerateApplicationCryptogramResponse rapdu)
            {
                // S910.1
                if (TryRetrievePublicKeys(currentStateIdRetriever, session, rapdu, session.GetStaticDataToBeAuthenticated()))
                    return currentStateIdRetriever.GetStateId();

                _Database.TryGet(IntegratedDataStorageStatus.Tag, out IntegratedDataStorageStatus? integratedDataStorageStatus);

                // S910.2, S910.2.2 - S910.3.1
                if (TryHandlingIntegratedDataStorageError(currentStateIdRetriever, session.GetKernelSessionId(),
                                                          integratedDataStorageStatus))
                    return currentStateIdRetriever.GetStateId();

                // S910.2.1, S910.4 - S910.4.1
                if (TryHandlingStandaloneDataStorageError(currentStateIdRetriever, session.GetKernelSessionId()))
                    return currentStateIdRetriever.GetStateId();

                // S910.5, S910.6
                if (IsIdsReadFlagSet(integratedDataStorageStatus))
                    return _ResponseHandler.HandleValidResponse(currentStateIdRetriever, session);

                // S910.8 - S910.9
                if (TryHandlingMissingDataSummary2(currentStateIdRetriever, session.GetKernelSessionId()))
                    return currentStateIdRetriever.GetStateId();

                if (TryHandlingInvalidDataStorageSummary1And2Equality(session.GetKernelSessionId()))
                    return currentStateIdRetriever.GetStateId();

                // S910.12
                SetSuccessfulRead();

                // S910.13
                if (TryHandleIdsWriteFlagNotSet(currentStateIdRetriever, session, out StateId? successfulResponseStateId))
                    return successfulResponseStateId!.Value;

                // S910.14 - S910.15
                if (TryHandlingMissingDataSummary3(session.GetKernelSessionId()))
                    return currentStateIdRetriever.GetStateId();

                if (TryHandlingInvalidDataStorageSummary2And3Equality(session.GetKernelSessionId()))
                    return currentStateIdRetriever.GetStateId();

                return _ResponseHandler.HandleValidResponse(currentStateIdRetriever, session);
            }

            #region S910.1, S910.4, S910.4.1, S910.3, S910.3.1

            /// <summary>
            ///     This method Retrieves the required Public Keys, updates the database with the recovered data, and validates the
            ///     signature of the dynamic data
            /// </summary>
            /// <exception cref="TerminalDataException"></exception>
            private bool TryRetrievePublicKeys(
                IGetKernelStateId currentStateIdRetriever, Kernel2Session session, GenerateApplicationCryptogramResponse rapdu,
                StaticDataToBeAuthenticated staticDataToBeAuthenticated)
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
                    _ResponseHandler.HandleCamFailed(session.GetKernelSessionId());

                    return false;
                }
                catch (Exception)
                {
                    // TODO: Log exception. We need to make sure we stop execution of the transaction but don't terminate the application due to an unhandled exception
                    _ResponseHandler.HandleCamFailed(session.GetKernelSessionId());

                    return false;
                }
            }

            #endregion

            #region S910.2, S910.2.2 - S910.3.1

            /// <exception cref="TerminalDataException"></exception>
            private bool TryHandlingIntegratedDataStorageError(
                IGetKernelStateId currentStateIdRetriever, KernelSessionId sessionId,
                IntegratedDataStorageStatus? integratedDataStorageStatus)
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
                    _ResponseHandler.ProcessInvalidDataResponse(sessionId);

                    return true;
                }

                if (applicationCapabilitiesInformation.GetDataStorageVersionNumber() != DataStorageVersionNumbers.Version2)
                    return false;

                if (!IsRelayResistantDataStorageVersion2DataValid())
                {
                    _ResponseHandler.ProcessInvalidDataResponse(sessionId);

                    return true;
                }

                return false;
            }

            #endregion

            #region S910.2

            /// <exception cref="TerminalDataException"></exception>
            private bool IsIdsReadFlagSet(IntegratedDataStorageStatus? integratedDataStorageStatus)
            {
                if (!_Database.IsIdsAndTtrImplemented())
                    return false;

                if (integratedDataStorageStatus is null)
                    return false;

                if (!integratedDataStorageStatus!.IsReadSet())
                    return false;

                return true;
            }

            #endregion

            #region S910.3.1, S910.4.1

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

            #region S910.3.1 continued

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
                if (summary3.GetValueByteCount() != 16)
                    return false;

                return true;
            }

            #endregion

            #region S910.2.1, S910.4 - S910.4.1

            /// <exception cref="TerminalDataException"></exception>
            private bool TryHandlingStandaloneDataStorageError(IGetKernelStateId currentStateIdRetriever, KernelSessionId sessionId)
            {
                if (_Database.IsSet(TerminalVerificationResultCodes.RelayResistancePerformed))
                    return false;

                if (!IsMandatoryRelayResistantDataPresent())
                {
                    _ResponseHandler.ProcessInvalidDataResponse(sessionId);

                    return true;
                }

                return false;
            }

            #endregion

            #region S910.8 - S910.9

            /// <exception cref="TerminalDataException"></exception>
            private bool TryHandlingMissingDataSummary2(IGetKernelStateId currentStateIdRetriever, KernelSessionId sessionId)
            {
                if (_Database.IsPresentAndNotEmpty(DataStorageSummary2.Tag))
                    return false;

                _Database.Update(Level2Error.CardDataMissing);
                _ResponseHandler.ProcessInvalidDataResponse(sessionId);

                return true;
            }

            #endregion

            #region S910.10 - S910.11

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

            #region S910.12

            /// <exception cref="TerminalDataException"></exception>
            private void SetSuccessfulRead()
            {
                _Database.SetReadIsSuccessful(true);
            }

            #endregion

            #region S910.13

            /// <exception cref="TerminalDataException"></exception>
            private bool TryHandleIdsWriteFlagNotSet(
                IGetKernelStateId currentStateIdRetriever, Kernel2Session session, out StateId? successfulResponseStateId)
            {
                if (_Database.IsIntegratedDataStorageWriteFlagSet())
                {
                    successfulResponseStateId = null;

                    return false;
                }

                successfulResponseStateId = _ResponseHandler.HandleValidResponse(currentStateIdRetriever, session);

                return true;
            }

            #endregion

            #region S910.14 - S910.15

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

            #region S910.16, S910.18 - S910.19

            /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
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
        }
    }
}