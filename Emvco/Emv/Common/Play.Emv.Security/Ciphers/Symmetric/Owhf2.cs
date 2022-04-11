using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Core.Extensions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Encryption;
using Play.Encryption.Ciphers.Symmetric;

namespace Play.Emv.Security.Ciphers.Symmetric
{
    public class Owhf2 : IBlockCipher
    {
        #region Instance Values

        private readonly TripleDesCodec _Codec;
        private readonly byte[] _Key;

        #endregion

        #region Constructor

        public Owhf2(IReadTlvDatabase database)
        {
            _Codec = new TripleDesCodec(SetupCodec());

            _Key = ResolveKey(database);
        }

        #endregion

        #region Instance Members

        private static BlockCipherConfiguration SetupCodec() =>
            new(BlockCipherMode.Cbc, BlockPaddingMode.None, KeySize._128, BlockSize._8, new Iso7816PlainTextPreprocessor(BlockSize._8));

        public BlockCipherAlgorithm GetAlgorithm() => _Codec.GetAlgorithm();
        public BlockCipherMode GetCipherMode() => _Codec.GetCipherMode();
        public KeySize GetKeySize() => _Codec.GetKeySize();
        public byte[] Sign(ReadOnlySpan<byte> message, ReadOnlySpan<byte> key) => _Codec.Sign(message, key);

        #endregion

        #region Temp

        // WARNING =====================================================

        private void TempMain(IReadTlvDatabase database)
        { }

        /// <remarks>EMVco Book C-2 Section 8.2 </remarks>
        public int GetPermanentSlotIdLength(DataStorageId id) =>

            // CHECK: The spec in EMVco Book C-2 Section 8.2 doesn't specify whether we're using the full TLV encoding or just the Value. Need to verify
            id.GetValueByteCount();

        /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
        public byte[] ResolveObjectId(
            DataStorageRequestedOperatorId operatorId, DataStorageOperatorDataSetInfo info, DataStorageSlotManagementControl? control)
        {
            if (control is not null)
                return operatorId.EncodeValue();

            if (!control?.IsPermanent() ?? false)
                return operatorId.EncodeValue();

            if (!info.IsVolatile())
                return operatorId.EncodeValue();

            return new byte[8];
        }

        /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
        public byte[] ResolveLeftKey(
            DataStorageId dataStorageId, DataStorageRequestedOperatorId operatorId, DataStorageOperatorDataSetInfo info,
            DataStorageSlotManagementControl? control)
        {
            ReadOnlySpan<byte> dataStorageIdContentOctets = dataStorageId.EncodeValue();
            ReadOnlySpan<byte> objectId = ResolveObjectId(operatorId, info, control);

            byte[] result = new byte[8];

            // why is it not zero based
            for (int i = 0; i < 6; i++)
            {
                unchecked
                {
                    result[i] = (byte) ((((dataStorageIdContentOctets[i - 1] / 16) * 10) + (dataStorageIdContentOctets[i - 1] % 16)) * 2);
                }

                //
            }

            objectId[4..6].CopyTo(result[^2..]);

            return result;
        }

        /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
        public byte[] ResolveRightKey(
            DataStorageId dataStorageId, DataStorageRequestedOperatorId operatorId, DataStorageOperatorDataSetInfo info,
            DataStorageSlotManagementControl? control)
        {
            ReadOnlySpan<byte> dataStorageIdContentOctets = dataStorageId.EncodeValue();
            ReadOnlySpan<byte> objectId = ResolveObjectId(operatorId, info, control);
            int permanentSlotIdLength = GetPermanentSlotIdLength(dataStorageId);

            byte[] result = new byte[8];

            for (int i = 0; i < result.Length; i++)
            {
                unchecked
                {
                    result[i] = (byte) ((((dataStorageIdContentOctets[(permanentSlotIdLength - 6) + i] / 16) * 10)
                            + (dataStorageIdContentOctets[(permanentSlotIdLength - 6) + i] % 16))
                        * 2);
                }
            }

            objectId[^2..].CopyTo(result[^2..]);

            return result;
        }

        /// <exception cref="Ber.Exceptions.TerminalDataException"></exception>
        /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
        private byte[] ResolveKey(IReadTlvDatabase database)
        {
            byte[] key = new byte[16];

            DataStorageId dataStorageId = database.Get<DataStorageId>(DataStorageId.Tag);
            DataStorageRequestedOperatorId operatorId = database.Get<DataStorageRequestedOperatorId>(DataStorageRequestedOperatorId.Tag);
            DataStorageOperatorDataSetInfo info = database.Get<DataStorageOperatorDataSetInfo>(DataStorageOperatorDataSetInfo.Tag);
            database.TryGet(DataStorageSlotManagementControl.Tag, out DataStorageSlotManagementControl? control);

            ResolveLeftKey(dataStorageId, operatorId, info, control).AsSpan().CopyTo(key);
            ResolveRightKey(dataStorageId, operatorId, info, control).AsSpan().CopyTo(key[8..]);

            return key;
        }

        #endregion
    }
}