using System;

namespace Play.Icc.Emv.FileControlInformation;

public class GetFileControlInformationRApduSignal : RApduSignal
{
    #region Constructor

    public GetFileControlInformationRApduSignal(byte[] response) : base(response)
    { }

    #endregion

    #region Instance Members

    public override Level1Error GetLevel1Error() =>

        // Check out Status Words
        throw new NotImplementedException();

    #endregion
}