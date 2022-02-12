namespace Play.Emv.Icc.PutData;

public class PutDataRApduSignal : RApduSignal
{
    #region Constructor

    public PutDataRApduSignal(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Level1Error GetLevel1Error() =>
        throw

            // Check out Status Words
            new NotImplementedException();

    #endregion
}