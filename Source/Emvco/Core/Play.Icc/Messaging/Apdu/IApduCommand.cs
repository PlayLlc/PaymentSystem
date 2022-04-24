namespace Play.Icc.Messaging.Apdu;

public interface IApduCommand
{
    #region Serialization

    public byte[] Serialize();

    #endregion
}