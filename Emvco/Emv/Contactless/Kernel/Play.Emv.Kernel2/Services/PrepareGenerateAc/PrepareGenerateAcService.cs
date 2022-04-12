using System;

using Play.Ber.Exceptions;
using Play.Core.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Kernel2.Services.CommonStateLogic;
using Play.Emv.Kernel2.StateMachine;
using Play.Emv.Pcd.Contracts;
using Play.Messaging;

using Exception = System.Exception;

namespace Play.Emv.Kernel2.Services.PrepareGenerateAc
{
    /// <summary>
    ///     Prepares a Generate AC CAPDU for Terminal implementations with IDS capabilities implemented
    /// </summary>
    /// <remarks>Book EMVco C-2 Section 7.6 GAC</remarks>
    public partial class PrepareGenerateAcService : CommonProcessing
    {
        #region Instance Values

        private readonly NoIntegratedDataStorage _NoIds;
        private readonly CdaFailure _CdaFailure;
        private readonly ReadIntegratedDataStorage _ReadIds;
        private readonly WriteIntegratedDataStorage _WriteIds;

        protected override StateId[] _ValidStateIds { get; } =
        {
            WaitingForGpoResponse.StateId, WaitingForExchangeRelayResistanceDataResponse.StateId
        };

        #endregion

        #region Constructor

        public PrepareGenerateAcService(
            KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IGetKernelState kernelStateResolver,
            IHandlePcdRequests pcdEndpoint, IKernelEndpoint kernelEndpoint) : base(database, dataExchangeKernelService, kernelStateResolver,
            pcdEndpoint, kernelEndpoint)
        {
            _CdaFailure = new CdaFailure(database, pcdEndpoint);
            _ReadIds = new ReadIntegratedDataStorage(database, pcdEndpoint);
            _WriteIds = new WriteIntegratedDataStorage(database, pcdEndpoint);
            _NoIds = new NoIntegratedDataStorage(database, pcdEndpoint, _ReadIds);
        }

        #endregion

        #region Instance Members

        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="RequestOutOfSyncException"></exception>
        /// <exception cref="OverflowException"></exception>
        /// <exception cref="BerParsingException"></exception>
        /// <exception cref="CardDataException"></exception>
        public override StateId Process(IGetKernelStateId currentStateIdRetriever, Kernel2Session session, Message message)
        {
            HandleRequestOutOfSync(currentStateIdRetriever.GetStateId());

            // GAC.1
            if (!IsIdsReadFlagSet())
                return _NoIds.Process(currentStateIdRetriever, session, message);

            //  GAC.2
            if (HasCombinationDataAuthenticationFailed())
                return _CdaFailure.Process(currentStateIdRetriever, session, message);

            // GAC.3
            if (IsIntegratedDataStorageReadOnly())
                return _ReadIds.Process(currentStateIdRetriever, session, message);

            // GAC.5 - GAC.6
            if (TryHandlingIdsDataError())
                return currentStateIdRetriever.GetStateId();

            DataStorageOperatorDataSetInfoForReader dsOdsInfoForReader =
                _Database.Get<DataStorageOperatorDataSetInfoForReader>(DataStorageOperatorDataSetInfoForReader.Tag);

            // GAC.7 - GAC-9
            if (!TryHandlingIdsWrite(currentStateIdRetriever, session, message, dsOdsInfoForReader, out StateId? writeIdsFlowStateIdResult))
                return writeIdsFlowStateIdResult!.Value;

            // GAC.10 - GAC13
            if (TryHandlingIdsNoMatchingAc(currentStateIdRetriever, session, message, dsOdsInfoForReader,
                out StateId? idsNoMatchingAcErrorStateId))
                return idsNoMatchingAcErrorStateId!.Value;

            return _ReadIds.Process(currentStateIdRetriever, session, message);
        }

        #region GAC.1

        /// <remarks>EMV Book C-2 GAC.1</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private bool IsIdsReadFlagSet() => _Database.IsIdsAndTtrImplemented() && _Database.IsIntegratedDataStorageReadFlagSet();

        #endregion

        #region GAC.2

        /// <remarks>EMV Book C-2 GAC.2</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private bool HasCombinationDataAuthenticationFailed() =>
            _Database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag).CombinationDataAuthenticationFailed();

        #endregion

        #region GAC.3

        /// <remarks>EMV Book C-2 GAC.3</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private bool IsIntegratedDataStorageReadOnly()
        {
            if (!_Database.IsPresentAndNotEmpty(DataStorageOperatorDataSetInfo.Tag))
                return true;

            if (IsDsdolEmpty())
                return true;

            return false;
        }

        #endregion

        #region GAC.4

        /// <remarks>EMV Book C-2 GAC.4</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private bool IsDsdolEmpty() => _Database.IsPresentAndNotEmpty(DataStorageDataObjectList.Tag);

        #endregion

        #region GAC.5 - GAC.6

