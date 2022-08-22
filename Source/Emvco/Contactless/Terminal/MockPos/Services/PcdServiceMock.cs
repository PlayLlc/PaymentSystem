using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Pcd;

namespace MockPos.Services
{
    internal class PcdServiceMock : IProximityCouplingDeviceClient
    {
        #region Instance Members

        public void Abort()
        { }

        public void Activate()
        { }

        public void CloseSession()
        { }

        public void CloseSessionCardCheck()
        { }

        public Task<byte[]> Transceive(byte[] command) => Task.FromResult(Array.Empty<byte>());

        #endregion
    }
}