namespace Play.Icc.Emv.PutData;

public class PutDataRApduSignal : RApduSignal
{
    #region Constructor

    public PutDataRApduSignal(byte[] value) : base(value)
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