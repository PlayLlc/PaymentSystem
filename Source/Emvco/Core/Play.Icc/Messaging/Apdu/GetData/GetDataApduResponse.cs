namespace Play.Icc.Messaging.Apdu.GetData;

public class GetDataApduResponse : ApduResponse
{
    #region Constructor

    public GetDataApduResponse(byte[] value) : base(value)
    { }

    #endregion
}