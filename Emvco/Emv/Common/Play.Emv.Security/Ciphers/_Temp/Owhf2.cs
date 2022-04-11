using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Core.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Encryption;
using Play.Encryption.Ciphers.Symmetric;

namespace Play.Emv.Security.Ciphers.Symmetrics
{
    /// <summary>
    ///     OWHF2 is the DES-based variant of the one-way function for computing the digest. OWHF2 computes an 8-byte output R
    ///     based on an 8-byte input PD.
    /// </summary>
    /// <remarks>EMVco Book C-2 Section 8.2</remarks>
    public class Owhf2
    {
        #region Static Metadata

        private static readonly TripleDesCodec _Codec = new(new BlockCipherConfiguration(BlockCipherMode.Cbc, BlockPaddingMode.None,
            KeySize._128, BlockSize._8, new Iso7816PlainTextPreprocessor(BlockSize._8)));

        #endregion

        #region Constructor

        public Owhf2()
        { }

        #endregion

        #region Instance Members

        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="PlayInternalException"></exception>
        /// <exception cref="OverflowException"></exception>
        public byte[] Encrypt(IReadTlvDatabase database, ReadOnlySpan<byte> message)
        {
            using SpanOwner<byte> owner = SpanOwner<byte>.Allocate(16);
            Span<byte> buffer = owner.Span;
            ResolveKey(database, buffer);

            return _Codec.Sign(message, buffer);
        }

        #endregion

        #region Key Generation

        /// <remarks>EMVco Book C-2 Section 8.2 </remarks>
        public int GetPermanentSlotIdLength(DataStorageId id) => id.GetValueByteCount();

        /// <exception cref="PlayInternalException"></exception>
        public byte[] ResolveObjectId(
            DataStorageRequestedOperatorId operatorId, DataStorageOperatorDataSetInfo info, DataStorageSlotManagementControl? control)
        {
            if (control is not null)
                return operatorId.EncodeValue();

            if (!control?.IsPermanent() ?? false)
                return operatorId.EncodeValue();

            return !info.IsVolatile() ? operatorId.EncodeValue() : new byte[8];
        }

        /// <exception cref="PlayInternalException"></exception>
        public void ResolveLeftKey(
            DataStorageId dataStorageId, DataStorageRequestedOperatorId operatorId, DataStorageOperatorDataSetInfo info,
            DataStorageSlotManagementControl? control, Span<byte> buffer)
        {
            ReadOnlySpan<byte> dataStorageIdContentOctets = dataStorageId.EncodeValue();
            ReadOnlySpan<byte> objectId = ResolveObjectId(operatorId, info, control);

            for (int i = 0; i < 6; i++)
            {
                unchecked
                {
                    buffer[i] = (byte) ((((dataStorageIdContentOctets[i - 1] / 16) * 10) + (dataStorageIdContentOctets[i - 1] % 16)) * 2);
                }
            }

            objectId[4..6].CopyTo(buffer[^2..]);
        }

        /// <exception cref="PlayInternalException"></exception>
        /// <exception cref="OverflowException"></exception>
        public void ResolveRightKey(
            DataStorageId dataStorageId, DataStorageRequestedOperatorId operatorId, DataStorageOperatorDataSetInfo info,
            DataStorageSlotManagementControl? control, Span<byte> buffer)
        {
            ReadOnlySpan<byte> dataStorageIdContentOctets = dataStorageId.EncodeValue();
            ReadOnlySpan<byte> objectId = ResolveObjectId(operatorId, info, control);
            int permanentSlotIdLength = GetPermanentSlotIdLength(dataStorageId);

            for (int i = 0; i < buffer.Length; i++)
            {
                unchecked
                {
                    buffer[i] = (byte) ((((dataStorageIdContentOctets[(permanentSlotIdLength - 6) + i] / 16) * 10)
                            + (dataStorageIdContentOctets[(permanentSlotIdLength - 6) + i] % 16))
                        * 2);
                }
            }

            objectId[^2..].CopyTo(buffer[^2..]);
        }

        /// <exception cref="TerminalDataException"></exception>
        /// <exception cref="PlayInternalException"></exception>
        /// <exception cref="OverflowException"></exception>
        private void ResolveKey(IReadTlvDatabase database, Span<byte> buffer)
        {
            DataStorageId dataStorageId = database.Get<DataStorageId>(DataStorageId.Tag);
            DataStorageRequestedOperatorId operatorId = database.Get<DataStorageRequestedOperatorId>(DataStorageRequestedOperatorId.Tag);
            DataStorageOperatorDataSetInfo info = database.Get<DataStorageOperatorDataSetInfo>(DataStorageOperatorDataSetInfo.Tag);
            database.TryGet(DataStorageSlotManagementControl.Tag, out DataStorageSlotManagementControl? control);

            ResolveLeftKey(dataStorageId, operatorId, info, control, buffer[..8]);
            ResolveRightKey(dataStorageId, operatorId, info, control, buffer[8..]);
        }

        #endregion
    }
}