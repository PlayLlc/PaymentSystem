using System;

using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services._TempNew
{
    public interface ISelecctCardholderVerificationMethod
    {
        public void Process(KernelDatabase database);
    }

    public class CardholderVerificationMethodSelector : ISelecctCardholderVerificationMethod
    {
        #region Instance Members

        public void Process(KernelDatabase database)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}