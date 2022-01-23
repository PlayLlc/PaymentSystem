namespace Play.Icc.Messaging.Apdu;

public interface IApduResponse
{
    public byte[] GetData();
    public byte GetStatusWord1();
    public byte GetStatusWord2();
}