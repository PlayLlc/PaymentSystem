using System;

using Play.Emv.DataElements;
using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services._CardholderVerification;

public class CardholderVerificationMethodSelector : ISelectCardholderVerificationMethod
{
    #region Instance Values

    private CvmQueue _CvmQueue;

    #endregion

    #region Instance Members

    public CvmResults Process(KernelDatabase database)
    {
        ApplicationInterchangeProfile applicationInterchangeProfile =
            ApplicationInterchangeProfile.Decode(database.Get(ApplicationInterchangeProfile.Tag).EncodeValue().AsSpan());

        if (!IsOnDeviceVerificationSupported(applicationInterchangeProfile, database))
            return CreateResultForOnDeviceVerification();

        if (!IsCardholderVerificationSupported())
            return CreateResultForCardholderVerificationNotSupported();

        throw new NotImplementedException();
    }

    #region CVM.1

    public bool IsOnDeviceVerificationSupported(ApplicationInterchangeProfile aip, KernelDatabase database)
    {
        if (!aip.IsOnDeviceCardholderVerificationSupported())
            return false;

        return database.IsOnDeviceCardholderVerificationSupported();
    }

    #endregion

    #region CVM.2 - CVM.4

    public CvmResults CreateResultForOnDeviceVerification() => throw new NotImplementedException();

    #endregion

    #region CVM.5

    public bool IsCardholderVerificationSupported()
    { }

    #endregion

    #region CVM.6

    public CvmResults CreateResultForCardholderVerificationNotSupported()
    { }

    #endregion

    #endregion
}