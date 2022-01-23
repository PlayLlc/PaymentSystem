namespace Play.Icc.Emv.GetData;

public class GetDataRApduSignal : RApduSignal
{
    #region Constructor

    public GetDataRApduSignal(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Level1Error GetLevel1Error()
    {
        // Check out Status Words
        throw new NotImplementedException();
    }

    #endregion
}