        /// <remarks>EMV Book C-2 GAC.5 - GAC.6</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private bool TryHandlingIdsDataError()
        {
            if (!_Database.IsPresentAndNotEmpty(DataStorageApplicationCryptogramType.Tag))
            {
                HandleIdsDataError();

                return true;
            }

            if (!_Database.IsPresentAndNotEmpty(DataStorageOperatorDataSetInfoForReader.Tag))
            {
                HandleIdsDataError();

                return true;
            }

            return false;
        }

        #endregion

        #region GAC.6

        /// <remarks>EMV Book C-2 GAC.6</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private void HandleIdsDataError()
        {
            _Database.Update(Level2Error.IdsDataError);
        }

        #endregion

        #region GAC.7 - GAC-9

        /// <remarks>EMV Book C-2 GAC.7 - GAC-9</remarks>
        /// <exception cref="PlayInternalException"></exception>
        private bool TryHandlingIdsWrite(
            IGetKernelStateId currentStateIdRetriever, Kernel2Session session, Message message,
            DataStorageOperatorDataSetInfoForReader dsOdsInfoForReader, out StateId? result)
        {
            if (IsAcTypeRankedHigherThanDsAcType(session))
            {
                result = _WriteIds.Process(currentStateIdRetriever, session, message);

                return true;
            }

            if (IsDsOdsTermUsableForAcType(session, dsOdsInfoForReader))
            {
                result = _WriteIds.Process(currentStateIdRetriever, session, message);

                return true;
            }

            result = null;

            return false;
        }

        #endregion

        #region GAC.7 - GAC.8

        /// <remarks>EMV Book C-2 GAC.7 - GAC.8</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private bool IsAcTypeRankedHigherThanDsAcType(Kernel2Session session)
        {
            CryptogramTypes acType = session.GetApplicationCryptogramType();

            CryptogramType dsAcType = _Database.Get<DataStorageApplicationCryptogramType>(DataStorageApplicationCryptogramType.Tag);

            if (dsAcType == CryptogramTypes.ApplicationAuthenticationCryptogram)
            {
                UpdateAcType(dsAcType);

                return true;
            }

            if (dsAcType == acType)
            {
                UpdateAcType(dsAcType);

                return true;
            }

            if (dsAcType != CryptogramTypes.AuthorizationRequestCryptogram)
                return false;

            if (acType != CryptogramTypes.TransactionCryptogram)
                return false;

            UpdateAcType(dsAcType);

            return true;
        }

        #endregion

        #region GAC.8

        /// <remarks>EMV Book C-2 GAC.8</remarks>
        /// <exception cref="CardDataException"></exception>
        private void UpdateAcType(CryptogramType dsAcType)
        {
            _Database.Update(new DataStorageApplicationCryptogramType((byte) dsAcType));
        }

        #endregion

        #region GAC.9

        /// <remarks>EMV Book C-2 GAC.9</remarks>
        /// <exception cref="PlayInternalException"></exception>
        private bool IsDsOdsTermUsableForAcType(Kernel2Session session, DataStorageOperatorDataSetInfoForReader dsOdsInfoForReader)
        {
            if (!dsOdsInfoForReader.IsUsableForApplicationCryptogram())
                return false;

            CryptogramType acType = session.GetApplicationCryptogramType();

            if (acType == CryptogramTypes.ApplicationAuthenticationCryptogram)
                return true;

            if (acType == CryptogramTypes.AuthorizationRequestCryptogram)
                return true;

            return false;
        }

        #endregion

        #region GAC.10 - GAC13

        /// <remarks>EMV Book C-2 GAC.10 - GAC13</remarks>
        private bool TryHandlingIdsNoMatchingAc(
            IGetKernelStateId currentStateIdRetriever, Kernel2Session session, Message message,
            DataStorageOperatorDataSetInfoForReader dsOdsInfoForReader, out StateId? result)
        {
            if (!IsStopIfNoDsOdsTermSet(dsOdsInfoForReader))
            {
                result = _ReadIds.Process(currentStateIdRetriever, session, message);

                return true;
            }

            try
            {
                result = currentStateIdRetriever.GetStateId();
                _Database.Update(Level2Error.IdsNoMatchingAc);
                _Database.Update(MessageIdentifiers.ErrorUseAnotherCard);
                _Database.Update(Status.NotReady);
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
                _KernelEndpoint.Request(new StopKernelRequest(session.GetKernelSessionId()));
            }

            result = null;

            return false;
        }

        #endregion

        #region GAC.10

        /// <remarks>EMV Book C-2 GAC.10</remarks>
        /// <exception cref="PlayInternalException"></exception>
        private bool IsStopIfNoDsOdsTermSet(DataStorageOperatorDataSetInfoForReader dsOdsInfoForReader) =>
            dsOdsInfoForReader.IsStopIfNoDataStorageOperatorSetTerminalSet();

        #endregion

        #endregion
    }
}