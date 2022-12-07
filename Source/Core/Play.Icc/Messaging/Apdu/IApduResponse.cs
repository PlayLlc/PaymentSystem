namespace Play.Icc.Messaging.Apdu;

public interface IApduResponse
{
    #region Instance Members

    public byte[] GetData();
    public byte GetStatusWord1();
    public byte GetStatusWord2();

    #endregion
}