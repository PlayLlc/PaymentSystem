namespace ___TEMP.Play.Emv.Security.Certificates;

public interface IEncipheredPublicKeyCertificate
{
    #region Instance Members

    public ReadOnlySpan<byte> GetEncipherment();

    #endregion
}