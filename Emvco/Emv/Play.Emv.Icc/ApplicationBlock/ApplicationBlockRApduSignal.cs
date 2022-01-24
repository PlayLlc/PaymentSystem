namespace Play.Emv.Icc.ApplicationBlock;

public class ApplicationBlockRApduSignal : RApduSignal
{
    #region Constructor

    public ApplicationBlockRApduSignal(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Level1Error GetLevel1Error() =>
        throw

            // Check out Status Words
            new NotImplementedException();

    #endregion
}