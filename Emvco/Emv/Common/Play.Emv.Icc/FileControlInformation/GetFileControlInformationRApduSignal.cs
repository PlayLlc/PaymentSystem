namespace Play.Emv.Icc;

public class GetFileControlInformationRApduSignal : RApduSignal
{
    #region Constructor

    public GetFileControlInformationRApduSignal(byte[] response) : base(response)
    { }

    public GetFileControlInformationRApduSignal(byte[] response, Level1Error level1Error) : base(response, level1Error)
    { }

    #endregion

    #region Instance Members

    public override bool IsSuccessful() => throw new NotImplementedException();

    public override Level1Error GetLevel1Error() =>
        throw

            // Check out Status Words
            new NotImplementedException();

    #endregion
}