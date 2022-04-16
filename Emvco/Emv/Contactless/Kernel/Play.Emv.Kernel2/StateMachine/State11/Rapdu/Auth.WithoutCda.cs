using System;

using Play.Ber.Exceptions;
using Play.Core;
using Play.Core.Extensions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel2.Databases;
using Play.Globalization.Time.Seconds;
using Play.Icc.Exceptions;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGenerateAcResponse2
{
    private partial class AuthHandler
    {
        #region Instance Members

        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="IccProtocolException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="BerParsingException"></exception>
        /// <exception cref="Exception"></exception>
        public StateId ProcessWithoutCda(Kernel2Session session, TornRecord tempTornRecord)
        {
            // S11.70 - S11.71
            if (TryHandlingForMissingMandatoryData(session.GetKernelSessionId(), tempTornRecord!))
                return StateId;

            // S11.72 - S11.79
            return IsApplicationAuthenticationCryptogram() ? HandleAac(session, tempTornRecord!) : HandleIsNotAac(session, tempTornRecord!);
        }

        #region S11.70 - S11.71

        /// <remarks>EMV Book C-2 Section S11.70 - S11.71</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private bool TryHandlingForMissingMandatoryData(KernelSessionId sessionId, TornRecord tempTornRecord)
        {
            if (_Database.IsPresentAndNotEmpty(ApplicationCryptogram.Tag))
                return false;

            _Database.Update(Level2Error.CardDataMissing);
            _ResponseHandler.ProcessInvalidDataResponse(sessionId, tempTornRecord);

            return true;
        }

        #endregion

        #region S11.72

        /// <remarks>EMV Book C-2 Section S11.72</remarks>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private bool IsApplicationAuthenticationCryptogram()
        {
            CryptogramInformationData cid = _Database.Get<CryptogramInformationData>(CryptogramInformationData.Tag);

            return cid.GetCryptogramType() == CryptogramTypes.ApplicationAuthenticationCryptogram;
        }

        #endregion

        #region S11.73

        /// <remarks>EMV Book C-2 Section S11.73</remarks>
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

        #region S11.74, S11.76

        /// <remarks>EMV Book C-2 Section S11.74, S11.76</remarks>
        /// <exception cref="TerminalDataException"></exception>
        private bool IsCdaRequested() =>
            _Database.TryGet(ReferenceControlParameter.Tag, out ReferenceControlParameter? referenceControlParameter)
            && referenceControlParameter!.IsCdaSignatureRequested();

        #endregion

        #region S11.75

        /// <remarks>EMV Book C-2 Section S11.75</remarks>
        /// <summary>
        ///     IsApplicationAuthenticationCryptogramRequested
        /// </summary>
        /// <returns></returns>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="DataElementParsingException"></exception>
        private bool IsApplicationAuthenticationCryptogramRequested()
        {
            if (!_Database.TryGet(ReferenceControlParameter.Tag, out ReferenceControlParameter? referenceControlParameter))
                return false;

            return referenceControlParameter!.GetCryptogramType() == CryptogramTypes.ApplicationAuthenticationCryptogram;
        }

        #endregion

        #region S11.77

        /// <remarks>EMV Book C-2 Section S11.77</remarks>
        /// <summary>
        ///     HandleInvalidResponse
        /// </summary>
        /// <param name="sessionId"></param>
        /// <exception cref="TerminalDataException"></exception>
        private void HandleInvalidResponse(KernelSessionId sessionId, TornRecord tempTornRecord)
        {
            _Database.Update(Level2Error.CardDataError);
            _ResponseHandler.ProcessInvalidDataResponse(sessionId, tempTornRecord);
        }

        #endregion

        #region S11.78 - S11.79

        /// <remarks>EMV Book C-2 Section S11.78 - S11.79</remarks>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="BerParsingException"></exception>
        /// <exception cref="Exception"></exception>
        private StateId HandleRelayResistanceData(Kernel2Session session)
        {
            if (_Database.IsSet(TerminalVerificationResultCodes.RelayResistancePerformed))
                UpdateTrack2DiscretionaryData();

            return _ResponseHandler.HandleValidResponse(session);
        }

        #endregion

        #region S11.79

        /// <remarks>EMV Book C-2 Section S11.79</remarks>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="OverflowException"></exception>
        /// <exception cref="BerParsingException"></exception>
        /// <exception cref="Exception"></exception>
        private void UpdateTrack2DiscretionaryData()
        {
            if (!_Database.IsPresentAndNotEmpty(Track2EquivalentData.Tag))
                return;

            Track2EquivalentData track2EquivalentDataBuffer = _Database.Get<Track2EquivalentData>(Track2EquivalentData.Tag);
            TrackPrimaryAccountNumber pan = track2EquivalentDataBuffer.GetPrimaryAccountNumber();
            Nibble[] discretionaryDataBuffer;

            if (pan.GetCharCount() <= 16)
            {
                discretionaryDataBuffer = new Nibble[]
                {
                    0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
                    0x0, 0x0, 0x0
                };
            }
            else
                discretionaryDataBuffer = new Nibble[] {0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0};

            if (_Database.TryGet(CaPublicKeyIndex.Tag, out CaPublicKeyIndex? caPublicKeyIndex) && ((byte) caPublicKeyIndex! < 0x0A))
                discretionaryDataBuffer[0] = new Nibble((byte) caPublicKeyIndex!);

            RelayResistanceProtocolCounter? rrpCounter = _Database.Get<RelayResistanceProtocolCounter>(RelayResistanceProtocolCounter.Tag);
            discretionaryDataBuffer[1] = new Nibble((byte) rrpCounter!);

            DeviceRelayResistanceEntropy? deviceRelayResistanceEntropy =
                _Database.Get<DeviceRelayResistanceEntropy>(DeviceRelayResistanceEntropy.Tag);
            ReadOnlySpan<Nibble> deviceRelayResistanceEntropyNibbles = deviceRelayResistanceEntropy.EncodeValue()[^2..].AsNibbleArray();
            deviceRelayResistanceEntropyNibbles.CopyTo(discretionaryDataBuffer[2..]);

            if (pan.GetCharCount() <= 16)
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

        #region S11.72 - S11.77

        /// <remarks>EMV Book C-2 Section S11.72 - S11.77</remarks>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="DataElementParsingException"></exception>
        /// <exception cref="IccProtocolException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        private StateId HandleAac(Kernel2Session session, TornRecord tempTornRecord)
        {
            if (IsIdsReadFlagSet())
            {
                HandleInvalidResponse(session.GetKernelSessionId(), tempTornRecord);

                return StateId;
            }

            if (!IsApplicationAuthenticationCryptogramRequested())
                return _ResponseHandler.HandleValidResponse(session);

            if (!IsCdaRequested())
                return _ResponseHandler.HandleValidResponse(session);

            HandleInvalidResponse(session.GetKernelSessionId(), tempTornRecord);

            return StateId;
        }

        #endregion

        #region S11.74, S11.77

        /// <remarks>EMV Book C-2 Section S11.74, S11.77</remarks>
        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="BerParsingException"></exception>
        /// <exception cref="Exception"></exception>
        private StateId HandleIsNotAac(Kernel2Session session, TornRecord tempTornRecord)
        {
            if (!IsCdaRequested())
                return HandleRelayResistanceData(session);

            HandleInvalidResponse(session.GetKernelSessionId(), tempTornRecord);

            return StateId;
        }

        #endregion

        #endregion
    }
}