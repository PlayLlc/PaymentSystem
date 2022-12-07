using System;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Kernel.Tests.DataStorage.DigestHash;

public static class Owhf2TestsConfigurationSetup
{
    public static void RegisterDefaultConfiguration(ITlvReaderAndWriter database)
    {
        ReadOnlyMemory<byte> dataStorageIdInput = new byte[] { 11, 12, 33, 44, 55, 66, 77, 88 };
        DataStorageId dataStorageId = DataStorageId.Decode(dataStorageIdInput);
        database.Update(dataStorageId);

        ReadOnlyMemory<byte> dataStorageRequestedOperatorIdInput = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
        DataStorageRequestedOperatorId operatorId = DataStorageRequestedOperatorId.Decode(dataStorageRequestedOperatorIdInput);
        database.Update(operatorId);

        ReadOnlyMemory<byte> dataStorageOperatorDataSetInfo = new byte[] { 1 };
        DataStorageOperatorDataSetInfo info = DataStorageOperatorDataSetInfo.Decode(dataStorageOperatorDataSetInfo);
        database.Update(info);
    }
}
