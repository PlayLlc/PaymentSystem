using System;

using Play.Core;
using Play.Core.Extensions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Globalization.Time.Seconds;
using Play.Icc.Exceptions;

namespace Play.Emv.Kernel2.StateMachine
{
    public partial class S910
    {
        private partial class AuthenticationHandler
        {
            #region Instance Members

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
                    UpdateTrack2DiscretionaryData();

                return _ResponseHandler.HandleValidResponse(currentGetKernelStateId, session);
            }

            #endregion

            #region S910.39

            /// <exception cref="TerminalDataException"></exception>
            /// <exception cref="OverflowException"></exception>
            /// <exception cref="Play.Ber.Exceptions.BerParsingException"></exception>
            /// <exception cref="Exception"></exception>
            private void UpdateTrack2DiscretionaryData()
            {
                if (!_Database.IsPresentAndNotEmpty(Track2EquivalentData.Tag))
                    return;

                Track2EquivalentData track2EquivalentDataBuffer = _Database.Get<Track2EquivalentData>(Track2EquivalentData.Tag);
                Nibble[] discretionaryDataBuffer;

                if (track2EquivalentDataBuffer.GetNumberOfDigitsInPrimaryAccountNumber() <= 16)
                {
                    discretionaryDataBuffer = new Nibble[]
                    {
                        0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
                        0x0, 0x0, 0x0, 0x0, 0x0
                    };
                }
                else
                {
                    discretionaryDataBuffer = new Nibble[]
                    {
                        0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
                        0x0, 0x0
                    };
                }

                if (_Database.TryGet(CaPublicKeyIndex.Tag, out CaPublicKeyIndex? caPublicKeyIndex) && ((byte) caPublicKeyIndex! < 0x0A))
                    discretionaryDataBuffer[0] = new Nibble((byte) caPublicKeyIndex!);

                var rrpCounter = _Database.Get<RelayResistanceProtocolCounter>(RelayResistanceProtocolCounter.Tag);
                discretionaryDataBuffer[1] = new Nibble((byte) rrpCounter!);

                var deviceRelayResistanceEntropy = _Database.Get<DeviceRelayResistanceEntropy>(DeviceRelayResistanceEntropy.Tag);
                ReadOnlySpan<Nibble> deviceRelayResistanceEntropyNibbles = deviceRelayResistanceEntropy.EncodeValue()[^2..].AsNibbleArray();
                deviceRelayResistanceEntropyNibbles.CopyTo(discretionaryDataBuffer[2..]);

                if (track2EquivalentDataBuffer.GetNumberOfDigitsInPrimaryAccountNumber() <= 16)
                {
                    byte entropySeed = deviceRelayResistanceEntropy.EncodeValue()[^3];
                    discretionaryDataBuffer[4] = new Nibble((byte) (entropySeed / 100));
                    discretionaryDataBuffer[5] = new Nibble((byte) ((entropySeed / 10) % 10));
                    discretionaryDataBuffer[6] = new Nibble((byte) (entropySeed % 100));
                }

                MeasuredRelayResistanceProcessingTime time =
                    _Database.Get<MeasuredRelayResistanceProcessingTime>(MeasuredRelayResistanceProcessingTime.Tag);
                Milliseconds timeInMilliseconds = (RelaySeconds) time;

                ushort timeSeed = (ushort) ((ushort) timeInMilliseconds > 999 ? 999 : (ushort) timeInMilliseconds);

                discretionaryDataBuffer[^3] = new Nibble((byte) (timeSeed / 100));
                discretionaryDataBuffer[^2] = new Nibble((byte) ((timeSeed / 10) % 10));
                discretionaryDataBuffer[^1] = new Nibble((byte) (timeSeed % 100));

                _Database.Update(track2EquivalentDataBuffer.UpdateDiscretionaryData(discretionaryDataBuffer));
            }

            #endregion

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
        }
    }
}