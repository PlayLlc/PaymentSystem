using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Play.Core.Extensions;
using Play.Emv.Card.Account;
using Play.Emv.Security.Encryption.Ciphers;
using Play.Emv.Sessions;
using Play.Icc.DataObjects.Auxiliary;

namespace Play.Emv.Security.Encryption.Mac
{

    internal partial class MasterKeyDeriver
    {
        private static class OptionB
        { 
            #region Instance Members
            public static byte[] GetMasterKey(
                IHashGenerator sha1Hasher,
                IBlockCipher tripleDesCodec,
                CryptographicChecksum cryptogram,
                PrimaryAccountNumber accountNumber,
                [AllowNull] SequenceTraceAuditNumber? sequenceNumber)
            {
                ReadOnlySpan<byte> auxiliaryData = ConcatAuxiliaryData(accountNumber, sequenceNumber); 
                byte[] y = GetY(sha1Hasher.Generate(auxiliaryData)); 
                return tripleDesCodec.Sign(y, cryptogram.AsByteArray());
            }

            private static byte[] GetY(ReadOnlySpan<byte> value)
            {
                List<byte> unusedXNibbles = new();
                int digitOffset = 0;

                for (int i = 0; i < 20 || digitOffset < 16; i++)
                {
                    if (IsLeftNibbleADigit(value[i]))
                    {
                        UpdateYBuffer(value[i].GetLeftNibble(), digitOffset);
                        digitOffset++;
                    }
                    else
                    {
                        unusedXNibbles.Add(value[i].GetLeftNibble());
                    }

                    if (IsRightNibbleADigit(value[i]))
                    {
                        UpdateYBuffer(value[i].GetRightNibble(), digitOffset);
                        digitOffset++;
                    }
                    else
                    {
                        unusedXNibbles.Add(value[i].GetRightNibble());
                    } 
                }

                if (digitOffset != 16)
                    return GetYExtended(unusedXNibbles, digitOffset);

                return _YBuffer.ToArray();
            }

            private static void UpdateYBuffer(byte nibble, int digitOffset)
            {
                if (digitOffset.IsEven())
                    _YBuffer[digitOffset / 2] = (byte)(_YBuffer.ElementAt(digitOffset / 2) | nibble);
                else
                    _YBuffer[digitOffset / 2] = (byte)(nibble >> 4);
            }
            private static byte[] GetYExtended(IReadOnlyCollection<byte> unusedNibbles, int digitOffset)
            {
                for (int i = 0, j = digitOffset; j < 16; i++, j++)
                {
                    if (digitOffset.IsEven())
                        _YBuffer[j / 2] = _NonHexIntegerMap[unusedNibbles.ElementAt(i)];
                    else
                        _YBuffer[j / 2] |= (byte)(_NonHexIntegerMap[unusedNibbles.ElementAt(i)] >> 4);
                }

                return _YBuffer.ToArray();
            } 

            private static bool IsLeftNibbleADigit(byte value)
            {
                const byte maxValue = 0x9;

                byte result = value.GetLeftNibble();
                return result <= maxValue;
            }
            private static bool IsRightNibbleADigit(byte value)
            {
                const byte maxValue = 0x9;

                byte result = value.GetRightNibble();
                return result <= maxValue;
            }
            #endregion

            #region Instance Values

            private static readonly List<byte> _YBuffer = new();

            private static readonly Dictionary<byte, byte> _NonHexIntegerMap =
                new()
                {
                    { 0x0, 0x0 },
                    { 0x1, 0x1 },
                    { 0x2, 0x2 },
                    { 0x3, 0x3 },
                    { 0x4, 0x4 },
                    { 0x5, 0x5 },
                    { 0x6, 0x6 },
                    { 0x7, 0x7 },
                    { 0x8, 0x8 },
                    { 0x9, 0x9 },
                    { 0xA, 0x0 },
                    { 0xB, 0x1 },
                    { 0xC, 0x2 },
                    { 0xD, 0x3 },
                    { 0xE, 0x4 },
                    { 0xF, 0x5 },
                };


            #endregion
        }
    }
}
