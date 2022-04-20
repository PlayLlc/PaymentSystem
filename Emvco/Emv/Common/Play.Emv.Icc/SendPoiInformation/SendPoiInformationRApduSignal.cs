using Play.Emv.Ber;
using Play.Emv.Ber.Enums;

namespace Play.Emv.Icc;

public class SendPoiInformationRApduSignal : RApduSignal
{
    #region Constructor

    public SendPoiInformationRApduSignal(byte[] response) : base(response)
    { }

    #endregion

    #region Instance Members

    public override bool IsSuccessful() => throw new NotImplementedException();
    public Level1Error GetLevel1Error() => throw new NotImplementedException();

    #endregion
}