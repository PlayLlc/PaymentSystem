namespace Play.Icc.Messaging.Apdu;

public interface IApduCommand
{
    public byte[] Serialize();
}