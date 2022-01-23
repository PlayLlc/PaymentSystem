namespace Play.Icc.Emv.ApplicationBlock;

public class ApplicationBlockRApduSignal : RApduSignal
{
    #region Constructor

    public ApplicationBlockRApduSignal(byte[] value) : base(value)
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