using System;

namespace Play.Icc.Emv.SendPoiInformation;

public class SendPoiInformationRApduSignal : RApduSignal
{
    #region Constructor

    public SendPoiInformationRApduSignal(byte[] response) : base(response)
    { }

    #endregion

    #region Instance Members

    public override Level1Error GetLevel1Error() =>

        // Check out Status Words
        throw new NotImplementedException();

    #endregion
}