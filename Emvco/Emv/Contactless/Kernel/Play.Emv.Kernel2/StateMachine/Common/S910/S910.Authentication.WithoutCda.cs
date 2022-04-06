using System;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Icc.Exceptions;

namespace Play.Emv.Kernel2.StateMachine
{
    public partial class S910
    {
        private partial class AuthenticationHandler
        {
            /// <exception cref="TerminalDataException"></exception>
            /// <exception cref="DataElementParsingException"></exception>
            /// <exception cref="IccProtocolException"></exception>
            public StateId ProcessWithoutCda(
                IGetKernelStateId currentStateIdRetriever, Kernel2Session session, GenerateApplicationCryptogramResponse rapdu)
            {
                // S910.30 - S910.31
                if (TryHandlingForMissingMandatoryData(session.GetKernelSessionId()))
                    return currentStateIdRetriever.GetStateId();

                return IsApplicationAuthenticationCryptogram()
                    ? HandleAac(currentStateIdRetriever, session)
                    : HandleIsNotAac(currentStateIdRetriever, session);
            }

            #region Temp

            #region S910.33, S910.35 - S910.37

            /// <exception cref="TerminalDataException"></exception>
            /// <exception cref="DataElementParsingException"></exception>
            /// <exception cref="IccProtocolException"></exception>
            private StateId HandleAac(IGetKernelStateId currentStateIdRetriever, Kernel2Session session)
            {
                if (IsIdsReadFlagSet())
                {
                    HandleInvalidResponse(session.GetKernelSessionId());

                    return currentStateIdRetriever.GetStateId();
                }

                if (!IsApplicationAuthenticationCryptogramRequested())
                    return _ResponseHandler.HandleValidResponse(currentStateIdRetriever, session);

                if (!IsCdaRequested())
                    return _ResponseHandler.HandleValidResponse(currentStateIdRetriever, session);

                HandleInvalidResponse(session.GetKernelSessionId());

                return currentStateIdRetriever.GetStateId();
            }

            #endregion

            #region S910.34, S910.38 - S910.39

            private StateId HandleIsNotAac(IGetKernelStateId currentGetKernelStateId, Kernel2Session session)
            {
                if (!IsCdaRequested())
                    return HandleRelayResistanceData(currentGetKernelStateId, session);

                HandleInvalidResponse(session.GetKernelSessionId());

                return currentGetKernelStateId.GetStateId();
            }

            #endregion

            #endregion

            #region S910.30 - S910.31

            /// <exception cref="TerminalDataException"></exception>
            private bool TryHandlingForMissingMandatoryData(KernelSessionId sessionId)
            {
                if (_Database.IsPresentAndNotEmpty(ApplicationCryptogram.Tag))
                    return false;

                _Database.Update(Level2Error.CardDataMissing);
                _ResponseHandler.ProcessInvalidDataResponse(sessionId);

                return true;
            }

            #endregion

            #region S910.32

            /// <exception cref="TerminalDataException"></exception>
            /// <exception cref="DataElementParsingException"></exception>
            private bool IsApplicationAuthenticationCryptogram()
            {
                CryptogramInformationData cid = _Database.Get<CryptogramInformationData>(CryptogramInformationData.Tag);

                return cid.GetCryptogramType() == CryptogramTypes.ApplicationAuthenticationCryptogram;
            }

            #endregion

            #region S910.33

            /// <exception cref="TerminalDataException"></exception>
            private bool IsIdsReadFlagSet()
            {
                if (!_Database.IsIdsAndTtrImplemented())
                    return false;

                if (!_Database.TryGet(IntegratedDataStorageStatus.Tag, out IntegratedDataStorageStatus? integratedDataStorageStatus))
                    return false;

                if (!integratedDataStorageStatus!.IsReadSet())
                    return false;

                return true;
            }

            #endregion

            #region S910.34, S910.36

            /// <exception cref="TerminalDataException"></exception>
            private bool IsCdaRequested() =>
                _Database.TryGet(ReferenceControlParameter.Tag, out ReferenceControlParameter? referenceControlParameter)
                && referenceControlParameter!.IsCdaSignatureRequested();

