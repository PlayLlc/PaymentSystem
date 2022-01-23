using System;

namespace Play.Emv.Security.Certificates;

public interface IEncipheredPublicKeyCertificate
{
    #region Instance Members

    public ReadOnlySpan<byte> GetEncipherment();

    #endregion
}