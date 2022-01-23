namespace Play.Emv.DataElements;

public interface IEncipheredPublicKeyCertificate
{
    #region Instance Members

    public ReadOnlySpan<byte> GetEncipherment();

    #endregion
}