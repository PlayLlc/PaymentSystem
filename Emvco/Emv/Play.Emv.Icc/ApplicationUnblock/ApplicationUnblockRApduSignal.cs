namespace Play.Icc.Emv.ApplicationUnblock;

public class ApplicationUnblockRApduSignal : RApduSignal
{
    #region Constructor

    public ApplicationUnblockRApduSignal(byte[] value) : base(value)
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