namespace Play.Icc.Emv.SendPoiInformation;

public class SendPoiInformationRApduSignal : RApduSignal
{
    #region Constructor

    public SendPoiInformationRApduSignal(byte[] response) : base(response)
    { }

    #endregion

    #region Instance Members

    public override Level1Error GetLevel1Error() =>
        throw

            // Check out Status Words
            new NotImplementedException();

    #endregion
}