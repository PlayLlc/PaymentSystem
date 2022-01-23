namespace Play.Icc.Emv.FileControlInformation;

public class GetFileControlInformationRApduSignal : RApduSignal
{
    #region Constructor

    public GetFileControlInformationRApduSignal(byte[] response) : base(response)
    { }

    #endregion

    #region Instance Members

    public override Level1Error GetLevel1Error() =>
        throw

            // Check out Status Words
            new NotImplementedException();

    #endregion
}