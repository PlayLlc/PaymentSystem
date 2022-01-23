namespace Play.Icc.Emv.InternalAuthenticate;

public class InternalAuthenticateRApduSignal : RApduSignal
{
    #region Constructor

    public InternalAuthenticateRApduSignal(byte[] value) : base(value)
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