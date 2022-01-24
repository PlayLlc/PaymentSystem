namespace Play.Emv.Icc.GetData;

public class GetDataRApduSignal : RApduSignal
{
    #region Constructor

    public GetDataRApduSignal(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Level1Error GetLevel1Error() =>
        throw

            // Check out Status Words
            new NotImplementedException();

    #endregion
}