            #endregion

            #region S910.35

            private bool IsApplicationAuthenticationCryptogramRequested()
            {
                if (!_Database.TryGet(ReferenceControlParameter.Tag, out ReferenceControlParameter? referenceControlParameter))
                    return false;

                return referenceControlParameter!.GetCryptogramType() == CryptogramTypes.ApplicationAuthenticationCryptogram;
            }

            #endregion

            #region S910.37

            private void HandleInvalidResponse(KernelSessionId sessionId)
            {
                _Database.Update(Level2Error.CardDataError);
                _ResponseHandler.ProcessInvalidDataResponse(sessionId);
            }

            #endregion

            #region S910.38 - S910.39

            private StateId HandleRelayResistanceData(IGetKernelStateId currentGetKernelStateId, Kernel2Session session)
            {
                if (_Database.IsSet(TerminalVerificationResultCodes.RelayResistancePerformed))
                    StoreRelayResistanceDataInTrack2();

                return _ResponseHandler.HandleValidResponse(currentGetKernelStateId, session);
            }

            #endregion

            #region S910.39

            private void StoreRelayResistanceDataInTrack2()
            {
                if (!_Database.IsPresentAndNotEmpty(Track2EquivalentData.Tag))
                    return;

                Track2EquivalentData track2EquivalentData = _Database.Get<Track2EquivalentData>(Track2EquivalentData.Tag);

                if (track2EquivalentData.GetNumberOfDigitsInPrimaryAccountNumber() <= 16)
                    track2EquivalentData.ZeroFillDiscretionaryDataWith13HexZeros();

                /*
                 *  IF [IsNotEmpty(TagOf(Track 2 Equivalent Data))]
	                THEN

	                    IF [Number of digits in 'Primary Account Number' in Track 2 Equivalent Data ≤ 16]
	                    THEN
	                        Replace 'Discretionary Data' in Track 2 Equivalent Data with
	                        '0000000000000' (13 hexadecimal zeroes). Pad with 'F' if needed to
	                        ensure whole bytes.
	                    ELSE
	                        Replace 'Discretionary Data' in Track 2 Equivalent Data with
	                        '0000000000' (10 hexadecimal zeroes). Pad with 'F' if needed to
	                        ensure whole bytes.
	                    ENDIF

	                    IF [IsNotEmpty(TagOf(CA Public Key Index (Card))) AND CA Public Key Index (Card) < '0A']
	                    THEN
	                        Replace the most significant digit of the 'Discretionary Data' in Track 2
	                        Equivalent Data with a digit representing CA Public Key Index (Card).
	                    ENDIF

	                    Replace the second most significant digit of the 'Discretionary Data' in Track 2
	                    Equivalent Data with a digit representing RRP Counter.

                        Convert the two least significant bytes of the Device Relay Resistance Entropy
                        from 2 byte binary to 5 digit decimal by considering the two bytes as an
                        integer in the range 0 to 65535. Replace the 5 digits of 'Discretionary Data' in
                        Track 2 Equivalent Data that follow the RRP Counter digit with that value.

                        IF [Number of digits in 'Primary Account Number' in Track 2 Equivalent Data ≤ 16]
                        THEN
                            Convert the third least significant byte of Device Relay Resistance
                            Entropy from binary to 3 digit decimal in the range 0 to 255. Replace
                            the next 3 digits of 'Discretionary Data' in Track 2 Equivalent Data
                            with that value.
                        ENDIF

                        Divide the Measured Relay Resistance Processing Time by 10 using the div
                        operator to give a count in milliseconds. If the value exceeds '03E7' (999),
                        then set the value to '03E7'. Convert this value from 2 byte binary to 3 digit
                        decimal by considering the 2 bytes as an integer. Replace the 3 least
                        significant digits of 'Discretionary Data' in Track 2 Equivalent Data with this
                        3 digit decimal value.
                    ENDIF
                 */

                throw new NotImplementedException();
            }

            #endregion
        }
    }
}