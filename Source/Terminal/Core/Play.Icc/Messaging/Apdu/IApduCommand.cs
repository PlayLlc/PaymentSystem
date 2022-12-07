namespace Play.Icc.Messaging.Apdu;

public interface IApduCommand
{
    #region Instance Members

    public byte[] Encode();

    #endregion
}